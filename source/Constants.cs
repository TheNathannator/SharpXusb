using System;

namespace SharpXusb
{
    /// <summary>
    /// Protocol version numbers, so to speak.
    /// </summary>
    /// <remarks>
    /// These aren't strictly version numbers, it's just the best word that describes what they're used for.
    /// </remarks>
    public enum XusbDeviceVersion : ushort
    {
        /// <summary>
        /// The called procedure is not supported by this device.
        /// </summary>
        ProcNotSupported = 0,
        /// <summary>
        /// Version v1.0.
        /// </summary>
        v1_0 = 0x0100,
        /// <summary>
        /// Version v1.1.
        /// </summary>
        v1_1 = 0x0101,
        /// <summary>
        /// Version v1.2.
        /// </summary>
        v1_2 = 0x0102,
        /// <summary>
        /// Version v1.3.
        /// </summary>
        v1_3 = 0x0103,
        /// <summary>
        /// Version v1.4.
        /// </summary>
        v1_4 = 0x0104
    }

    /// <summary>
    /// Button bitmask values.
    /// </summary>
    [Flags]
    public enum XusbButton : ushort
    {
        DpadUp = 0x0001,
        DpadDown = 0x0002,
        DpadLeft = 0x0004,
        DpadRight = 0x0008,
        Start = 0x0010,
        Back = 0x0020,
        LeftThumb = 0x0040,
        RightThumb = 0x0080,
        LeftShoulder = 0x0100,
        RightShoulder = 0x0200,
        Guide = 0x0400,
        Sync = 0x0800,
        A = 0x1000,
        B = 0x2000,
        X = 0x4000,
        Y = 0x8000
    }

    /// <summary>
    /// Subdevices that can be queried.
    /// </summary>
    public enum XusbSubDevice : byte
    {
        /// <summary>
        /// The main device.
        /// </summary>
        Gamepad = 0,
        /// <summary>
        /// The headset attached to the main device.
        /// </summary>
        Headset = 1
    }

    /// <summary>
    /// States that the player LED can be set to.
    /// </summary>
    public enum XusbLedSetting : byte
    {
        /// <summary>
        /// Turns all 4 lights off.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Blinks all 4 lights quickly once roughly every 2/3 of a second for two seconds, then returns to the previous LED state.
        /// </summary>
        ShortBlink = 1,
        /// <summary>
        /// Switches to the player 1 light and blinks it once roughly every 2/3 of a second for two seconds.
        /// </summary>
        Player1_Blink = 2,
        /// <summary>
        /// Switches to the player 2 light and blinks it once roughly every 2/3 of a second for two seconds.
        /// </summary>
        Player2_Blink = 3,
        /// <summary>
        /// Switches to the player 3 light and blinks it once roughly every 2/3 of a second for two seconds.
        /// </summary>
        Player3_Blink = 4,
        /// <summary>
        /// Switches to the player 4 light and blinks it once roughly every 2/3 of a second for two seconds.
        /// </summary>
        Player4_Blink = 5,
        /// <summary>
        /// Switches to the player 1 light.
        /// </summary>
        Player1 = 6,
        /// <summary>
        /// Switches to the player 2 light.
        /// </summary>
        Player2 = 7,
        /// <summary>
        /// Switches to the player 3 light.
        /// </summary>
        Player3 = 8,
        /// <summary>
        /// Switches to the player 4 light.
        /// </summary>
        Player4 = 9,
        /// <summary>
        /// Cycles through all of the player LEDs in a clockwise motion (used as the syncing indicator).
        /// </summary>
        /// <remarks>
        /// This state cannot be returned to by states that return to the previous state.
        /// </remarks>
        Cycle = 10,
        /// <summary>
        /// Blinks the active player light(s) once roughly every 2/3 of a second for roughly 8 seconds, then returns to the previous state.
        /// </summary>
        FastBlink = 11,
        /// <summary>
        /// Blinks the active player light(s) once every 3 seconds infinitely.
        /// </summary>
        SlowBlink = 12,
        /// <summary>
        /// Lights switch back and forth between 1 + 4 and 2 + 3 being enabled (used as the low battery indicator).
        /// </summary>
        /// <remarks>
        /// This state cannot be returned to by states that return to the previous state.
        /// </remarks>
        Flipflop = 13,
        /// <summary>
        /// Blinks all 4 player lights once every second infinitely.
        /// </summary>
        AllBlink = 14,
        /// <summary>
        /// Turns all 4 player lights on for a second, then turns them all off.
        /// </summary>
        FlashToOff = 15
    }

    /// <summary>
    /// Controller type values.
    /// </summary>
    public enum XusbControllerType : byte
    {
        /// <summary>
        /// A gamepad-compatible input device.
        /// </summary>
        Gamepad = 1
    }

    /// <summary>
    /// Controller subtype values.
    /// </summary>
    public enum XusbControllerSubType : byte
    {
        /// <summary>
        /// Subtype is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// A standard gamepad.
        /// </summary>
        Gamepad = 1,
        /// <summary>
        /// A steering wheel.
        /// </summary>
        Wheel = 2,
        /// <summary>
        /// An arcade stick.
        /// </summary>
        ArcadeStick = 3,
        /// <summary>
        /// A flight stick.
        /// </summary>
        FlightStick = 4,
        /// <summary>
        /// A dance pad.
        /// </summary>
        DancePad = 5,
        /// <summary>
        /// A guitar peripheral.
        /// </summary>
        Guitar = 6,
        /// <summary>
        /// A guitar peripheral with more motion capabilities.
        /// </summary>
        GuitarAlternate = 7,
        /// <summary>
        /// A drumkit peripheral.
        /// </summary>
        DrumKit = 8,
        /// <summary>
        /// A bass guitar peripheral.
        /// </summary>
        GuitarBass = 11,
        /// <summary>
        /// An arcade pad.
        /// </summary>
        ArcadePad = 19,
    }

    /// <summary>
    /// Flags for <see cref="XusbCapabilities_v2.Flags"/>.
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
        Disconnected = 0,
        /// <summary>
        /// Device is wired and does not have a battery.
        /// </summary>
        Wired = 1,
        /// <summary>
        /// Device has an alkaline battery.
        /// </summary>
        Alkaline = 2,
        /// <summary>
        /// Device has a nickel metal hydride battery.
        /// </summary>
        NiMH = 3,
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
        Empty = 0,
        /// <summary>
        /// Battery level is low.
        /// </summary>
        Low = 1,
        /// <summary>
        /// Battery level is medium.
        /// </summary>
        Medium = 2,
        /// <summary>
        /// Battery level is full.
        /// </summary>
        Full = 3
    }

    /// <summary>
    /// Types of requests that can be made for extended bus information.
    /// </summary>
    public enum XusbBusInformationExType
    {
        /// <summary>
        /// Gets basic information for all connected buses.
        /// </summary>
        Basic = 0,
        /// <summary>
        /// Gets all available information for all connected buses.
        /// </summary>
        Full = 1,
        /// <summary>
        /// Gets information for just the current bus.
        /// </summary>
        Minimal = 2
    }

    /// <summary>
    /// Flags used when setting device state.
    /// </summary>
    [Flags]
    public enum XusbSetStateFlags : byte
    {
        /// <summary>
        /// Set the LED's state.
        /// </summary>
        Led = 0x01,
        /// <summary>
        /// Set the vibration motors' states.
        /// </summary>
        Vibration = 0x02,

        /// <summary>
        /// Set the state of both the LED and the vibration motors.
        /// </summary>
        Both = Led | Vibration
    }
}
