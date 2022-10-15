using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpXusb
{
    /// <summary>
    /// Represents an XUSB device.
    /// </summary>
    public sealed class XusbDevice
    {
        private readonly XusbBus m_bus;
        private readonly byte m_userIndex;
        private readonly byte m_indexOnBus;

        /// <summary>
        /// The bus associated with this device.
        /// </summary>
        public XusbBus AssociatedBus => m_bus;
        /// <summary>
        /// This device's global user index.
        /// </summary>
        public byte UserIndex => m_userIndex;
        /// <summary>
        /// This device's index on its associated bus.
        /// </summary>
        public byte IndexOnBus => m_indexOnBus;
        /// <summary>
        /// This device's XUSB version.
        /// </summary>
        public XusbDeviceVersion Version => m_bus.Version;

        internal XusbDevice(XusbBus bus, byte deviceIndex, byte indexOnBus)
        {
            m_bus = bus;
            m_userIndex = deviceIndex;
            m_indexOnBus = indexOnBus;
        }

        /// <summary>
        /// Gets this device's input state.
        /// </summary>
        public XusbInputState GetInputState() => m_bus.GetDeviceInputState(m_indexOnBus);

        /// <summary>
        /// Attempts to get this device's input state.
        /// </summary>
        public bool TryGetInputState(out XusbInputState state) => m_bus.TryGetDeviceInputState(m_indexOnBus, out state);

        /// <summary>
        /// Sets this device's LED and/or vibration state.
        /// </summary>
        public void SetState(XusbLedSetting ledState, XusbVibration vibration, XusbSetStateFlags flags) => m_bus.SetDeviceState(m_indexOnBus, ledState, vibration, flags);

        /// <summary>
        /// Sets this device's LED and vibration state.
        /// </summary>
        public void SetState(XusbLedSetting ledState, XusbVibration vibration) => m_bus.SetDeviceState(m_indexOnBus, ledState, vibration);

        /// <summary>
        /// Sets this device's vibration state.
        /// </summary>
        public void SetState(XusbVibration vibration) => m_bus.SetDeviceState(m_indexOnBus, vibration);

        /// <summary>
        /// Sets this device's LED state.
        /// </summary>
        public void SetState(XusbLedSetting ledState) => m_bus.SetDeviceState(m_indexOnBus, ledState);

        /// <summary>
        /// Gets this device's LED state.
        /// </summary>
        public XusbLedState GetLedState() => m_bus.GetDeviceLedState(m_indexOnBus);

        /// <summary>
        /// Gets this device's capabilities.
        /// </summary>
        public XusbCapabilities GetCapabilities() => m_bus.GetDeviceCapabilities(m_indexOnBus);

        /// <summary>
        /// Gets this device's battery information.
        /// </summary>
        public XusbBatteryInformation GetBatteryInformation(XusbSubDevice subDevice = XusbSubDevice.Gamepad) => m_bus.GetDeviceBatteryInformation(m_indexOnBus, subDevice);

        /// <summary>
        /// Gets this device's audio device information.
        /// </summary>
        public XusbAudioDeviceInformation GetAudioDeviceInformation() => m_bus.GetDeviceAudioDeviceInformation(m_indexOnBus);

        /// <summary>
        /// Waits for an input state from this device where the guide button is pressed.
        /// </summary>
        public XusbInputState WaitForGuideButton() => m_bus.WaitForDeviceGuideButton(m_indexOnBus, m_userIndex);

        /// <summary>
        /// Waits asynchronously for an input state from this device where the guide button is pressed.
        /// </summary>
        public Task<XusbInputState> WaitForGuideButtonAsync() => m_bus.WaitForDeviceGuideButtonAsync(m_indexOnBus, m_userIndex);

        /// <summary>
        /// Waits for an input state from this device where the guide button is pressed.
        /// </summary>
        public XusbInputState WaitForInput() => m_bus.WaitForDeviceInput(m_indexOnBus, m_userIndex);

        /// <summary>
        /// Waits asynchronously for an input state from this device where the guide button is pressed.
        /// </summary>
        public Task<XusbInputState> WaitForInputAsync() => m_bus.WaitForDeviceInputAsync(m_indexOnBus, m_userIndex);

        /// <summary>
        /// Cancels an input wait for this device.
        /// </summary>
        public void CancelWait() => m_bus.CancelWait(m_indexOnBus);

        /// <summary>
        /// Powers off this device.
        /// </summary>
        public void PowerOff() => m_bus.PowerOffDevice(m_indexOnBus);
    }
}
