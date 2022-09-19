using System;
using System.Collections.Generic;
using SharpXusb;

namespace SharpXusbTestApp
{
    static class StructToConsole
    {
        private static readonly Dictionary<int, string> indentations = new();
        private static string GetIndentation(int indentAmount)
        {
            if (!indentations.TryGetValue(indentAmount, out string indent))
            {
                indent = new string(' ', indentAmount);
                indentations.Add(indentAmount, indent);
            }
            return indent;
        }

        public static void ToConsole(this XusbBusInfo busInfo, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Version:      0x{busInfo.Version:X4}");
            Console.WriteLine($"{indent}MaxCount:     {busInfo.MaxCount}");
            Console.WriteLine($"{indent}DeviceCount:  {busInfo.DeviceCount}");
            Console.WriteLine($"{indent}Status:       0x{busInfo.Status:X2}");
            Console.WriteLine($"{indent}unk1:         0x{busInfo.unk1:X2}");
            Console.WriteLine($"{indent}unk2:         0x{busInfo.unk2:X4}");
            Console.WriteLine($"{indent}VendorId:     0x{busInfo.VendorId:X4}");
            Console.WriteLine($"{indent}ProductId:    0x{busInfo.ProductId:X4}");
        }

        public static void ToConsole(this XusbBusInfoEx busInfo, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Version:     0x{busInfo.Version:X4}");
            Console.WriteLine($"{indent}Failure:     {busInfo.Failure}");
            Console.WriteLine($"{indent}DataLength:  {busInfo.DataLength}");

            if (busInfo.IsEmpty)
            {
                Console.WriteLine($"{indent}Could not retrieve further extended bus information.");
                return;
            }
            else if (busInfo.IsMinimal)
            {
                Console.WriteLine($"{indent}Minimal:");
                busInfo.Minimal.ToConsole(indentAmount + 2);
            }
            else if (busInfo.IsBasic)
            {
                var basic = busInfo.Basic;
                for (int i = 0; i < basic.Length; i++)
                {
                    Console.WriteLine($"{indent}Basic[{i}]:");
                    basic[i].ToConsole(indentAmount + 2);
                }
            }
            else if (busInfo.IsFull)
            {
                var full = busInfo.Full;
                for (int i = 0; i < full.Length; i++)
                {
                    Console.WriteLine($"{indent}Full[{i}]:");
                    full[i].ToConsole(indentAmount + 2);
                }
            }
            else
            {
                Console.WriteLine($"{indent}Unknown bus info type! Writing raw data.");
                busInfo.RawData.ToConsole();
            }
        }

        public static void ToConsole(this XusbBusInfoEx_Minimal minimal, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}unk1:       0x{minimal.unk1:X8}");
            Console.WriteLine($"{indent}unk2:       0x{minimal.unk2:X8}");
            Console.WriteLine($"{indent}unk3:       0x{minimal.unk3:X8}");
            Console.WriteLine($"{indent}unk4:       0x{minimal.unk4:X8}");
            Console.WriteLine($"{indent}unk5:       0x{minimal.unk5:X4}");
            Console.WriteLine($"{indent}VendorId:   0x{minimal.VendorId:X4}");
            Console.WriteLine($"{indent}ProductId:  0x{minimal.ProductId:X4}");
            Console.WriteLine($"{indent}unk8:       0x{minimal.unk8:X4}");
            Console.WriteLine($"{indent}unk9:       0x{minimal.unk9:X2}");
            Console.WriteLine($"{indent}unk10:      0x{minimal.unk10:X8}");
            Console.WriteLine($"{indent}unk11:      0x{minimal.unk11:X4}");
            Console.WriteLine($"{indent}unk12:      0x{minimal.unk12:X2}");
        }

        public static void ToConsole(this XusbBusInfoEx_Basic basic, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}unk1:       0x{basic.unk1:X2}");
            Console.WriteLine($"{indent}unk2:       0x{basic.unk2:X8}");
            Console.WriteLine($"{indent}unk3:       0x{basic.unk3:X8}");
            Console.WriteLine($"{indent}VendorId:   0x{basic.VendorId:X4}");
            Console.WriteLine($"{indent}ProductId:  0x{basic.ProductId:X4}");
            Console.WriteLine($"{indent}unk5:       0x{basic.unk5:X8}");
            Console.WriteLine($"{indent}unk6:       0x{basic.unk6:X4}");
            Console.WriteLine($"{indent}unk7:       0x{basic.unk7:X8}");
            Console.WriteLine($"{indent}unk8:       0x{basic.unk8:X2}");
            Console.WriteLine($"{indent}unk9:       0x{basic.unk9:X4}");
            Console.WriteLine($"{indent}unk10:      0x{basic.unk10:X2}");
        }

        public static void ToConsole(this XusbBusInfoEx_Full full, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}unk1:  0x{full.unk1:X2}");
            Console.WriteLine($"{indent}unk2:  0x{full.unk2:X2}");
            Console.WriteLine($"{indent}unk3:  0x{full.unk3:X2}");

            var deviceList = full.DeviceList;
            for (int n = 0; n < deviceList.Length; n++)
            {
                var deviceElement = deviceList[n];
                Console.WriteLine($"{indent}deviceList[{n}]:");
                deviceElement.ToConsole(indentAmount + 2);
            }
        }

        public static void ToConsole(this XusbBusInfoEx_Full_Sub sub, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}unk1:  0x{sub.unk1:X2}");
            Console.WriteLine($"{indent}unk2:  0x{sub.unk2:X2}");
            Console.WriteLine($"{indent}unk3:  0x{sub.unk3:X2}");
            Console.WriteLine($"{indent}unk4:  0x{sub.unk4:X2}");
            Console.WriteLine($"{indent}unk5:  0x{sub.unk5:X2}");
            Console.WriteLine($"{indent}unk6:  0x{sub.unk6:X2}");
            Console.WriteLine($"{indent}unk7:  0x{sub.unk7:X2}");
        }

        public static void ToConsole(this XusbLedState ledState, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Version:   0x{ledState.Version:X4}");
            Console.WriteLine($"{indent}LEDState:  0x{ledState.LEDState:X2} ({(XusbLedSetting)ledState.LEDState})");
        }

        public static void ToConsole(this XusbInputState inputState, int indentAmount = 0)
        {
            if (inputState.Version == (ushort)XusbDeviceVersion.v1_0)
                inputState.State_v0.ToConsole(indentAmount);
            else
                inputState.State_v1.ToConsole(indentAmount);
        }

        public static void ToConsole(this XusbInputState_v0 inputState, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Version:  0x{(ushort)XusbDeviceVersion.v1_0:X4}");
            Console.WriteLine($"{indent}Status:   0x{inputState.Status:X2}");
            Console.WriteLine($"{indent}unk1:     0x{inputState.unk1:X2}");
            Console.WriteLine($"{indent}unk2:     0x{inputState.unk2:X2}");
            Console.WriteLine($"{indent}Packet:   {inputState.PacketNumber}");
            Console.WriteLine($"{indent}unk3:     0x{inputState.unk3:X2}");

            inputState.Gamepad.ToConsole(indentAmount);
        }

        public static void ToConsole(this XusbInputState_v1 inputState, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Version:  0x{inputState.Version:X4}");
            Console.WriteLine($"{indent}Status:   0x{inputState.Status:X2}");
            Console.WriteLine($"{indent}unk1:     0x{inputState.unk1:X2}");
            Console.WriteLine($"{indent}unk2:     0x{inputState.unk2:X2}");
            Console.WriteLine($"{indent}Packet:   {inputState.PacketNumber}");
            Console.WriteLine($"{indent}unk3:     0x{inputState.unk3:X2}");
            Console.WriteLine($"{indent}unk4:     0x{inputState.unk4:X2}");

            inputState.Gamepad.ToConsole(indentAmount);
        }

        public static void ToConsole(this XusbGamepad gamepad, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Buttons:     0x{gamepad.Buttons:X4}");
            Console.WriteLine($"{indent}Triggers:    L: 0x{gamepad.LeftTrigger:X2}  R: 0x{gamepad.RightTrigger:X2}");
            Console.WriteLine($"{indent}LeftThumb:   X: 0x{gamepad.LeftThumbX:X4}  Y: 0x{gamepad.LeftThumbY:X4}");
            Console.WriteLine($"{indent}RightThumb:  X: 0x{gamepad.RightThumbX:X4}  Y: 0x{gamepad.RightThumbY:X4}");
            Console.WriteLine($"{indent}Ext:         Not supported by this device");
        }

        public static void ToConsole(this XusbGamepadEx gamepad, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Buttons:     0x{gamepad.Buttons:X4}");
            Console.WriteLine($"{indent}Triggers:    L: 0x{gamepad.LeftTrigger:X2}  R: 0x{gamepad.RightTrigger:X2}");
            Console.WriteLine($"{indent}LeftThumb:   X: 0x{gamepad.LeftThumbX:X4}  Y: 0x{gamepad.LeftThumbY:X4}");
            Console.WriteLine($"{indent}RightThumb:  X: 0x{gamepad.RightThumbX:X4}  Y: 0x{gamepad.RightThumbY:X4}");
            Console.WriteLine($"{indent}Ext:         0x{gamepad.Ext1:X2}-{gamepad.Ext2:X2}-{gamepad.Ext3:X2}-{gamepad.Ext4:X2}-{gamepad.Ext5:X2}-{gamepad.Ext6:X2}");
        }

        public static void ToConsole(this XusbVibration vibrationState, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}LeftMotorSpeed:   0x{vibrationState.LeftMotorSpeed:X2}");
            Console.WriteLine($"{indent}RightMotorSpeed:  0x{vibrationState.RightMotorSpeed:X2}");
        }

        public static void ToConsole(this XusbCapabilities capabilities, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            if (capabilities.Version == (ushort)XusbDeviceVersion.ProcNotSupported)
                Console.WriteLine($"{indent}This device does not support capability querying.");
            else if (capabilities.Version == (ushort)XusbDeviceVersion.v1_1)
                capabilities.Capabilities_v1.ToConsole();
            else
                capabilities.Capabilities_v2.ToConsole();
        }

        public static void ToConsole(this XusbCapabilities_v1 capabilities, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Version:  0x{capabilities.Version:X4}");
            Console.WriteLine($"{indent}Type:     0x{capabilities.Type:X2} ({(XusbControllerType)capabilities.Type})");
            Console.WriteLine($"{indent}SubType:  0x{capabilities.SubType:X2} ({(XusbControllerSubType)capabilities.SubType})");

            capabilities.Gamepad.ToConsole(indentAmount);

            capabilities.Vibration.ToConsole(indentAmount);
        }

        public static void ToConsole(this XusbCapabilities_v2 capabilities, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Version:    0x{capabilities.Version:X4}");
            Console.WriteLine($"{indent}Type:       0x{capabilities.Type:X2} ({(XusbControllerType)capabilities.Type})");
            Console.WriteLine($"{indent}SubType:    0x{capabilities.SubType:X2} ({(XusbControllerSubType)capabilities.SubType})");
            Console.WriteLine($"{indent}Flags:      0x{capabilities.Flags:X4} ({(XusbCapabilityFlags)capabilities.Flags})");
            Console.WriteLine($"{indent}VendorId:   0x{capabilities.VendorId:X4}");
            Console.WriteLine($"{indent}ProductId:  0x{capabilities.ProductId:X4}");
            Console.WriteLine($"{indent}Revision:   0x{capabilities.Revision:X4}");
            Console.WriteLine($"{indent}XusbId:     0x{capabilities.XusbId:X8}");

            capabilities.Gamepad.ToConsole(indentAmount);

            capabilities.Vibration.ToConsole(indentAmount);
        }

        public static void ToConsole(this XusbBatteryInformation batteryInfo, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Version:  0x{batteryInfo.Version:X4}");
            Console.WriteLine($"{indent}Type:     0x{batteryInfo.Type:X2} ({(XusbBatteryType)batteryInfo.Type})");
            Console.WriteLine($"{indent}Level:    0x{batteryInfo.Level:X2} ({(XusbBatteryLevel)batteryInfo.Level})");
        }

        public static void ToConsole(this XusbAudioDeviceInformation audioInfo, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            Console.WriteLine($"{indent}Version:    0x{audioInfo.Version:X4}");
            Console.WriteLine($"{indent}VendorId:   0x{audioInfo.VendorId:X4}");
            Console.WriteLine($"{indent}ProductId:  0x{audioInfo.ProductId:X4}");
            Console.WriteLine($"{indent}unk:        0x{audioInfo.unk:X4}");
        }

        public static void ToConsole(this byte[] array, int indentAmount = 0)
        {
            string indent = GetIndentation(indentAmount);

            int index = 0;
            int increment = 0; // For doing 16 elements per line
            Console.Write(indent);
            while (index < array.Length - 1)
            {
                if (increment < 15)
                {
                    // Continue on same line
                    Console.Write($"{array[index]:X2}-");
                    increment++;
                }
                else
                {
                    // End line and create a new one
                    Console.WriteLine($"{array[index]:X2}");
                    Console.Write(indent);
                    increment = 0;
                }
                index++;
            }
            // Write last element in the array without the hyphen at the start
            Console.WriteLine($"{array[index]:X2}");
        }
    }
}