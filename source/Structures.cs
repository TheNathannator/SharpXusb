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

        /// <summary>
        /// Gets this instance as a regular <see cref="XusbGamepad"/>.
        /// </summary>
        public unsafe XusbGamepad Standard
        {
            get
            {
                fixed (XusbGamepadEx* ptr = &this)
                {
                    return *(XusbGamepad*)ptr;
                }
            }
        }
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
        /// Version of the bus.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Maximum number of devices that can be connected to the bus.
        /// </summary>
        [FieldOffset(2)]
        public byte MaxCount;

        /// <summary>
        /// Number of devices connected to the bus.
        /// </summary>
        [FieldOffset(3)]
        public byte DeviceCount;

        /// <summary>
        /// Status of the bus.
        /// </summary>
        [FieldOffset(4)]
        public byte Status;

        [FieldOffset(5)]
        public byte unk1;

        [FieldOffset(6)]
        public ushort unk2;

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

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
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

        /// <summary>
        /// Vendor ID of the bus.
        /// </summary>
        [FieldOffset(9)]
        public ushort VendorId;

        /// <summary>
        /// Product ID of the bus.
        /// </summary>
        [FieldOffset(11)]
        public ushort ProductId;

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

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
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

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
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
        /// List of info about connected devices?
        /// </summary>
        [FieldOffset(3)]
        // Can't do this due to the fixed keyword only working with integral types
        // private fixed XusbBusInfoEx_Full_Sub unkList[8];
        public unsafe fixed byte deviceList[56];

        /// <summary>
        /// The size of the list of devices(?).
        /// </summary>
        private const int listSize = 8;

        // Have to do this instead
        /// <summary>
        /// Gets <see cref="deviceList"/> as an <see cref="XusbBusInfoEx_Full_Sub"/> array.
        /// </summary>
        public unsafe XusbBusInfoEx_Full_Sub[] DeviceList
        {
            get
            {
                fixed (byte* ptr = deviceList)
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

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
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

        /// <summary>
        /// Vendor ID of the bus.
        /// </summary>
        [FieldOffset(18)]
        public ushort VendorId;

        /// <summary>
        /// Product ID of the bus.
        /// </summary>
        [FieldOffset(20)]
        public ushort ProductId;

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

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        internal const int Size = 32;
    }

    /// <summary>
    /// Extended bus information buffer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbBusInfoEx
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Whether or not the operation failed.
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
        // Privated to prevent overflows from external users
        [FieldOffset(5)]
        private unsafe fixed byte Buffer[944];

        /// <summary>
        /// Whether or not the buffer is empty.
        /// </summary>
        public bool IsEmpty => DataLength == 0;

        /// <summary>
        /// Whether or not the buffer contains minimal info.
        /// </summary>
        public bool IsMinimal => DataLength == MinimalSize;

        /// <summary>
        /// Whether or not the buffer contains basic info.
        /// </summary>
        public bool IsBasic => DataLength == BasicSize;

        /// <summary>
        /// Whether or not the buffer contains full info.
        /// </summary>
        public bool IsFull => DataLength == FullSize;

        /// <summary>
        /// Gets the data buffer as a managed byte array.
        /// </summary>
        /// <remarks>
        /// The true type of this buffer varies based on the type of request this data was received from.
        /// Use the <see cref="Minimal"/>, <see cref="Basic"/>, and <see cref="Full"/> properties to get
        /// the proper types for each respective request.
        /// </remarks>
        public unsafe byte[] RawData
        {
            get
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("The data buffer is empty.");
                }

                fixed (byte* buffer = Buffer)
                {
                    var data = new byte[DataLength];
                    for (int i = 0; i < DataLength; i++)
                    {
                        data[i] = buffer[i];
                    }
                    return data;
                }
            }
        }

        /// <summary>
        /// Gets the data buffer as an <see cref="XusbBusInfoEx_Minimal"/> structure.
        /// </summary>
        /// <remarks>
        /// Returns default data if no data was returned from the driver.
        /// </remarks>
        public unsafe XusbBusInfoEx_Minimal Minimal
        {
            get
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("The data buffer is empty.");
                }
                else if (!IsMinimal)
                {
                    throw new InvalidOperationException("The data buffer does not contain this type of info.");
                }

                fixed (byte* buffer = Buffer)
                {
                    XusbBusInfoEx_Minimal* typedPtr = (XusbBusInfoEx_Minimal*)buffer;
                    return *typedPtr;
                }
            }
        }

        /// <summary>
        /// Gets the data buffer as an <see cref="XusbBusInfoEx_Basic"/> array.
        /// </summary>
        /// <remarks>
        /// Returns null if no data was returned from the driver.
        /// </remarks>
        public unsafe XusbBusInfoEx_Basic[] Basic
        {
            get
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("The data buffer is empty.");
                }
                else if (!IsBasic)
                {
                    throw new InvalidOperationException("The data buffer does not contain this type of info.");
                }

                fixed (byte* buffer = Buffer)
                {
                    XusbBusInfoEx_Basic* typedPtr = (XusbBusInfoEx_Basic*)buffer;
                    var array = new XusbBusInfoEx_Basic[ListSize];
                    for (int i = 0; i < ListSize; i++)
                    {
                        array[i] = typedPtr[i];
                    }
                    return array;
                }
            }
        }

        /// <summary>
        /// Gets the data buffer as an <see cref="XusbBusInfoEx_Full"/> array.
        /// </summary>
        /// <remarks>
        /// Returns null if no data was returned from the driver.
        /// </remarks>
        public unsafe XusbBusInfoEx_Full[] Full
        {
            get
            {
                if (IsEmpty)
                {
                    throw new InvalidOperationException("The data buffer is empty.");
                }
                else if (!IsFull)
                {
                    throw new InvalidOperationException("The data buffer does not contain this type of info.");
                }

                fixed (byte* buffer = Buffer)
                {
                    XusbBusInfoEx_Full* typedPtr = (XusbBusInfoEx_Full*)buffer;
                    var array = new XusbBusInfoEx_Full[ListSize];
                    for (int i = 0; i < ListSize; i++)
                    {
                        array[i] = typedPtr[i];
                    }
                    return array;
                }
            }
        }

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        internal const int Size = 949;

        /// <summary>
        /// The size of the list of buses.
        /// </summary>
        internal const int ListSize = 16;

        /// <summary>
        /// Size reported when receiving minimal info.
        /// </summary>
        internal const int MinimalSize = XusbBusInfoEx_Minimal.Size;

        /// <summary>
        /// Size reported when receiving basic info.
        /// </summary>
        internal const int BasicSize = XusbBusInfoEx_Basic.Size * ListSize;

        /// <summary>
        /// Size reported when receiving full info.
        /// </summary>
        internal const int FullSize = XusbBusInfoEx_Full.Size * ListSize;
    }

    /// <summary>
    /// LED state information.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbLedState
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// State to set the controller LED to.
        /// </summary>
        /// <remarks>
        /// See <see cref="XusbLedSetting"/> for possible values.
        /// </remarks>
        [FieldOffset(2)]
        public byte LEDState;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        internal const int Size = 3;
    }

    /// <summary>
    /// State information for version v1.0.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbInputState_v0
    {
        /// <summary>
        /// The status of the device.
        /// </summary>
        /// <remarks>
        /// 1 if the device is connected, 0 if the device is not.
        /// </remarks>
        [FieldOffset(0)]
        public byte Status;

        [FieldOffset(1)]
        public byte unk1;

        [FieldOffset(2)]
        public byte unk2;

        /// <summary>
        /// Packet number for the state.
        /// </summary>
        [FieldOffset(3)]
        public uint PacketNumber;

        [FieldOffset(7)]
        public byte unk3;

        /// <summary>
        /// Input information of the state.
        /// </summary>
        [FieldOffset(8)]
        public XusbGamepad Gamepad;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        internal const int Size = 20;
    }

    /// <summary>
    /// State information for version v1.1.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbInputState_v1
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// The status of the device.
        /// </summary>
        /// <remarks>
        /// 1 if the device is connected, 0 if the device is not.
        /// </remarks>
        [FieldOffset(2)]
        public byte Status;

        [FieldOffset(3)]
        public byte unk1;

        [FieldOffset(4)]
        public byte unk2;

        /// <summary>
        /// Packet number for the state.
        /// </summary>
        [FieldOffset(5)]
        public uint PacketNumber;

        [FieldOffset(9)]
        public byte unk3;

        [FieldOffset(10)]
        public byte unk4;

        /// <summary>
        /// Input information of the state.
        /// </summary>
        [FieldOffset(11)]
        public XusbGamepadEx Gamepad;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        internal const int Size = 29;
    }

    /// <summary>
    /// State information for an XUSB device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbInputState
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Version v1.0 state information.
        /// </summary>
        [FieldOffset(2)] // Union emulation
        public XusbInputState_v0 State_v0;

        /// <summary>
        /// Version v1.1 state information.
        /// </summary>
        [FieldOffset(2)] // Union emulation
        public XusbInputState_v1 State_v1;

        /// <summary>
        /// Packet information of the state.
        /// </summary>
        public uint PacketNumber =>
            Version == (ushort)XusbDeviceVersion.v1_0 ? State_v0.PacketNumber : State_v1.PacketNumber;

        /// <summary>
        /// Input information of the state.
        /// </summary>
        public XusbGamepad Gamepad =>
            Version == (ushort)XusbDeviceVersion.v1_0 ? State_v0.Gamepad : State_v1.Gamepad.Standard;
    }

    /// <summary>
    /// Capability information for version v1.1.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbCapabilities_v1
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Device type.
        /// </summary>
        [FieldOffset(2)]
        public byte Type;

        /// <summary>
        /// Device sub-type.
        /// </summary>
        [FieldOffset(3)]
        public byte SubType;

        /// <summary>
        /// Supported inputs of the device.
        /// </summary>
        [FieldOffset(4)]
        public XusbGamepadEx Gamepad;

        /// <summary>
        /// Whether or not the device supports either vibration motor..
        /// </summary>
        [FieldOffset(22)]
        public XusbVibration Vibration;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        internal const int Size = 24;
    }

    /// <summary>
    /// Capability information for version v1.2.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbCapabilities_v2
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Device type.
        /// </summary>
        [FieldOffset(2)]
        public byte Type;

        /// <summary>
        /// Device sub-type.
        /// </summary>
        [FieldOffset(3)]
        public byte SubType;

        /// <summary>
        /// Capability flags.
        /// </summary>
        [FieldOffset(4)]
        public ushort Flags;

        /// <summary>
        /// Vendor ID of the device.
        /// </summary>
        [FieldOffset(6)]
        public ushort VendorId;

        /// <summary>
        /// Product ID of the device.
        /// </summary>
        [FieldOffset(8)]
        public ushort ProductId;

        /// <summary>
        /// Device revision number.
        /// </summary>
        [FieldOffset(10)]
        public ushort Revision;

        /// <summary>
        /// Serial number of the device.
        /// </summary>
        [FieldOffset(12)]
        public uint SerialNumber;

        /// <summary>
        /// Supported inputs of the device.
        /// </summary>
        [FieldOffset(16)]
        public XusbGamepadEx Gamepad;

        /// <summary>
        /// Whether or not the device supports either vibration motor..
        /// </summary>
        [FieldOffset(34)]
        public XusbVibration Vibration;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        internal const int Size = 36;
    }

    /// <summary>
    /// Capability information for an XUSB device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbCapabilities
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)] // This value exists in both unioned types below
        public ushort Version;

        /// <summary>
        /// Version v1.1 capability information.
        /// </summary>
        [FieldOffset(0)] // Union emulation
        public XusbCapabilities_v1 Capabilities_v1;

        /// <summary>
        /// Version v1.2 capability information.
        /// </summary>
        [FieldOffset(0)] // Union emulation
        public XusbCapabilities_v2 Capabilities_v2;

        /// <summary>
        /// Device type.
        /// </summary>
        [FieldOffset(2)] // This value exists in both unioned types
        public byte Type;

        /// <summary>
        /// Device sub-type.
        /// </summary>
        [FieldOffset(3)] // This value exists in both unioned types
        public byte SubType;

        /// <summary>
        /// Supported inputs of the device.
        /// </summary>
        public XusbGamepadEx Gamepad =>
            Version == (ushort)XusbDeviceVersion.v1_1 ? Capabilities_v1.Gamepad : Capabilities_v2.Gamepad;

        /// <summary>
        /// Whether or not the device supports either vibration motor..
        /// </summary>
        public XusbVibration Vibration =>
            Version == (ushort)XusbDeviceVersion.v1_1 ? Capabilities_v1.Vibration : Capabilities_v2.Vibration;
    }

    /// <summary>
    /// Battery information for an XUSB device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbBatteryInformation
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Type of power source currently in use.
        /// </summary>
        [FieldOffset(2)]
        public byte Type;

        /// <summary>
        /// Current power level of the device.
        /// </summary>
        [FieldOffset(3)]
        public byte Level;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        internal const int Size = 4;
    }

    /// <summary>
    /// Audio device information for an XUSB device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct XusbAudioDeviceInformation
    {
        /// <summary>
        /// Version of the query.
        /// </summary>
        [FieldOffset(0)]
        public ushort Version;

        /// <summary>
        /// Vendor ID of the audio device.
        /// </summary>
        [FieldOffset(2)]
        public ushort VendorId;

        /// <summary>
        /// Product ID of the audio device.
        /// </summary>
        [FieldOffset(4)]
        public ushort ProductId;

        /// <summary>
        /// Input(?) ID of the audio device.
        /// </summary>
        [FieldOffset(6)]
        public byte unk;

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        internal const int Size = 7;
    }
}
