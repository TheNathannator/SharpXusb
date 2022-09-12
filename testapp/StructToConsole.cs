using System;
using SharpXusb;

namespace SharpXusbTestApp
{
    static class StructToConsole
    {
        public static void ToConsole(this XusbBusInfo busInfo)
        {
            Console.WriteLine($"Version:      0x{busInfo.Version:X4}");
            Console.WriteLine($"MaxCount:     {busInfo.MaxCount}");
            Console.WriteLine($"DeviceCount:  {busInfo.DeviceCount}");
            Console.WriteLine($"Status:       0x{busInfo.Status:X2}");
            Console.WriteLine($"unk1:         0x{busInfo.unk1:X2}");
            Console.WriteLine($"unk2:         0x{busInfo.unk2:X4}");
            Console.WriteLine($"VendorId:     0x{busInfo.VendorId:X4}");
            Console.WriteLine($"ProductId:    0x{busInfo.ProductId:X4}");
        }

        public static void ToConsole(this XusbBusInfoEx busInfo, XusbBusInformationExType type)
        {
            Console.WriteLine($"Version:     0x{busInfo.Version:X4}");
            Console.WriteLine($"Failure:     {busInfo.Failure}");
            Console.WriteLine($"DataLength:  {busInfo.DataLength}");

            if (busInfo.DataLength == 0)
            {
                Console.WriteLine($"Could not retrieve further extended bus information.");
                return;
            }

            switch (type)
            {
                case XusbBusInformationExType.Minimal:
                {
                    var minimal = busInfo.Minimal;
                    Console.WriteLine($"unk1:        0x{minimal.unk1:X8}");
                    Console.WriteLine($"unk2:        0x{minimal.unk2:X8}");
                    Console.WriteLine($"unk3:        0x{minimal.unk3:X8}");
                    Console.WriteLine($"unk4:        0x{minimal.unk4:X8}");
                    Console.WriteLine($"unk5:        0x{minimal.unk5:X4}");
                    Console.WriteLine($"VendorId:    0x{minimal.VendorId:X4}");
                    Console.WriteLine($"ProductId:   0x{minimal.ProductId:X4}");
                    Console.WriteLine($"unk8:        0x{minimal.unk8:X4}");
                    Console.WriteLine($"unk9:        0x{minimal.unk9:X2}");
                    Console.WriteLine($"unk10:       0x{minimal.unk10:X8}");
                    Console.WriteLine($"unk11:       0x{minimal.unk11:X4}");
                    Console.WriteLine($"unk12:       0x{minimal.unk12:X2}");
                    break;
                }

                case XusbBusInformationExType.Basic:
                {
                    var basic = busInfo.Basic;
                    if (basic is null)
                    {
                        Console.WriteLine("No further data supplied by the driver.");
                        return;
                    }

                    for (int i = 0; i < basic.Length; i++)
                    {
                        Console.WriteLine($"Basic[{i}]:");

                        var element = basic[i];
                        Console.WriteLine($"  unk1:       0x{element.unk1:X2}");
                        Console.WriteLine($"  unk2:       0x{element.unk2:X8}");
                        Console.WriteLine($"  unk3:       0x{element.unk3:X8}");
                        Console.WriteLine($"  VendorId:   0x{element.VendorId:X4}");
                        Console.WriteLine($"  ProductId:  0x{element.ProductId:X4}");
                        Console.WriteLine($"  unk5:       0x{element.unk5:X8}");
                        Console.WriteLine($"  unk6:       0x{element.unk6:X4}");
                        Console.WriteLine($"  unk7:       0x{element.unk7:X8}");
                        Console.WriteLine($"  unk8:       0x{element.unk8:X2}");
                        Console.WriteLine($"  unk9:       0x{element.unk9:X4}");
                        Console.WriteLine($"  unk10:      0x{element.unk10:X2}");
                    }
                    break;
                }

                case XusbBusInformationExType.Full:
                {
                    var full = busInfo.Full;
                    if (full is null)
                    {
                        Console.WriteLine("No further data supplied by the driver.");
                        return;
                    }

                    for (int i = 0; i < full.Length; i++)
                    {
                        Console.WriteLine($"Full[{i}]:");

                        var busElement = full[i];
                        Console.WriteLine($"  unk1:  0x{busElement.unk1:X2}");
                        Console.WriteLine($"  unk2:  0x{busElement.unk2:X2}");
                        Console.WriteLine($"  unk3:  0x{busElement.unk3:X2}");

                        var deviceList = busElement.DeviceList;
                        for (int n = 0; n < deviceList.Length; n++)
                        {
                            var deviceElement = deviceList[n];
                            Console.WriteLine($"  deviceList[{n}]:");
                            Console.WriteLine($"    0x{deviceElement.unk1:X2}-{deviceElement.unk2:X2}-{deviceElement.unk3:X2}-{deviceElement.unk4:X2}-{deviceElement.unk5:X2}-{deviceElement.unk6:X2}-{deviceElement.unk7:X2}");
                        }
                    }
                    break;
                }
            }
        }

        public static void ToConsole(this XusbLedState ledState)
        {
            Console.WriteLine($"Version:   0x{ledState.Version:X4}");
            Console.WriteLine($"LEDState:  0x{ledState.LEDState:X2} ({(XusbLedSetting)ledState.LEDState})");
        }

        public static void ToConsole(this XusbInputState inputState)
        {
            if (inputState.Version == (ushort)XusbDeviceVersion.v1_0)
            {
                var state = inputState.State_v0;
                Console.WriteLine($"Version:     0x{inputState.Version:X4}");
                Console.WriteLine($"Status:      0x{state.Status:X2}");
                Console.WriteLine($"unk1:        0x{state.unk1:X2}");
                Console.WriteLine($"unk2:        0x{state.unk2:X2}");
                Console.WriteLine($"Packet:      {state.PacketNumber}");
                Console.WriteLine($"unk3:        0x{state.unk3:X2}");

                state.Gamepad.ToConsole();
            }
            else
            {
                var state = inputState.State_v1;
                Console.WriteLine($"Version:     0x{state.Version:X4}");
                Console.WriteLine($"Status:      0x{state.Status:X2}");
                Console.WriteLine($"unk1:        0x{state.unk1:X2}");
                Console.WriteLine($"unk2:        0x{state.unk2:X2}");
                Console.WriteLine($"Packet:      {state.PacketNumber}");
                Console.WriteLine($"unk3:        0x{state.unk3:X2}");
                Console.WriteLine($"unk4:        0x{state.unk4:X2}");

                state.Gamepad.ToConsole();
            }
        }

        public static void ToConsole(this XusbGamepad gamepad)
        {
            Console.WriteLine($"Buttons:     0x{gamepad.Buttons:X4}");
            Console.WriteLine($"Triggers:    L: 0x{gamepad.LeftTrigger:X2}  R: 0x{gamepad.RightTrigger:X2}");
            Console.WriteLine($"LeftThumb:   X: 0x{gamepad.LeftThumbX:X4}  Y: 0x{gamepad.LeftThumbY:X4}");
            Console.WriteLine($"RightThumb:  X: 0x{gamepad.RightThumbX:X4}  Y: 0x{gamepad.RightThumbY:X4}");
            Console.WriteLine($"Ext:         Not supported by this device");
        }

        public static void ToConsole(this XusbGamepadEx gamepad)
        {
            Console.WriteLine($"Buttons:     0x{gamepad.Buttons:X4}");
            Console.WriteLine($"Triggers:    L: 0x{gamepad.LeftTrigger:X2}  R: 0x{gamepad.RightTrigger:X2}");
            Console.WriteLine($"LeftThumb:   X: 0x{gamepad.LeftThumbX:X4}  Y: 0x{gamepad.LeftThumbY:X4}");
            Console.WriteLine($"RightThumb:  X: 0x{gamepad.RightThumbX:X4}  Y: 0x{gamepad.RightThumbY:X4}");
            Console.WriteLine($"Ext:         0x{gamepad.Ext1:X2}-{gamepad.Ext2:X2}-{gamepad.Ext3:X2}-{gamepad.Ext4:X2}-{gamepad.Ext5:X2}-{gamepad.Ext6:X2}");
        }

        public static void ToConsole(this XusbVibration vibrationState)
        {
            Console.WriteLine($"LeftMotorSpeed:   0x{vibrationState.LeftMotorSpeed:X2}");
            Console.WriteLine($"RightMotorSpeed:  0x{vibrationState.RightMotorSpeed:X2}");
        }

        public static void ToConsole(this XusbCapabilities capabilities)
        {
            if (capabilities.Version == (ushort)XusbDeviceVersion.ProcNotSupported)
            {
                Console.WriteLine("This device does not support capability querying.");
            }
            else if (capabilities.Version == (ushort)XusbDeviceVersion.v1_1)
            {
                var caps = capabilities.Capabilities_v1;
                Console.WriteLine($"Version:     0x{caps.Version:X4}");
                Console.WriteLine($"Type:        0x{caps.Type:X2} ({(XusbControllerType)caps.Type})");
                Console.WriteLine($"SubType:     0x{caps.SubType:X2} ({(XusbControllerSubType)caps.SubType})");

                caps.Gamepad.ToConsole();

                caps.Vibration.ToConsole();
            }
            else
            {
                var caps = capabilities.Capabilities_v2;
                Console.WriteLine($"Version:     0x{caps.Version:X4}");
                Console.WriteLine($"Type:        0x{caps.Type:X2} ({(XusbControllerType)caps.Type})");
                Console.WriteLine($"SubType:     0x{caps.SubType:X2} ({(XusbControllerSubType)caps.SubType})");
                Console.WriteLine($"Flags:       0x{caps.Flags:X4} ({(XusbCapabilityFlags)caps.Flags})");
                Console.WriteLine($"VendorId:    0x{caps.VendorId:X4}");
                Console.WriteLine($"ProductId:   0x{caps.ProductId:X4}");
                Console.WriteLine($"Revision:    0x{caps.Revision:X4}");
                Console.WriteLine($"XusbId:      0x{caps.XusbId:X8}");

                caps.Gamepad.ToConsole();

                caps.Vibration.ToConsole();
            }
        }

        public static void ToConsole(this XusbBatteryInformation batteryInfo)
        {
            Console.WriteLine($"Version:  0x{batteryInfo.Version:X4}");
            Console.WriteLine($"Type:     0x{batteryInfo.Type:X2} ({(XusbBatteryType)batteryInfo.Type})");
            Console.WriteLine($"Level:    0x{batteryInfo.Level:X2} ({(XusbBatteryLevel)batteryInfo.Level})");
        }

        public static void ToConsole(this XusbAudioDeviceInformation audioInfo)
        {
            Console.WriteLine($"Version:    0x{audioInfo.Version:X4}");
            Console.WriteLine($"VendorId:   0x{audioInfo.VendorId:X4}");
            Console.WriteLine($"ProductId:  0x{audioInfo.ProductId:X4}");
            Console.WriteLine($"unk:        0x{audioInfo.unk:X4}");
        }

        public static void ToConsole(this XusbInputWaitState waitState)
        {
            Console.WriteLine($"Version:  0x{waitState.Version:X4}");
            Console.WriteLine($"Status:   0x{waitState.Status:X2}");

            Console.WriteLine($"unk1:     0x{waitState.unk1:X2}");
            Console.WriteLine($"unk2:     0x{waitState.unk2:X2}");
            Console.WriteLine($"unk3:     0x{waitState.unk3:X2}");
            Console.WriteLine($"unk4:     0x{waitState.unk4:X2}");
        }
    }
}