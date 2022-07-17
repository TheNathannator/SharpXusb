using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpXusb
{
    public sealed class XusbDevice : IDisposable
    {
        private XusbBus m_bus;
        private readonly XusbDeviceVersion m_version;
        private readonly byte m_userIndex;
        private EventWaitHandle m_waitHandle;
        private bool m_isDisposed = false;

        public XusbBus AssociatedBus => m_bus;
        public byte UserIndex => m_userIndex;
        public XusbDeviceVersion Version => m_version;

        internal XusbDevice(XusbBus bus, byte deviceIndex)
        {
            m_bus = bus;
            m_version = bus.Version;
            m_userIndex = deviceIndex;
        }

        ~XusbDevice()
        {
            Dispose(false);
        }

        public XusbInputState GetInputState()
        {
            int result = XusbCore.Device_GetInputState(m_bus.Handle, m_version, m_userIndex, out var state);
            Utilities.ThrowOnError(result);
            return state;
        }

        public bool TryGetInputState(out XusbInputState state)
        {
            int result = XusbCore.Device_GetInputState(m_bus.Handle, m_version, m_userIndex, out state);
            return result == 0;
        }

        public void SetState(XusbLedSetting ledState, XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_bus.Handle, m_userIndex, ledState, vibration);
            Utilities.ThrowOnError(result);
        }

        public void SetState(XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_bus.Handle, m_userIndex, vibration);
            Utilities.ThrowOnError(result);
        }

        public void SetState(XusbLedSetting ledState)
        {
            int result = XusbCore.Device_SetState(m_bus.Handle, m_userIndex, ledState);
            Utilities.ThrowOnError(result);
        }

        public XusbLedState GetLedState()
        {
            int result = XusbCore.Device_GetLedState(m_bus.Handle, m_version, m_userIndex, out var ledState);
            Utilities.ThrowOnError(result);
            return ledState;
        }

        public XusbCapabilities GetCapabilities()
        {
            int result = XusbCore.Device_GetCapabilities(m_bus.Handle, m_version, m_userIndex, out var capabilities);
            Utilities.ThrowOnError(result);
            return capabilities;
        }

        public XusbBatteryInformation GetBatteryInformation()
        {
            return GetBatteryInformation(XusbSubDevice.Gamepad);
        }

        public XusbBatteryInformation GetBatteryInformation(XusbSubDevice subDevice)
        {
            int result = XusbCore.Device_GetBatteryInformation(m_bus.Handle, m_version, m_userIndex,
                out var batteryInfo, subDevice);
            Utilities.ThrowOnError(result);
            return batteryInfo;
        }

        public XusbAudioDeviceInformation GetAudioDeviceInformation()
        {
            int result = XusbCore.Device_GetAudioDeviceInformation(m_bus.Handle, m_version, m_userIndex,
                out var audioInfo);
            Utilities.ThrowOnError(result);
            return audioInfo;
        }

        public XusbInputWaitState WaitForGuideButton()
        {
            int result = XusbCore.Device_WaitForGuideButton(m_bus.AsyncHandle, CreateWaitHandle(), m_version,
                m_userIndex, out var waitState);
            CloseWaitHandle();
            Utilities.ThrowOnError(result);
            return waitState;
        }

        public async Task<XusbInputWaitState> WaitForGuideButtonAsync()
        {
            return await Task.Run(() => WaitForGuideButton());
        }

        public XusbInputWaitState WaitForInput()
        {
            int result = XusbCore.Device_WaitForInput(m_bus.AsyncHandle, CreateWaitHandle(), m_version,
                m_userIndex, out var waitState);
            CloseWaitHandle();
            Utilities.ThrowOnError(result);
            return waitState;
        }

        public async Task<XusbInputWaitState> WaitForInputAsync()
        {
            return await Task.Run(() => WaitForInput());
        }

        public void CancelWait()
        {
            CloseWaitHandle();
        }

        private EventWaitHandle CreateWaitHandle()
        {
            if (m_waitHandle != null)
            {
                throw new InvalidOperationException("The previous wait must be completed or cancelled before starting a new one.");
            }

            m_waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
            return m_waitHandle;
        }

        private void CloseWaitHandle()
        {
            m_waitHandle?.Dispose();
            m_waitHandle = null;
        }

        public void PowerOff()
        {
            int result = XusbCore.Device_PowerOff(m_bus.Handle, m_version, m_userIndex);
            if (result != Win32Error.DeviceNotConnected)
            {
                Utilities.ThrowOnError(result);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!m_isDisposed)
            {
                if (disposing)
                {
                    m_bus?.Dispose();
                    m_bus = null;

                    m_waitHandle.Dispose();
                    m_waitHandle = null;
                }

                m_isDisposed = true;
            }
        }
    }
}
