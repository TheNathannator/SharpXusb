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

    public enum XusBusInformationExType
    {
        Basic = 0,
        Full = 1,
        Minimal = 2
    }

    internal static class XusbSetStateFlags
    {
        public const byte
        Led = 1,
        Vibration = 2;
    }
}
