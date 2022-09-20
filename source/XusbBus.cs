using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private XusbDeviceVersion m_version;

        public XusbDeviceVersion Version
        {
            get
            {
                Debug.Assert(m_version != XusbDeviceVersion.ProcNotSupported);
                return m_version;
            }
        }

        public string DevicePath { get; }

        internal SafeObjectHandle Handle => m_handle;
        internal SafeObjectHandle AsyncHandle => m_handleAsync;

        internal XusbBus(string path)
        {
            DevicePath = path;
            m_handle = Utilities.CreateFile(path);
            m_handleAsync = Utilities.CreateFile(path, CreateFileFlags.FILE_ATTRIBUTE_NORMAL | CreateFileFlags.FILE_FLAG_OVERLAPPED);
            m_version = (XusbDeviceVersion)GetInformation().Version;
            Debug.Assert(m_version != XusbDeviceVersion.ProcNotSupported);
        }

        ~XusbBus()
        {
            Dispose(false);
        }

        public XusbBusInfo GetInformation()
        {
            int result = XusbCore.Bus_GetInformation(m_handle, out var info);
            Utilities.ThrowOnError(result);
            return info;
        }

        public bool TryGetInformation(out XusbBusInfo info)
        {
            int result = XusbCore.Bus_GetInformation(m_handle, out info);
            return result == 0;
        }

        public XusbBusInfoEx GetInformationEx(XusbBusInformationExType type = XusbBusInformationExType.Basic)
        {
           int result = XusbCore.Bus_GetInformationEx(m_handle, Version, type, out var info);
            Utilities.ThrowOnError(result);
            return info;
        }

        public XusbInputState GetDeviceInputState(byte indexOnBus)
        {
            int result = XusbCore.Device_GetInputState(m_handle, Version, indexOnBus, out var state);
            Utilities.ThrowOnError(result);
            return state;
        }

        public bool TryGetDeviceInputState(byte indexOnBus, out XusbInputState state)
        {
            int result = XusbCore.Device_GetInputState(m_handle, Version, indexOnBus, out state);
            return result == 0;
        }

        public void SetDeviceState(byte indexOnBus, XusbLedSetting ledState, XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_handle, indexOnBus, ledState, vibration);
            Utilities.ThrowOnError(result);
        }

        public void SetDeviceState(byte indexOnBus, XusbLedSetting ledState, XusbVibration vibration, XusbSetStateFlags flags)
        {
            int result = XusbCore.Device_SetState(m_handle, indexOnBus, ledState, vibration, flags);
            Utilities.ThrowOnError(result);
        }

        public void SetDeviceState(byte indexOnBus, XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_handle, indexOnBus, vibration);
            Utilities.ThrowOnError(result);
        }

        public void SetDeviceState(byte indexOnBus, XusbLedSetting ledState)
        {
            int result = XusbCore.Device_SetState(m_handle, indexOnBus, ledState);
            Utilities.ThrowOnError(result);
        }

        public bool TryGetDeviceLedState(byte indexOnBus, out XusbLedState ledState)
        {
            int result = XusbCore.Device_GetLedState(m_handle, Version, indexOnBus, out ledState);
            return result == 0;
        }

        public XusbLedState GetDeviceLedState(byte indexOnBus)
        {
            int result = XusbCore.Device_GetLedState(m_handle, Version, indexOnBus, out var ledState);
            Utilities.ThrowOnError(result);
            return ledState;
        }

        public XusbCapabilities GetDeviceCapabilities(byte indexOnBus)
        {
            int result = XusbCore.Device_GetCapabilities(m_handle, Version, indexOnBus, out var capabilities);
            Utilities.ThrowOnError(result);
            return capabilities;
        }

        public XusbBatteryInformation GetDeviceBatteryInformation(byte indexOnBus, XusbSubDevice subDevice = XusbSubDevice.Gamepad)
        {
            int result = XusbCore.Device_GetBatteryInformation(m_handle, Version, indexOnBus, out var batteryInfo, subDevice);
            Utilities.ThrowOnError(result);
            return batteryInfo;
        }

        public XusbAudioDeviceInformation GetDeviceAudioDeviceInformation(byte indexOnBus)
        {
            int result = XusbCore.Device_GetAudioDeviceInformation(m_handle, Version, indexOnBus, out var audioInfo);
            Utilities.ThrowOnError(result);
            return audioInfo;
        }

        // TODO: Things are a lot more involved than this
        // public string GetDeviceAudioDeviceString(byte indexOnBus)
        // {
        //     var audioInfo = GetDeviceAudioDeviceInformation(indexOnBus);
        //     return $"USB\\VID_{audioInfo.VendorId:X4}&PID_{audioInfo.ProductId:X4}&IA_{audioInfo.unk:X2}";
        // }

        public XusbInputState WaitForDeviceGuideButton(byte indexOnBus, byte userIndex)
        {
            int result = XusbCore.Device_WaitForGuideButton(m_handleAsync, indexOnBus, userIndex, out var inputState);
            Utilities.ThrowOnError(result);
            return inputState;
        }

        public Task<XusbInputState> WaitForDeviceGuideButtonAsync(byte indexOnBus, byte userIndex)
        {
            return Task.Run(() => WaitForDeviceGuideButton(indexOnBus, userIndex));
        }

        public XusbInputState WaitForDeviceInput(byte indexOnBus, byte userIndex)
        {
            int result = XusbCore.Device_WaitForInput(m_handleAsync, indexOnBus, userIndex, out var inputState);
            Utilities.ThrowOnError(result);
            return inputState;
        }

        public Task<XusbInputState> WaitForDeviceInputAsync(byte indexOnBus, byte userIndex)
        {
            return Task.Run(() => WaitForDeviceInput(indexOnBus, userIndex));
        }

        public void CancelWait(byte indexOnBus)
        {
            XusbCore.Device_CancelWait(indexOnBus);
        }

        public void PowerOffDevice(byte indexOnBus)
        {
            int result = XusbCore.Device_PowerOff(Handle, Version, indexOnBus);
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
