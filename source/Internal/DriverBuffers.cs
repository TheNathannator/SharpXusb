using System.Runtime.InteropServices;

namespace SharpXusb
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct XusbBuffer_Common
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public byte DeviceIndex;

        public const int Size = 3;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct XusbBuffer_GetBatteryInformation
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public byte DeviceIndex;

        [FieldOffset(3)]
        public byte SubDevice;

        public const int Size = 4;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct XusbBuffer_SetState
    {
        [FieldOffset(0)]
        public byte DeviceIndex;

        [FieldOffset(1)]
        public byte LedState;

        [FieldOffset(2)]
        public XusbVibration Vibration;

        [FieldOffset(4)]
        public byte Flags;

        public const int Size = 5;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct XusbBuffer_WaitForInput
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public byte DeviceIndex;

        [FieldOffset(3)]
        public byte unk;

        public const int Size = 4;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct XusbBuffer_GetBusInformationEx
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public byte RequestType;

        [FieldOffset(3)]
        public fixed byte unk[33];

        public const int Size = 36;
    }
}
