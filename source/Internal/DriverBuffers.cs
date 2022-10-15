using System.Runtime.InteropServices;

namespace SharpXusb
{
    /// <summary>
    /// Common buffer used in multiple queries.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct XusbBuffer_Common
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Device index to query.
        /// </summary>
        [FieldOffset(2)]
        public byte IndexOnBus;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        public const int Size = 3;
    }

    /// <summary>
    /// Buffer used for querying battery information.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct XusbBuffer_GetBatteryInformation
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Device index to query.
        /// </summary>
        [FieldOffset(2)]
        public byte IndexOnBus;

        /// <summary>
        /// Sub-device to query.
        /// </summary>
        /// <remarks>
        /// See <see cref="XusbSubDevice"/> for possible values.
        /// </remarks>
        [FieldOffset(3)]
        public byte SubDevice;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        public const int Size = 4;
    }

    /// <summary>
    /// Buffer used for setting the state of the device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct XusbBuffer_SetState
    {
        /// <summary>
        /// Device index to query.
        /// </summary>
        [FieldOffset(0)]
        public byte IndexOnBus;

        /// <summary>
        /// State to set the controller LED to.
        /// </summary>
        /// <remarks>
        /// See <see cref="XusbLedSetting"/> for possible values.
        /// </remarks>
        [FieldOffset(1)]
        public byte LedState;

        /// <summary>
        /// Speed to set the rumble motors to.
        /// </summary>
        [FieldOffset(2)]
        public XusbVibration Vibration;

        /// <summary>
        /// Flags for what parts of the state to set.
        /// </summary>
        /// <remarks>
        /// See <see cref="XusbSetStateFlags"/> for possible values.
        /// </remarks>
        [FieldOffset(4)]
        public byte Flags;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        public const int Size = 5;
    }

    /// <summary>
    /// Buffer used for requesting an input wait.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct XusbBuffer_WaitForInput
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Device index to query.
        /// </summary>
        [FieldOffset(2)]
        public byte IndexOnBus;

        /// <summary>
        /// Whether or not a dummy state should be returned.
        /// 3 will return the actual state, any other value will return a dummy state.
        /// </summary>
        [FieldOffset(3)]
        public byte DummyState;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        public const int Size = 4;
    }

    /// <summary>
    /// Buffer used for getting extended bus information.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal unsafe struct XusbBuffer_GetBusInformationEx
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Type of information request to make.
        /// </summary>
        /// <remarks>
        /// See <see cref="XusbBusInformationExType"/> for possible values.
        /// </remarks>
        [FieldOffset(2)]
        public byte RequestType;

        [FieldOffset(3)]
        public fixed byte unk[33];

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        public const int Size = 36;
    }
}
