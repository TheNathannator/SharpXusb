using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpXusb
{
    public sealed class XusbDevice
    {
        private XusbBus m_bus;
        private readonly byte m_userIndex;
        private readonly byte m_indexOnBus;
        private EventWaitHandle m_waitHandle;

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

        public XusbInputState GetInputState() => m_bus.GetDeviceInputState(m_indexOnBus);
        public bool TryGetInputState(out XusbInputState state) => m_bus.TryGetDeviceInputState(m_indexOnBus, out state);

        public void SetState(XusbLedSetting ledState, XusbVibration vibration, XusbSetStateFlags flags) => m_bus.SetDeviceState(m_indexOnBus, ledState, vibration, flags);
        public void SetState(XusbLedSetting ledState, XusbVibration vibration) => m_bus.SetDeviceState(m_indexOnBus, ledState, vibration);
        public void SetState(XusbVibration vibration) => m_bus.SetDeviceState(m_indexOnBus, vibration);
        public void SetState(XusbLedSetting ledState) => m_bus.SetDeviceState(m_indexOnBus, ledState);

        public XusbLedState GetLedState() => m_bus.GetDeviceLedState(m_indexOnBus);

        public XusbCapabilities GetCapabilities() => m_bus.GetDeviceCapabilities(m_indexOnBus);

        public XusbBatteryInformation GetBatteryInformation() => m_bus.GetDeviceBatteryInformation(m_indexOnBus);
        public XusbBatteryInformation GetBatteryInformation(XusbSubDevice subDevice) => m_bus.GetDeviceBatteryInformation(m_indexOnBus, subDevice);

        public XusbAudioDeviceInformation GetAudioDeviceInformation() => m_bus.GetDeviceAudioDeviceInformation(m_indexOnBus);

        public XusbInputState WaitForGuideButton() => m_bus.WaitForDeviceGuideButton(m_indexOnBus, m_userIndex);
        public Task<XusbInputState> WaitForGuideButtonAsync() => m_bus.WaitForDeviceGuideButtonAsync(m_indexOnBus, m_userIndex);

        public XusbInputState WaitForInput() => m_bus.WaitForDeviceInput(m_indexOnBus, m_userIndex);
        public Task<XusbInputState> WaitForInputAsync() => m_bus.WaitForDeviceInputAsync(m_indexOnBus, m_userIndex);

        public void CancelWait() => m_bus.CancelWait(m_indexOnBus);

        public void PowerOff() => m_bus.PowerOffDevice(m_indexOnBus);
    }
}
