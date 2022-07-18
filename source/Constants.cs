using System;

namespace SharpXusb
{
    public enum XusbDeviceVersion : ushort
    {
        ProcNotSupported = 0,
        v1_0 = 0x0100,
        v1_1 = 0x0101,
        v1_2 = 0x0102,
        v1_3 = 0x0103,
        v1_4 = 0x0104
    }

    public enum XusbSubDevice : byte
    {
        Gamepad = 0,
        Headset = 1
    }

    public enum XusbLedSetting : byte
    {
        Off = 0,
        Blink = 1,
        Player1_SwitchBlink = 2,
        Player2_SwitchBlink = 3,
        Player3_SwitchBlink = 4,
        Player4_SwitchBlink = 5,
        Player1 = 6,
        Player2 = 7,
        Player3 = 8,
        Player4 = 9,
        Cycle = 10,
        FastBlink = 11,
        SlowBlink = 12,
        Flipflop = 13,
        AllBlink = 14
        // Unused = 15
    }

    /// <summary>
    /// Controller type values.
    /// </summary>
    public enum XusbControllerType : byte
    {
        Gamepad = 0x01
    }

    /// <summary>
    /// Controller subtype values.
    /// </summary>
    public enum XusbControllerSubType : byte
    {
        /// <summary>
        /// Subtype is unknown.
        /// </summary>
        Unknown = 0x00,
        /// <summary>
        /// A standard gamepad.
        /// </summary>
        Gamepad = 0x01,
        /// <summary>
        /// A steering wheel.
        /// </summary>
        Wheel = 0x02,
        /// <summary>
        /// An arcade stick.
        /// </summary>
        ArcadeStick = 0x03,
        /// <summary>
        /// A flight stick.
        /// </summary>
        FlightStick = 0x04,
        /// <summary>
        /// A dance pad.
        /// </summary>
        DancePad = 0x05,
        /// <summary>
        /// A guitar peripheral.
        /// </summary>
        Guitar = 0x06,
        /// <summary>
        /// A guitar peripheral with more motion capabilities.
        /// </summary>
        GuitarAlternate = 0x07,
        /// <summary>
        /// A drumkit peripheral.
        /// </summary>
        DrumKit = 0x08,
        /// <summary>
        /// A bass guitar peripheral.
        /// </summary>
        GuitarBass = 0x0B,
        /// <summary>
        /// An arcade pad.
        /// </summary>
        ArcadePad = 0x13
    }

    /// <summary>
    /// Flags for <see cref="XusbCapabilities_0102.Flags"/>.
    /// </summary>
    [Flags]
    public enum XusbCapabilityFlags : ushort
    {
        /// <summary>
        /// No special capabilities.
        /// </summary>
        None = 0x00,
        /// <summary>
        /// Force feedback is supported.
        /// </summary>
        ForceFeedback = 0x01,
        /// <summary>
        /// Controller is wireless.
        /// </summary>
        Wireless = 0x02,
        /// <summary>
        /// Voice input is supported.
        /// </summary>
        Voice = 0x04,
        /// <summary>
        /// Plug-in modules are supported.
        /// </summary>
        PluginModules = 0x08,
        /// <summary>
        /// Controller has no navigation buttons (Start, Back, D-pad).
        /// </summary>
        NoNavigation = 0x10
    }

    /// <summary>
    /// Battery types that can be detected.
    /// </summary>
    public enum XusbBatteryType : byte
    {
        /// <summary>
        /// Device is not connected.
        /// </summary>
        Disconnected = 0x00,
        /// <summary>
        /// Device is wired and does not have a battery.
        /// </summary>
        Wired = 0x01,
        /// <summary>
        /// Device has an alkaline battery.
        /// </summary>
        Alkaline = 0x02,
        /// <summary>
        /// Device has a nickel metal hydride battery.
        /// </summary>
        NiMH = 0x03,
        /// <summary>
        /// Battery type is unknown.
        /// </summary>
        Unknown = 0xFF
    }

    /// <summary>
    /// Battery levels.
    /// </summary>
    public enum XusbBatteryLevel : byte
    {
        /// <summary>
        /// Battery level is empty.
        /// </summary>
        Empty = 0x00,
        /// <summary>
        /// Battery level is low.
        /// </summary>
        Low = 0x01,
        /// <summary>
        /// Battery level is medium.
        /// </summary>
        Medium = 0x02,
        /// <summary>
        /// Battery level is full.
        /// </summary>
        Full = 0x03
    }

    public enum XusbBusInformationExType
    {
        Basic = 0,
        Full = 1,
        Minimal = 2
    }

    internal enum XusbSetStateFlags : byte
    {
        Led = 1,
        Vibration = 2
    }
}
