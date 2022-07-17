using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PInvoke;

namespace SharpXusb
{
    using static Kernel32;

    public sealed class XusbBus : IDisposable
    {
        private SafeObjectHandle m_handle = null;
        private SafeObjectHandle m_handleAsync = null;
        private readonly XusbDeviceVersion m_version;
        private readonly Dictionary<byte, EventWaitHandle> m_waitHandles = new Dictionary<byte, EventWaitHandle>();

        public XusbDeviceVersion Version { get; }
        public string DevicePath { get; }

        internal SafeObjectHandle Handle
        {
            get
            {
                if (m_handle is null)
                {
                    m_handle = Utilities.CreateFile(DevicePath);
                }

                return m_handle;
            }
        }

        internal SafeObjectHandle AsyncHandle
        {
            get
            {
                if (m_handleAsync is null)
                {
                    m_handleAsync = Utilities.CreateFile(DevicePath, CreateFileFlags.FILE_ATTRIBUTE_NORMAL | CreateFileFlags.FILE_FLAG_OVERLAPPED);
                }

                return m_handleAsync;
            }
        }

        internal XusbBus(string path)
        {
            DevicePath = path;
            m_handle = Utilities.CreateFile(path);
            m_handleAsync = Utilities.CreateFile(path, CreateFileFlags.FILE_ATTRIBUTE_NORMAL | CreateFileFlags.FILE_FLAG_OVERLAPPED);
            m_version = (XusbDeviceVersion)GetInformation().Version;
        }

        ~XusbBus()
        {
            Dispose(false);
        }

        private Dictionary<byte, XusbDevice> GetDevicesOnInterface()
        {
            var deviceList = new Dictionary<byte, XusbDevice>();

            var busHandle = Handle;
            var busInfo = GetInformation();

            for (byte userIndex = 0; userIndex < busInfo.MaxIndex; userIndex++)
            {
                if (XusbCore.Device_GetInputState(busHandle, m_version, userIndex, out _) != 0)
                {
                    continue;
                }

                deviceList.Add(userIndex, new XusbDevice(m_version, userIndex));
            }

            return deviceList;
        }

        public XusbBusInfo GetInformation()
        {
            int result = XusbCore.Bus_GetInformation(Handle, out var info);
            Utilities.ThrowOnError(result);
            return info;
        }

        public XusbBusInfoEx GetInformationEx(XusBusInformationExType type = XusBusInformationExType.Basic)
        {
           int result = XusbCore.Bus_GetInformationEx(Handle, m_version, type, out var info);
            Utilities.ThrowOnError(result);
            return info;
        }

        public XusbInputState GetDeviceInputState(byte userIndex)
        {
            int result = XusbCore.Device_GetInputState(m_handle, m_version, userIndex, out var state);
            Utilities.ThrowOnError(result);
            return state;
        }

        public void SetDeviceState(byte userIndex, XusbLedState ledState, XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_handle, userIndex, ledState, vibration);
            Utilities.ThrowOnError(result);
        }

        public void SetDeviceState(byte userIndex, XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_handle, userIndex, vibration);
            Utilities.ThrowOnError(result);
        }

        public void SetDeviceState(byte userIndex, XusbLedState ledState)
        {
            int result = XusbCore.Device_SetState(m_handle, userIndex, ledState);
            Utilities.ThrowOnError(result);
        }

        public XusbLedStateBuffer GetDeviceLedState(byte userIndex)
        {
            int result = XusbCore.Device_GetLedState(m_handle, m_version, userIndex, out var ledState);
            Utilities.ThrowOnError(result);
            return ledState;
        }

        public XusbCapabilities GetDeviceCapabilities(byte userIndex)
        {
            int result = XusbCore.Device_GetCapabilities(m_handle, m_version, userIndex, out var capabilities);
            Utilities.ThrowOnError(result);
            return capabilities;
        }

        public XusbBatteryInformation GetDeviceBatteryInformation(byte userIndex)
        {
            return GetDeviceBatteryInformation(userIndex, XusbSubDevice.Gamepad);
        }

        public XusbBatteryInformation GetDeviceBatteryInformation(byte userIndex, XusbSubDevice subDevice)
        {
            int result = XusbCore.Device_GetBatteryInformation(m_handle, m_version, userIndex, out var batteryInfo, subDevice);
            Utilities.ThrowOnError(result);
            return batteryInfo;
        }

        public XusbAudioDeviceInformation GetDeviceAudioDeviceInformation(byte userIndex)
        {
            int result = XusbCore.Device_GetAudioDeviceInformation(m_handle, m_version, userIndex, out var audioInfo);
            Utilities.ThrowOnError(result);
            return audioInfo;
        }

        public XusbInputWaitState WaitForDeviceGuideButton(byte userIndex)
        {
            int result = XusbCore.Device_WaitForGuideButton(m_handleAsync, CreateWaitHandle(userIndex), m_version, userIndex, out var waitState);
            Utilities.ThrowOnError(result);
            return waitState;
        }

        public async Task<XusbInputWaitState> WaitForDeviceGuideButtonAsync(byte userIndex)
        {
            return await Task.Run(() => {
                int result = XusbCore.Device_WaitForGuideButton(m_handleAsync, CreateWaitHandle(userIndex), m_version, userIndex, out var waitState);
                Utilities.ThrowOnError(result);
                return waitState;
            });
        }

        public XusbInputWaitState WaitForDeviceInput(byte userIndex)
        {
            int result = XusbCore.Device_WaitForInput(m_handleAsync, CreateWaitHandle(userIndex), m_version, userIndex, out var waitState);
            Utilities.ThrowOnError(result);
            return waitState;
        }

        public async Task<XusbInputWaitState> WaitForDeviceInputAsync(byte userIndex)
        {
            return await Task.Run(() => {
                int result = XusbCore.Device_WaitForInput(m_handleAsync, CreateWaitHandle(userIndex), m_version, userIndex, out var waitState);
                Utilities.ThrowOnError(result);
                return waitState;
            });
        }

        public void CancelWait(byte userIndex)
        {
            CloseWaitHandle(userIndex);
        }

        private EventWaitHandle CreateWaitHandle(byte userIndex)
        {
            if (m_waitHandles.ContainsKey(userIndex))
            {
                throw new InvalidOperationException("The previous wait must be completed or cancelled before starting a new one.");
            }

            m_waitHandles.Add(userIndex, new EventWaitHandle(false, EventResetMode.ManualReset));
            return m_waitHandles[userIndex];
        }

        private void CloseWaitHandle(byte userIndex)
        {
            if (m_waitHandles.ContainsKey(userIndex))
            {
                m_waitHandles[userIndex]?.Dispose();
                m_waitHandles.Remove(userIndex);
            }
        }

        public void PowerOffDevice(byte userIndex)
        {
            int result = XusbCore.Device_PowerOff(m_handle, m_version, userIndex);
            if (result != Win32Error.DeviceNotConnected)
            {
                Utilities.ThrowOnError(result);
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_handle?.Dispose();
                m_handle = null;
            }
        }
    }
}
