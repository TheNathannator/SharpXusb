using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PInvoke;

namespace SharpXusb
{
    using static Kernel32;

    /// <summary>
    /// Represents an XUSB bus.
    /// </summary>
    public sealed class XusbBus : IDisposable
    {
        private SafeObjectHandle m_handle = null;
        private SafeObjectHandle m_handleAsync = null;
        private readonly XusbDeviceVersion m_version;

        /// <summary>
        /// The XUSB version reported by the bus.
        /// </summary>
        public XusbDeviceVersion Version
        {
            get
            {
                Debug.Assert(m_version != XusbDeviceVersion.ProcNotSupported);
                return m_version;
            }
        }

        /// <summary>
        /// The device path of the bus.
        /// </summary>
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

        /// <summary>
        /// Gets information about this bus.
        /// </summary>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// A non-zero error code was returned by the process.
        /// </exception>
        public XusbBusInfo GetInformation()
        {
            int result = XusbCore.Bus_GetInformation(m_handle, out var info);
            Utilities.ThrowOnError(result);
            return info;
        }

        /// <summary>
        /// Attempts to get information about this bus.
        /// </summary>
        public bool TryGetInformation(out XusbBusInfo info)
        {
            int result = XusbCore.Bus_GetInformation(m_handle, out info);
            return result == 0;
        }

        /// <summary>
        /// Gets extended information about this bus.
        /// </summary>
        public XusbBusInfoEx GetInformationEx(XusbBusInformationExType type = XusbBusInformationExType.Basic)
        {
           int result = XusbCore.Bus_GetInformationEx(m_handle, m_version, type, out var info);
            Utilities.ThrowOnError(result);
            return info;
        }

        /// <summary>
        /// Gets the input state of a device on this bus.
        /// </summary>
        public XusbInputState GetDeviceInputState(byte indexOnBus)
        {
            int result = XusbCore.Device_GetInputState(m_handle, m_version, indexOnBus, out var state);
            Utilities.ThrowOnError(result);
            return state;
        }

        /// <summary>
        /// Attempts to get the input state of a device on this bus.
        /// </summary>
        public bool TryGetDeviceInputState(byte indexOnBus, out XusbInputState state)
        {
            int result = XusbCore.Device_GetInputState(m_handle, m_version, indexOnBus, out state);
            return result == 0;
        }

        /// <summary>
        /// Sets the LED and vibration state of a device on this bus.
        /// </summary>
        public void SetDeviceState(byte indexOnBus, XusbLedSetting ledState, XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_handle, indexOnBus, ledState, vibration);
            Utilities.ThrowOnError(result);
        }

        /// <summary>
        /// Sets the LED and/or vibration state of a device on this bus.
        /// </summary>
        public void SetDeviceState(byte indexOnBus, XusbLedSetting ledState, XusbVibration vibration, XusbSetStateFlags flags)
        {
            int result = XusbCore.Device_SetState(m_handle, indexOnBus, ledState, vibration, flags);
            Utilities.ThrowOnError(result);
        }

        /// <summary>
        /// Sets the vibration state of a device on this bus.
        /// </summary>
        public void SetDeviceState(byte indexOnBus, XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_handle, indexOnBus, vibration);
            Utilities.ThrowOnError(result);
        }

        /// <summary>
        /// Sets the LED state of a device on this bus.
        /// </summary>
        public void SetDeviceState(byte indexOnBus, XusbLedSetting ledState)
        {
            int result = XusbCore.Device_SetState(m_handle, indexOnBus, ledState);
            Utilities.ThrowOnError(result);
        }

        /// <summary>
        /// Attempts to get the LED state of a device on this bus.
        /// </summary>
        public bool TryGetDeviceLedState(byte indexOnBus, out XusbLedState ledState)
        {
            int result = XusbCore.Device_GetLedState(m_handle, m_version, indexOnBus, out ledState);
            return result == 0;
        }

        /// <summary>
        /// Gets the LED state of a device on this bus.
        /// </summary>
        public XusbLedState GetDeviceLedState(byte indexOnBus)
        {
            int result = XusbCore.Device_GetLedState(m_handle, m_version, indexOnBus, out var ledState);
            Utilities.ThrowOnError(result);
            return ledState;
        }

        /// <summary>
        /// Gets the capabilities of a device on this bus.
        /// </summary>
        public XusbCapabilities GetDeviceCapabilities(byte indexOnBus)
        {
            int result = XusbCore.Device_GetCapabilities(m_handle, m_version, indexOnBus, out var capabilities);
            Utilities.ThrowOnError(result);
            return capabilities;
        }

        /// <summary>
        /// Gets the battery information of a device on this bus.
        /// </summary>
        public XusbBatteryInformation GetDeviceBatteryInformation(byte indexOnBus, XusbSubDevice subDevice = XusbSubDevice.Gamepad)
        {
            int result = XusbCore.Device_GetBatteryInformation(m_handle, m_version, indexOnBus, out var batteryInfo, subDevice);
            Utilities.ThrowOnError(result);
            return batteryInfo;
        }

        /// <summary>
        /// Gets the audio device information of a device on this bus.
        /// </summary>
        public XusbAudioDeviceInformation GetDeviceAudioDeviceInformation(byte indexOnBus)
        {
            int result = XusbCore.Device_GetAudioDeviceInformation(m_handle, m_version, indexOnBus, out var audioInfo);
            Utilities.ThrowOnError(result);
            return audioInfo;
        }

        // TODO: Things are a lot more involved than this
        // /// <summary>
        // /// Gets the audio device strings of a device on this bus.
        // /// </summary>
        // public string GetDeviceAudioDeviceString(byte indexOnBus)
        // {
        //     var audioInfo = GetDeviceAudioDeviceInformation(indexOnBus);
        //     return $"USB\\VID_{audioInfo.VendorId:X4}&PID_{audioInfo.ProductId:X4}&IA_{audioInfo.unk:X2}";
        // }

        /// <summary>
        /// Waits for an input state from a device on this bus where the guide button is active.
        /// </summary>
        public XusbInputState WaitForDeviceGuideButton(byte indexOnBus, byte userIndex)
        {
            int result = XusbCore.Device_WaitForGuideButton(m_handleAsync, indexOnBus, userIndex, out var inputState);
            Utilities.ThrowOnError(result);
            return inputState;
        }

        /// <summary>
        /// Waits asynchronously for an input state from a device on this bus where the guide button is active.
        /// </summary>
        public Task<XusbInputState> WaitForDeviceGuideButtonAsync(byte indexOnBus, byte userIndex)
        {
            return Task.Run(() => WaitForDeviceGuideButton(indexOnBus, userIndex));
        }

        // TODO
        // /// <summary>
        // /// Waits for an input state from a device on this bus.
        // /// </summary>
        // public XusbInputState WaitForDeviceInput(byte indexOnBus, byte userIndex)
        // {
        //     int result = XusbCore.Device_WaitForInput(m_handleAsync, indexOnBus, userIndex, out var inputState);
        //     Utilities.ThrowOnError(result);
        //     return inputState;
        // }

        // /// <summary>
        // /// Waits asynchronously for an input state from a device on this bus.
        // /// </summary>
        // public Task<XusbInputState> WaitForDeviceInputAsync(byte indexOnBus, byte userIndex)
        // {
        //     return Task.Run(() => WaitForDeviceInput(indexOnBus, userIndex));
        // }

        /// <summary>
        /// Cancels the input wait of a device on this bus.
        /// </summary>
        public void CancelWait(byte indexOnBus)
        {
            XusbCore.Device_CancelWait(indexOnBus);
        }

        /// <summary>
        /// Powers off a device on this bus.
        /// </summary>
        public void PowerOffDevice(byte indexOnBus)
        {
            int result = XusbCore.Device_PowerOff(Handle, m_version, indexOnBus);
            Utilities.ThrowOnError(result);
        }

        /// <summary>
        /// Disposes this bus's handles.
        /// </summary>
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

                m_handleAsync?.Dispose();
                m_handleAsync = null;
            }
        }
    }
}
