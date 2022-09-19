using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpXusb
{
    public sealed class XusbDevice : IDisposable
    {
        private XusbBus m_bus;
        private readonly byte m_userIndex;
        private readonly byte m_indexOnBus;
        private EventWaitHandle m_waitHandle;
        private bool m_isDisposed = false;

        public XusbBus AssociatedBus => m_bus;
        public byte UserIndex => m_userIndex;
        public byte IndexOnBus => m_indexOnBus;
        public XusbDeviceVersion Version => m_bus.Version;

        internal XusbDevice(XusbBus bus, byte deviceIndex, byte indexOnBus)
        {
            m_bus = bus;
            m_userIndex = deviceIndex;
            m_indexOnBus = indexOnBus;
        }

        ~XusbDevice()
        {
            Dispose(false);
        }

        public XusbInputState GetInputState()
        {
            int result = XusbCore.Device_GetInputState(m_bus.Handle, Version, m_indexOnBus, out var state);
            Utilities.ThrowOnError(result);
            return state;
        }

        public bool TryGetInputState(out XusbInputState state)
        {
            int result = XusbCore.Device_GetInputState(m_bus.Handle, Version, m_indexOnBus, out state);
            return result == Win32Error.Success;
        }

        public void SetState(XusbLedSetting ledState, XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_bus.Handle, m_indexOnBus, ledState, vibration);
            Utilities.ThrowOnError(result);
        }

        public void SetState(XusbLedSetting ledState, XusbVibration vibration, XusbSetStateFlags flags)
        {
            int result = XusbCore.Device_SetState(m_bus.Handle, m_indexOnBus, ledState, vibration, flags);
            Utilities.ThrowOnError(result);
        }

        public void SetState(XusbVibration vibration)
        {
            int result = XusbCore.Device_SetState(m_bus.Handle, m_indexOnBus, vibration);
            Utilities.ThrowOnError(result);
        }

        public void SetState(XusbLedSetting ledState)
        {
            int result = XusbCore.Device_SetState(m_bus.Handle, m_indexOnBus, ledState);
            Utilities.ThrowOnError(result);
        }

        public XusbLedState GetLedState()
        {
            int result = XusbCore.Device_GetLedState(m_bus.Handle, Version, m_indexOnBus, out var ledState);
            Utilities.ThrowOnError(result);
            return ledState;
        }

        public XusbCapabilities GetCapabilities()
        {
            int result = XusbCore.Device_GetCapabilities(m_bus.Handle, Version, m_indexOnBus, out var capabilities);
            Utilities.ThrowOnError(result);
            return capabilities;
        }

        public XusbBatteryInformation GetBatteryInformation()
        {
            return GetBatteryInformation(XusbSubDevice.Gamepad);
        }

        public XusbBatteryInformation GetBatteryInformation(XusbSubDevice subDevice)
        {
            int result = XusbCore.Device_GetBatteryInformation(m_bus.Handle, Version, m_indexOnBus,
                out var batteryInfo, subDevice);
            Utilities.ThrowOnError(result);
            return batteryInfo;
        }

        public XusbAudioDeviceInformation GetAudioDeviceInformation()
        {
            int result = XusbCore.Device_GetAudioDeviceInformation(m_bus.Handle, Version, m_indexOnBus,
                out var audioInfo);
            Utilities.ThrowOnError(result);
            return audioInfo;
        }

        public XusbInputState WaitForGuideButton()
        {
            int result = XusbCore.Device_WaitForGuideButton(m_bus.AsyncHandle, m_indexOnBus, out var inputState);
            Utilities.ThrowOnError(result);
            return inputState;
        }

        public Task<XusbInputState> WaitForGuideButtonAsync()
        {
            return Task.Run(() => WaitForGuideButton());
        }

        public XusbInputState WaitForInput()
        {
            int result = XusbCore.Device_WaitForInput(m_bus.AsyncHandle, m_indexOnBus, out var inputState);
            Utilities.ThrowOnError(result);
            return inputState;
        }

        public Task<XusbInputState> WaitForInputAsync()
        {
            return Task.Run(() => WaitForInput());
        }

        public void CancelWait()
        {
            XusbCore.Device_CancelWait(m_indexOnBus);
        }

        public void PowerOff()
        {
            int result = XusbCore.Device_PowerOff(m_bus.Handle, Version, m_indexOnBus);
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
