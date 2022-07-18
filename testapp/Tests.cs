using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using SharpXusb;

namespace SharpXusbTestApp
{
    static class Tests
    {
        public delegate void BusTestFn(XusbBus bus);
        public delegate void DeviceTestFn(XusbDevice device);

        public static readonly List<(BusTestFn func, string name)> busTests = new()
        {
            (Bus_GetInformation,   "Get Information"),
            (Bus_GetInformationEx, "Get Extended Information")
        };

        public static readonly List<(DeviceTestFn func, string name)> deviceTests = new()
        {
            (Device_GetAssociatedBusInfo,      "Get Associated Bus Info"),
            (Device_GetLedState,               "Get LED State"),
            (Device_GetInputState,             "Get Input State"),
            (Device_SetState,                  "Set LED/Vibration State"),
            (Device_GetCapabilities,           "Get Capabilities"),
            (Device_GetBatteryInformation,     "Get Battery Info"),
            (Device_GetAudioDeviceInformation, "Get Audio Device Info"),
            (Device_PowerOff,                  "Power Off"),
            (Device_WaitForGuide,              "Wait For Guide Button"),
            (Device_WaitForInput,              "Wait For Input")
        };

        private static T SelectDevice<T>(Dictionary<byte, T> list, string deviceType)
        {
            while (true)
            {
                byte index = (byte)Utilities.PromptChoice($"Select the {deviceType} to use: ");
                if (!list.ContainsKey(index))
                {
                    Console.WriteLine("Invalid entry, please try again.");
                }
                else
                {
                    return list[index];
                }
            }
        }

        private static int PromptTests<T>(List<(T func, string name)> funcs)
        {
            string[] testPrompt = new string[funcs.Count + 2];
            testPrompt[0] = "Select the test to carry out: ";
            testPrompt[1] = "Cancel";

            for (int index = 2; index < funcs.Count + 2; index++)
            {
                testPrompt[index] = funcs[index - 2].name;
            }

            return Utilities.PromptChoice(testPrompt);
        }

        public static void BusMain()
        {
            while (true)
            {
                Utilities.CycleMenu("XUSB Bus Tests");

                var busList = XusbList.BusList;
                if (busList.Count == 0)
                {
                    Console.WriteLine("No XUSB buses found.");
                    Utilities.WaitForKey("Press any key to return to the previous menu...");
                    return;
                }

                Console.WriteLine("Buses:");
                foreach (var busIndex in busList.Keys)
                {
                    var bus = busList[busIndex];
                    Console.WriteLine($"  Index {busIndex}:");
                    Console.WriteLine($"  - Version: {bus.Version} (0x{(short)bus.Version:X4})");
                    Console.WriteLine($"  - Path: {bus.DevicePath}");
                }

                var selectedBus = SelectDevice(busList, "bus");
                int selectedTest = PromptTests(busTests);
                if (selectedTest == 1)
                {
                    // User cancelled
                    return;
                }

                // Run selected test
                busTests[selectedTest - 2].func(selectedBus);
            }
        }

        public static void DeviceMain()
        {
            while (true)
            {
                Utilities.CycleMenu("XUSB Device Tests");

                var deviceList = XusbList.DeviceList;
                if (deviceList.Count == 0)
                {
                    Console.WriteLine("No XUSB devices found.");
                    Utilities.WaitForKey("Press any key to return to the previous menu...");
                    return;
                }

                Console.WriteLine("Devices:");
                foreach (var deviceIndex in deviceList.Keys)
                {
                    var device = deviceList[deviceIndex];
                    Console.WriteLine($"  {deviceIndex}.");
                    Console.WriteLine($"  - Version: {device.Version} (0x{(short)device.Version:X4})");
                    Console.WriteLine($"  - Path: {device.AssociatedBus.DevicePath}");
                }

                var selectedDevice = SelectDevice(deviceList, "bus");
                int selectedTest = PromptTests(deviceTests);
                if (selectedTest == 0)
                {
                    // User cancelled
                    return;
                }

                // Run selected test
                deviceTests[selectedTest - 2].func(selectedDevice);
            }
        }

        public static void Bus_GetInformation(XusbBus bus)
        {
            Utilities.CycleMenu("XUSB Bus - Get Information");

            var info = bus.GetInformation();
            Console.WriteLine($"Version:      0x{info.Version:X4}");
            Console.WriteLine($"MaxCount:     {info.MaxCount}");
            Console.WriteLine($"DeviceCount:  {info.DeviceCount}");
            Console.WriteLine($"Status:       0x{info.Status:X2}");
            Console.WriteLine($"unk1:         0x{info.unk1:X2}");
            Console.WriteLine($"unk2:         0x{info.unk2:X4}");
            Console.WriteLine($"VendorId:     0x{info.VendorId:X4}");
            Console.WriteLine($"ProductId:    0x{info.ProductId:X4}");

            Utilities.WaitForKey();
        }

        public static void Bus_GetInformationEx(XusbBus bus)
        {
            while (true)
            {
                Utilities.CycleMenu("XUSB Bus - Get Extended Information");

                int choice = Utilities.PromptChoice("Select the type of extended information to retrieve: ",
                    "Minimal",
                    "Basic",
                    "Full",
                    "Cancel"
                );

                switch (choice)
                {
                    case 1: Bus_GetInformationEx_Minimal(bus); break;
                    case 2: Bus_GetInformationEx_Basic(bus); break;
                    case 3: Bus_GetInformationEx_Full(bus); break;
                    case 4: return;
                }

                Console.WriteLine();
                Utilities.WaitForKey();
            }
        }

        private static void Bus_GetInformationEx_Minimal(XusbBus bus)
        {
            var info = bus.GetInformationEx(XusbBusInformationExType.Minimal);
            if (info.DataLength == 0)
            {
                Console.WriteLine($"Could not retrieve extended bus information.");
                return;
            }

            Console.WriteLine($"Version:     0x{info.Version:X4}");
            Console.WriteLine($"Failure:     {info.Failure}");
            Console.WriteLine($"DataLength:  {info.DataLength}");

            var minimal = info.Minimal;
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
        }

        private static void Bus_GetInformationEx_Basic(XusbBus bus)
        {
            var info = bus.GetInformationEx(XusbBusInformationExType.Basic);
            if (info.DataLength == 0)
            {
                Console.WriteLine($"Could not retrieve extended bus information.");
                return;
            }

            Console.WriteLine($"Version:     0x{info.Version:X4}");
            Console.WriteLine($"Failure:     {info.Failure}");
            Console.WriteLine($"DataLength:  {info.DataLength}");

            var basic = info.Basic;
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
        }

        private static void Bus_GetInformationEx_Full(XusbBus bus)
        {
            var info = bus.GetInformationEx(XusbBusInformationExType.Full);
            if (info.DataLength == 0)
            {
                Console.WriteLine($"Could not retrieve extended bus information.");
                return;
            }

            Console.WriteLine($"Version:     0x{info.Version:X4}");
            Console.WriteLine($"Failure:     {info.Failure}");
            Console.WriteLine($"DataLength:  {info.DataLength}");

            var full = info.Full;
            if (full is null)
            {
                Console.WriteLine("No further data supplied by the driver.");
                return;
            }

            for (int i = 0; i < full.Length; i++)
            {
                Console.WriteLine($"Full[{i}]:");

                var f1 = full[i];
                Console.WriteLine($"  unk1:  0x{f1.unk1:X2}");
                Console.WriteLine($"  unk2:  0x{f1.unk2:X2}");
                Console.WriteLine($"  unk3:  0x{f1.unk3:X2}");

                var unkList = f1.DeviceList;
                for (int j = 0; j < unkList.Length; j++)
                {
                    var f2 = unkList[j];
                    Console.WriteLine($"  unkList[{j}]:");
                    Console.WriteLine($"    0x{f2.unk1:X2}-{f2.unk2:X2}-{f2.unk3:X2}-{f2.unk4:X2}-{f2.unk5:X2}-{f2.unk6:X2}-{f2.unk7:X2}");
                }
            }
        }

        public static void Device_GetAssociatedBusInfo(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get Bus Information");

            var bus = device.AssociatedBus;
            var info = bus.GetInformation();
            Console.WriteLine($"Path:         {bus.DevicePath}");
            Console.WriteLine($"Version:      0x{info.Version:X4}");
            Console.WriteLine($"MaxCount:     {info.MaxCount}");
            Console.WriteLine($"DeviceCount:  {info.DeviceCount}");
            Console.WriteLine($"Status:       0x{info.Status:X2}");
            Console.WriteLine($"unk1:         0x{info.unk1:X2}");
            Console.WriteLine($"unk2:         0x{info.unk2:X4}");
            Console.WriteLine($"VendorId:     0x{info.VendorId:X4}");
            Console.WriteLine($"ProductId:    0x{info.ProductId:X4}");

            Utilities.WaitForKey();
        }

        public static void Device_GetLedState(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get LED State");

            var state = device.GetLedState();
            Console.WriteLine($"State Version:  0x{state.Version:X4}");
            Console.WriteLine($"LEDState:  0x{state.LEDState:X2} ({(XusbLedSetting)state.LEDState})");

            Utilities.WaitForKey();
        }

        public static void Device_GetInputState(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get Input State");

            Console.WriteLine("Press any key to stop the test and return to the selection.");
            Console.WriteLine();

            int cursorPosition = Console.CursorTop;
            while (!Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, cursorPosition);
                var inputState = device.GetInputState();
                if (inputState.Version == (ushort)XusbDeviceVersion.v1_0)
                {
                    var state = inputState.State_v0;
                    Console.WriteLine($"State version:  0x{inputState.Version:X4}");
                    Console.WriteLine($"Status:         0x{state.Status:X2}");
                    Console.WriteLine($"unk1:           0x{state.unk1:X2}");
                    Console.WriteLine($"unk2:           0x{state.unk2:X2}");
                    Console.WriteLine($"Packet:         {state.PacketNumber}");
                    Console.WriteLine($"unk3:           0x{state.unk3:X2}");

                    var gamepad = state.Gamepad;
                    Console.WriteLine($"Buttons:        0x{gamepad.Buttons:X4}");
                    Console.WriteLine($"Triggers:       L: 0x{gamepad.LeftTrigger:X2}  R: 0x{gamepad.RightTrigger:X2}");
                    Console.WriteLine($"LeftThumb:      X: 0x{gamepad.LeftThumbX:X4}  Y: 0x{gamepad.LeftThumbY:X4}");
                    Console.WriteLine($"RightThumb:     X: 0x{gamepad.RightThumbX:X4}  Y: 0x{gamepad.RightThumbY:X4}");
                    Console.WriteLine($"Ext:            Not supported by this device");
                }
                else
                {
                    var state = inputState.State_v1;
                    Console.WriteLine($"State version:  0x{state.Version:X4}");
                    Console.WriteLine($"Status:         0x{state.Status:X2}");
                    Console.WriteLine($"unk1:           0x{state.unk1:X2}");
                    Console.WriteLine($"unk2:           0x{state.unk2:X2}");
                    Console.WriteLine($"Packet:         {state.PacketNumber}");
                    Console.WriteLine($"unk3:           0x{state.unk3:X2}");
                    Console.WriteLine($"unk4:           0x{state.unk4:X2}");

                    var gamepad = state.Gamepad;
                    Console.WriteLine($"Buttons:        0x{gamepad.Buttons:X4}");
                    Console.WriteLine($"Triggers:       L: 0x{gamepad.LeftTrigger:X2}  R: 0x{gamepad.RightTrigger:X2}");
                    Console.WriteLine($"LeftThumb:      X: 0x{gamepad.LeftThumbX:X4}  Y: 0x{gamepad.LeftThumbY:X4}");
                    Console.WriteLine($"RightThumb:     X: 0x{gamepad.RightThumbX:X4}  Y: 0x{gamepad.RightThumbY:X4}");
                    Console.WriteLine($"Ext:            0x{gamepad.Ext1:X2}-{gamepad.Ext2:X2}-{gamepad.Ext3:X2}-{gamepad.Ext4:X2}-{gamepad.Ext5:X2}-{gamepad.Ext6:X2}");
                }

                Console.WriteLine();
                Thread.Sleep(10);
            }
        }

        public static void Device_SetState(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Set State");

            var vibration = new XusbVibration();
            var ledState = XusbLedSetting.Off;

            bool setVibration = false;
            bool setLed = false;

            if (Utilities.PromptYesNo("Set the speed of the vibration motors?"))
            {
                setVibration = true;
                vibration.LeftMotorSpeed = (byte)Utilities.PromptChoice("Enter the speed for the left motor (0-255): ");
                vibration.RightMotorSpeed = (byte)Utilities.PromptChoice("Enter the speed for the right motor (0-255): ");
            }

            if (Utilities.PromptYesNo("Set the state of the LED?"))
            {
                setLed = true;
                ledState = (XusbLedSetting)Utilities.PromptChoice("Enter the LED state to set: ",
                    "Off",
                    "Blink",
                    "Player 1 Switch-Blink",
                    "Player 2 Switch-Blink",
                    "Player 3 Switch-Blink",
                    "Player 4 Switch-Blink",
                    "Player 1",
                    "Player 2",
                    "Player 3",
                    "Player 4",
                    "Cycle",
                    "Fast Blink",
                    "Slow Blink",
                    "Flip-flop",
                    "All Blink"
                ) - 1;
            }

            if (setVibration && setLed)
            {
                device.SetState(ledState, vibration);
            }
            else
            {
                if (setVibration)
                {
                    device.SetState(vibration);
                }

                if (setLed)
                {
                    device.SetState(ledState);
                }
            }

            Console.WriteLine();
            Utilities.WaitForKey();
        }

        public static void Device_GetCapabilities(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get Capabilities");

            var capsBuf = device.GetCapabilities();
            if (capsBuf.Version == (ushort)XusbDeviceVersion.ProcNotSupported)
            {
                Console.WriteLine("This device does not support capability querying.");
            }
            else if (capsBuf.Version == (ushort)XusbDeviceVersion.v1_1)
            {
                var caps = capsBuf.Capabilities_v1;
                Console.WriteLine($"Version:     0x{caps.Version:X4}");
                Console.WriteLine($"Type:        0x{caps.Type:X2} ({(XusbControllerType)caps.Type})");
                Console.WriteLine($"SubType:     0x{caps.SubType:X2} ({(XusbControllerSubType)caps.SubType})");

                var gamepad = caps.Gamepad;
                Console.WriteLine($"Buttons:     0x{gamepad.Buttons:X4}");
                Console.WriteLine($"Triggers:    L: 0x{gamepad.LeftTrigger:X2}  R: 0x{gamepad.RightTrigger:X2}");
                Console.WriteLine($"LeftThumb:   X: 0x{gamepad.LeftThumbX:X4}  Y: 0x{gamepad.LeftThumbY:X4}");
                Console.WriteLine($"RightThumb:  X: 0x{gamepad.RightThumbX:X4}  Y: 0x{gamepad.RightThumbY:X4}");
                Console.WriteLine($"Ext:         0x{gamepad.Ext1:X2}-{gamepad.Ext2:X2}-{gamepad.Ext3:X2}-{gamepad.Ext4:X2}-{gamepad.Ext5:X2}-{gamepad.Ext6:X2}");

                var vibration = caps.Vibration;
                Console.WriteLine($"LeftMotorSpeed:   0x{vibration.LeftMotorSpeed:X2}");
                Console.WriteLine($"RightMotorSpeed:  0x{vibration.RightMotorSpeed:X2}");
            }
            else
            {
                var caps = capsBuf.Capabilities_v2;
                Console.WriteLine($"Version:     0x{caps.Version:X4}");
                Console.WriteLine($"Type:        0x{caps.Type:X2} ({(XusbControllerType)caps.Type})");
                Console.WriteLine($"SubType:     0x{caps.SubType:X2} ({(XusbControllerSubType)caps.SubType})");
                Console.WriteLine($"Flags:       0x{caps.Flags:X4} ({(XusbCapabilityFlags)caps.Flags})");
                Console.WriteLine($"VendorId:    0x{caps.VendorId:X4}");
                Console.WriteLine($"ProductId:   0x{caps.ProductId:X4}");
                Console.WriteLine($"Revision:    0x{caps.Revision:X4}");
                Console.WriteLine($"XusbId:      0x{caps.XusbId:X8}");

                var gamepad = caps.Gamepad;
                Console.WriteLine($"Buttons:     0x{gamepad.Buttons:X4}");
                Console.WriteLine($"Triggers:    L: 0x{gamepad.LeftTrigger:X2}  R: 0x{gamepad.RightTrigger:X2}");
                Console.WriteLine($"LeftThumb:   X: 0x{gamepad.LeftThumbX:X4}  Y: 0x{gamepad.LeftThumbY:X4}");
                Console.WriteLine($"RightThumb:  X: 0x{gamepad.RightThumbX:X4}  Y: 0x{gamepad.RightThumbY:X4}");
                Console.WriteLine($"Ext:         0x{gamepad.Ext1:X2}-{gamepad.Ext2:X2}-{gamepad.Ext3:X2}-{gamepad.Ext4:X2}-{gamepad.Ext5:X2}-{gamepad.Ext6:X2}");

                var vibration = caps.Vibration;
                Console.WriteLine($"LeftMotorSpeed:   0x{vibration.LeftMotorSpeed:X2}");
                Console.WriteLine($"RightMotorSpeed:  0x{vibration.RightMotorSpeed:X2}");
            }

            Console.WriteLine();
            Utilities.WaitForKey();
        }

        public static void Device_GetBatteryInformation(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get Battery Information");

            var battInfo = device.GetBatteryInformation();
            Console.WriteLine($"Version:  0x{battInfo.Version:X4}");
            Console.WriteLine($"Type:     0x{battInfo.Type:X2} ({(XusbBatteryType)battInfo.Type})");
            Console.WriteLine($"Level:    0x{battInfo.Level:X2} ({(XusbBatteryLevel)battInfo.Level})");

            Console.WriteLine();
            Utilities.WaitForKey();
        }

        public static void Device_GetAudioDeviceInformation(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get Audio Device Information");

            var audioInfo = device.GetAudioDeviceInformation();
            Console.WriteLine($"Version:    0x{audioInfo.Version:X4}");
            Console.WriteLine($"VendorId:   0x{audioInfo.VendorId:X4}");
            Console.WriteLine($"ProductId:  0x{audioInfo.ProductId:X4}");
            Console.WriteLine($"unk:        0x{audioInfo.unk:X4}");

            Console.WriteLine();
            Utilities.WaitForKey();
        }

        public unsafe static void Device_WaitForGuide(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Wait For Guide Button");

            Console.WriteLine("Press any key to stop the test and return to the selection.");

            var timer = new Stopwatch();
            while (!Console.KeyAvailable)
            {
                timer.Start();
                var waitState = device.WaitForGuideButton();
                timer.Stop();
                Console.WriteLine($"Waited for {timer.ElapsedMilliseconds} ms");
                timer.Reset();

                Console.WriteLine("Wait State:");
                Console.WriteLine($"  Version:  0x{waitState.Version:X4}");
                Console.WriteLine($"  Status:   0x{waitState.Status:X2}");

                Console.WriteLine($"  unk1:     0x{waitState.unk1:X2}");
                Console.WriteLine($"  unk2:     0x{waitState.unk2:X2}");
                Console.WriteLine($"  unk3:     0x{waitState.unk3:X2}");
                Console.WriteLine($"  unk4:     0x{waitState.unk4:X2}");
            }
        }

        public unsafe static void Device_WaitForInput(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Wait for Input");

            Console.WriteLine("Press any key to stop the test and return to the selection.");

            var timer = new Stopwatch();
            while (!Console.KeyAvailable)
            {
                timer.Start();
                var waitState = device.WaitForInput();
                timer.Stop();
                Console.WriteLine($"Waited for {timer.ElapsedMilliseconds} ms");
                timer.Reset();

                Console.WriteLine("Wait State:");
                Console.WriteLine($"  Version:  0x{waitState.Version:X4}");
                Console.WriteLine($"  Status:   0x{waitState.Status:X2}");

                Console.WriteLine($"  unk1:     0x{waitState.unk1:X2}");
                Console.WriteLine($"  unk2:     0x{waitState.unk2:X2}");
                Console.WriteLine($"  unk3:     0x{waitState.unk3:X2}");
                Console.WriteLine($"  unk4:     0x{waitState.unk4:X2}");
            }
        }

        public static void Device_PowerOff(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Power Off");

            var key = Utilities.WaitForKey("Press the Enter key to continue, or any other key to cancel.");
            if (key == ConsoleKey.Enter)
            {
                device.PowerOff();
                Console.WriteLine("Device has been powered off.");
            }
            else
            {
                return;
            }

            Utilities.WaitForKey();
        }
    }
}