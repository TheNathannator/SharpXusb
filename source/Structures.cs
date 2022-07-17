using System;
using System.Runtime.InteropServices;

namespace SharpXusb
{
    /// <summary>
    /// Input data of an XUSB device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbGamepad
    {
        /// <summary>
        /// Button bitmask.
        /// </summary>
        [FieldOffset(0)]
        public ushort Buttons;

        /// <summary>
        /// Left trigger axis.
        /// </summary>
        [FieldOffset(2)]
        public byte LeftTrigger;

        /// <summary>
        /// Right trigger axis.
        /// </summary>
        [FieldOffset(3)]
        public byte RightTrigger;

        /// <summary>
        /// Left stick X axis.
        /// </summary>
        [FieldOffset(4)]
        public short LeftThumbX;

        /// <summary>
        /// Left stick Y axis.
        /// </summary>
        [FieldOffset(6)]
        public short LeftThumbY;

        /// <summary>
        /// Right stick X axis.
        /// </summary>
        [FieldOffset(8)]
        public short RightThumbX;

        /// <summary>
        /// Right stick Y axis.
        /// </summary>
        [FieldOffset(10)]
        public short RightThumbY;
    }

    /// <summary>
    /// Extended input data of an XUSB device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbGamepadEx
    {
        /// <summary>
        /// Button bitmask.
        /// </summary>
        [FieldOffset(0)]
        public ushort Buttons;

        /// <summary>
        /// Left trigger axis.
        /// </summary>
        [FieldOffset(2)]
        public byte LeftTrigger;

        /// <summary>
        /// Right trigger axis.
        /// </summary>
        [FieldOffset(3)]
        public byte RightTrigger;

        /// <summary>
        /// Left stick X axis.
        /// </summary>
        [FieldOffset(4)]
        public short LeftThumbX;

        /// <summary>
        /// Left stick Y axis.
        /// </summary>
        [FieldOffset(6)]
        public short LeftThumbY;

        /// <summary>
        /// Right stick X axis.
        /// </summary>
        [FieldOffset(8)]
        public short RightThumbX;

        /// <summary>
        /// Right stick Y axis.
        /// </summary>
        [FieldOffset(10)]
        public short RightThumbY;

        /// <summary>
        /// Extended input info, byte 1.
        /// </summary>
        [FieldOffset(12)]
        public byte Ext1;

        /// <summary>
        /// Extended input info, byte 2.
        /// </summary>
        [FieldOffset(13)]
        public byte Ext2;

        /// <summary>
        /// Extended input info, byte 3.
        /// </summary>
        [FieldOffset(14)]
        public byte Ext3;

        /// <summary>
        /// Extended input info, byte 4.
        /// </summary>
        [FieldOffset(15)]
        public byte Ext4;

        /// <summary>
        /// Extended input info, byte 5.
        /// </summary>
        [FieldOffset(16)]
        public byte Ext5;

        /// <summary>
        /// Extended input info, byte 6.
        /// </summary>
        [FieldOffset(17)]
        public byte Ext6;
    }

    /// <summary>
    /// Motor vibration info.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbVibration
    {
        /// <summary>
        /// Left motor strength.
        /// </summary>
        [FieldOffset(0)]
        public byte LeftMotorSpeed;

        /// <summary>
        /// Right motor strength.
        /// </summary>
        [FieldOffset(1)]
        public byte RightMotorSpeed;

        /// <summary>
        /// Initializes a new <see cref="XusbVibration"/> with the specified value for the left and right motors.
        /// </summary>
        public XusbVibration(byte left, byte right)
        {
            LeftMotorSpeed = left;
            RightMotorSpeed = right;
        }

        /// <summary>
        /// An <see cref="XusbVibration"/> with both motors set to zero.
        /// </summary>
        public readonly static XusbVibration Zero = new XusbVibration()
        {
            LeftMotorSpeed = 0,
            RightMotorSpeed = 0
        };

        /// <summary>
        /// An <see cref="XusbVibration"/> with both motors set to <see cref="byte.MaxValue"/>.
        /// </summary>
        public readonly static XusbVibration Full = new XusbVibration()
        {
            LeftMotorSpeed = byte.MaxValue,
            RightMotorSpeed = byte.MaxValue
        };

        /// <summary>
        /// An <see cref="XusbVibration"/> with the left motor set to <see cref="byte.MaxValue"/>.
        /// </summary>
        public readonly static XusbVibration LeftFull = new XusbVibration()
        {
            LeftMotorSpeed = byte.MaxValue,
            RightMotorSpeed = 0
        };

        /// <summary>
        /// An <see cref="XusbVibration"/> with the right motor set to <see cref="byte.MaxValue"/>.
        /// </summary>
        public readonly static XusbVibration RightFull = new XusbVibration()
        {
            LeftMotorSpeed = 0,
            RightMotorSpeed = byte.MaxValue
        };
    }

    /// <summary>
    /// Information about an XUSB bus.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbBusInfo
    {
        /// <summary>
        /// Version number.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Highest user index connected to the bus, plus 1.
        /// </summary>
        [FieldOffset(2)]
        public byte MaxIndex;

        /// <summary>
        /// Number of devices connected to the bus.
        /// </summary>
        [FieldOffset(3)]
        public byte DeviceCount;

        /// <summary>
        /// Unused padding.
        /// </summary>
        [FieldOffset(4)]
        public uint Padding;

        /// <summary>
        /// Vendor ID of the bus.
        /// </summary>
        [FieldOffset(8)]
        public ushort VendorId;

        /// <summary>
        /// Product ID of the bus.
        /// </summary>
        [FieldOffset(10)]
        public ushort ProductId;

        internal const int Size = 12;
    }

    /// <summary>
    /// Basic extended information about an XUSB bus.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbBusInfoEx_Basic
    {
        [FieldOffset(0)]
        public byte unk1;

        [FieldOffset(1)]
        public uint unk2;

        [FieldOffset(5)]
        public uint unk3;

        [FieldOffset(9)]
        public uint unk4;

        [FieldOffset(13)]
        public uint unk5;

        [FieldOffset(17)]
        public ushort unk6;

        [FieldOffset(19)]
        public uint unk7;

        [FieldOffset(23)]
        public byte unk8;

        [FieldOffset(24)]
        public ushort unk9;

        [FieldOffset(26)]
        public byte unk10;

        internal const int Size = 27;
    }

    /// <summary>
    /// Sub-structure for the full extended information about an XUSB bus.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbBusInfoEx_Full_Sub
    {
        [FieldOffset(0)]
        public byte unk1;

        [FieldOffset(1)]
        public byte unk2;

        [FieldOffset(2)]
        public byte unk3;

        [FieldOffset(3)]
        public byte unk4;

        [FieldOffset(4)]
        public byte unk5;

        [FieldOffset(5)]
        public byte unk6;

        [FieldOffset(6)]
        public byte unk7;

        internal const int Size = 7;
    }

    /// <summary>
    /// Full extended information about an XUSB bus.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbBusInfoEx_Full
    {
        [FieldOffset(0)]
        public byte unk1;

        [FieldOffset(1)]
        public byte unk2;

        [FieldOffset(2)]
        public byte unk3;

        /// <summary>
        /// List of 8 elements whose data is unknown.
        /// </summary>
        [FieldOffset(3)]
        // Can't do this due to the fixed keyword only working with integral types
        // private fixed XusbBusInfoEx_Full_Sub unkList[8];
        public unsafe fixed byte unkBuffer[56];

        private const int listSize = 8;

        // Have to do this instead
        /// <summary>
        /// Gets <see cref="unkBuffer"/> as an <see cref="XusbBusInfoEx_Full_Sub"/> array.
        /// </summary>
        public unsafe XusbBusInfoEx_Full_Sub[] unkList
        {
            get
            {
                fixed (byte* ptr = unkBuffer)
                {
                    XusbBusInfoEx_Full_Sub* buffer = (XusbBusInfoEx_Full_Sub*)ptr;
                    var array = new XusbBusInfoEx_Full_Sub[listSize];
                    for (int i = 0; i < listSize; i++)
                    {
                        array[i] = buffer[i];
                    }
                    return array;
                }
            }
        }

        internal const int Size = 59;
    }

    /// <summary>
    /// Minimal extended information about an XUSB bus.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbBusInfoEx_Minimal
    {
        [FieldOffset(0)]
        public uint unk1;

        [FieldOffset(4)]
        public uint unk2;

        [FieldOffset(8)]
        public uint unk3;

        [FieldOffset(12)]
        public uint unk4;

        [FieldOffset(16)]
        public ushort unk5;

        [FieldOffset(18)]
        public ushort unk6;

        [FieldOffset(20)]
        public ushort unk7;

        [FieldOffset(22)]
        public ushort unk8;

        [FieldOffset(24)]
        public byte unk9;

        [FieldOffset(25)]
        public uint unk10;

        [FieldOffset(29)]
        public ushort unk11;

        [FieldOffset(31)]
        public byte unk12;

        internal const int Size = 32;
    }

    /// <summary>
    /// Extended bus information buffer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbBusInfoEx
    {
        /// <summary>
        /// Padding.
        /// </summary>
        [FieldOffset(0)]
        public ushort Padding;

        /// <summary>
        /// Whether or not the operation failed..
        /// </summary>
        [FieldOffset(2)]
        [MarshalAs(UnmanagedType.U1)]
        public bool Failure;

        /// <summary>
        /// The number of bytes written to <see cref="Buffer"/>.
        /// </summary>
        [FieldOffset(3)]
        public ushort DataLength;

        /// <summary>
        /// Buffer of data for the extended information.
        /// </summary>
        /// <remarks>
        /// When getting basic or full info, this buffer contains an array of size 16 of 
        /// </remarks>
        [FieldOffset(5)]
        public unsafe fixed byte Buffer[944];

        /// <summary>
        /// Gets <see cref="Buffer"/> as an <see cref="XusbBusInfoEx_Minimal"/> structure.
        /// </summary>
        /// <remarks>
        /// Returns default data if no data was returned from the driver.
        /// </remarks>
        public unsafe XusbBusInfoEx_Minimal Minimal
        {
            get
            {
                if (DataLength != XusbBusInfoEx_Minimal.Size)
                {
                    if (DataLength == 0)
                    {
                        // TODO: See if there's a better way to handle this case
                        return default;
                    }
                    else
                    {
                        throw new InvalidOperationException("The buffer does not contain this type of info.");
                    }
                }

                fixed (byte* buffer = Buffer)
                {
                    XusbBusInfoEx_Minimal* typedPtr = (XusbBusInfoEx_Minimal*)buffer;
                    return *typedPtr;
                }
            }
        }

        private const int listSize = 16;

        /// <summary>
        /// Gets <see cref="Buffer"/> as an <see cref="XusbBusInfoEx_Basic"/> array.
        /// </summary>
        /// <remarks>
        /// Returns null if no data was returned from the driver.
        /// </remarks>
        public unsafe XusbBusInfoEx_Basic[] Basic
        {
            get
            {
                if (DataLength != XusbBusInfoEx_Basic.Size * listSize)
                {
                    if (DataLength == 0)
                    {
                        // TODO: See if there's a better way to handle this case
                        return null;
                    }
                    else
                    {
                        throw new InvalidOperationException("The buffer does not contain this type of info.");
                    }
                }

                fixed (byte* buffer = Buffer)
                {
                    XusbBusInfoEx_Basic* typedPtr = (XusbBusInfoEx_Basic*)buffer;
                    var array = new XusbBusInfoEx_Basic[listSize];
                    for (int i = 0; i < listSize; i++)
                    {
                        array[i] = typedPtr[i];
                    }
                    return array;
                }
            }
        }

        /// <summary>
        /// Gets <see cref="Buffer"/> as an <see cref="XusbBusInfoEx_Full"/> array.
        /// </summary>
        /// <remarks>
        /// Returns null if no data was returned from the driver.
        /// </remarks>
        public unsafe XusbBusInfoEx_Full[] Full
        {
            get
            {
                if (DataLength != XusbBusInfoEx_Full.Size * listSize)
                {
                    if (DataLength == 0)
                    {
                        // TODO: See if there's a better way to handle this case
                        return default;
                    }
                    else
                    {
                        throw new InvalidOperationException("The buffer does not contain this type of info.");
                    }
                }

                fixed (byte* buffer = Buffer)
                {
                    XusbBusInfoEx_Full* typedPtr = (XusbBusInfoEx_Full*)buffer;
                    var array = new XusbBusInfoEx_Full[listSize];
                    for (int i = 0; i < listSize; i++)
                    {
                        array[i] = typedPtr[i];
                    }
                    return array;
                }
            }
        }

        internal const int Size = 949;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbLedState
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public byte LEDState;

        internal const int Size = 3;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbInputState_0100
    {
        [FieldOffset(0)]
        public byte Status;

        [FieldOffset(1)]
        public byte unk1;

        [FieldOffset(2)]
        public byte InputId;

        [FieldOffset(3)]
        public uint PacketNumber;

        [FieldOffset(7)]
        public byte unk2;

        [FieldOffset(8)]
        public XusbGamepad Gamepad;

        internal const int Size = 20;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbInputState_0101
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public byte Status;

        [FieldOffset(3)]
        public byte unk1;

        [FieldOffset(4)]
        public byte InputId;

        [FieldOffset(5)]
        public uint PacketNumber;

        [FieldOffset(9)]
        public byte unk2;

        [FieldOffset(10)]
        public byte unk3;

        [FieldOffset(11)]
        public XusbGamepadEx Gamepad;

        internal const int Size = 29;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbInputState
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)] // Union emulation
        public XusbInputState_0100 State_0100;

        [FieldOffset(2)] // Union emulation
        public XusbInputState_0101 State_0101;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbCapabilities_0101
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public byte Type;

        [FieldOffset(3)]
        public byte SubType;

        [FieldOffset(4)]
        public XusbGamepadEx Gamepad;

        [FieldOffset(22)]
        public XusbVibration Vibration;

        internal const int Size = 24;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbCapabilities_0102
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public byte Type;

        [FieldOffset(3)]
        public byte SubType;

        [FieldOffset(4)]
        public ushort Flags;

        [FieldOffset(6)]
        public ushort VendorId;

        [FieldOffset(8)]
        public ushort ProductId;

        [FieldOffset(10)]
        public short Revision;

        [FieldOffset(12)]
        public uint XusbId;

        [FieldOffset(16)]
        public XusbGamepadEx Gamepad;

        [FieldOffset(34)]
        public XusbVibration Vibration;

        internal const int Size = 36;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbCapabilities
    {
        [FieldOffset(0)] // This value exists in both unioned types below
        public ushort Version;

        [FieldOffset(0)] // Union emulation
        public XusbCapabilities_0101 Capabilities_0101;

        [FieldOffset(0)] // Union emulation
        public XusbCapabilities_0102 Capabilities_0102;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbBatteryInformation
    {
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Type of batteries currently in use.
        /// </summary>
        [FieldOffset(2)]
        public byte Type;

        /// <summary>
        /// Current power level of batteries.
        /// </summary>
        [FieldOffset(3)]
        public byte Level;

        internal const int Size = 4;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbAudioDeviceInformation
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public ushort VendorId;

        [FieldOffset(4)]
        public ushort ProductId;

        [FieldOffset(6)]
        public byte InputId;

        internal const int Size = 7;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct XusbInputWaitState
    {
        [FieldOffset(0)]
        public ushort Version;

        [FieldOffset(2)]
        public byte Status;

        [FieldOffset(3)]
        public ulong unk1;

        [FieldOffset(11)]
        public ulong unk2;

        [FieldOffset(19)]
        public ulong unk3;

        [FieldOffset(27)]
        public ushort unk4;

        internal const int Size = 29;
    }
}
