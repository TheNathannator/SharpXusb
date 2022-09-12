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
    /// Flags used in the state setting buffer sent to the driver.
    /// </summary>
    [Flags]
    internal enum XusbSetStateFlags : byte
    {
        /// <summary>
        /// Set the LED's state.
        /// </summary>
        Led = 1,
        /// <summary>
        /// Set the vibration motors' states.
        /// </summary>
        Vibration = 2,

        /// <summary>
        /// Set the state of both the LED and the vibration motors.
        /// </summary>
        Both = Led | Vibration
    }
}
