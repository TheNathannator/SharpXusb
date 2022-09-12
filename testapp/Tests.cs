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
                byte index = (byte)(Utilities.PromptChoice($"Select the {deviceType} to use: ") - 1);
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
                    Console.WriteLine($"  {busIndex + 1}.");
                    Console.WriteLine($"  - Version: {bus.Version} (0x{(short)bus.Version:X4})");
                    Console.WriteLine($"  - Path: {bus.DevicePath}");
                }

                var selectedBus = SelectDevice(busList, "bus");

                while (true)
                {
                    int selectedTest = PromptTests(busTests);
                    if (selectedTest == 1)
                    {
                        // User cancelled
                        break;
                    }

                    // Run selected test
                    busTests[selectedTest - 2].func(selectedBus);
                }
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
                    Console.WriteLine($"  {deviceIndex + 1}.");
                    Console.WriteLine($"  - Version: {device.Version} (0x{(short)device.Version:X4})");
                    Console.WriteLine($"  - Path: {device.AssociatedBus.DevicePath}");
                }

                var selectedDevice = SelectDevice(deviceList, "device");

                while (true)
                {
                    int selectedTest = PromptTests(deviceTests);
                    if (selectedTest == 1)
                    {
                        // User cancelled
                        break;
                    }

                    // Run selected test
                    deviceTests[selectedTest - 2].func(selectedDevice);
                }
            }
        }

        public static void Bus_GetInformation(XusbBus bus)
        {
            Utilities.CycleMenu("XUSB Bus - Get Information");

            bus.GetInformation().ToConsole();

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
                    case 1: bus.GetInformationEx(XusbBusInformationExType.Minimal).ToConsole(XusbBusInformationExType.Minimal); break;
                    case 2: bus.GetInformationEx(XusbBusInformationExType.Basic).ToConsole(XusbBusInformationExType.Basic); break;
                    case 3: bus.GetInformationEx(XusbBusInformationExType.Full).ToConsole(XusbBusInformationExType.Full); break;
                    case 4: return;
                }

                Console.WriteLine();
                Utilities.WaitForKey();
            }
        }

        public static void Device_GetAssociatedBusInfo(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get Bus Information");

            device.AssociatedBus.GetInformation().ToConsole();

            Utilities.WaitForKey();
        }

        public static void Device_GetLedState(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get LED State");

            device.GetLedState().ToConsole();

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
                device.GetInputState().ToConsole();

                Console.WriteLine();
                Thread.Sleep(10);
            }
        }

        public static void Device_SetState(XusbDevice device)
        {
            while (true)
            {
                Utilities.CycleMenu("XUSB Device - Set State");

                var vibration = new XusbVibration();
                var ledState = XusbLedSetting.Off;
                XusbSetStateFlags flags = 0;

                if (Utilities.PromptYesNo("Set the speed of the vibration motors?"))
                {
                    flags |= XusbSetStateFlags.Vibration;
                    vibration.LeftMotorSpeed = (byte)Utilities.PromptChoice(0, 255, "Enter the speed for the left motor (0-255): ");
                    vibration.RightMotorSpeed = (byte)Utilities.PromptChoice(0, 255, "Enter the speed for the right motor (0-255): ");
                    vibration.ToConsole();
                }

                if (Utilities.PromptYesNo("Set the state of the LED?"))
                {
                    flags |= XusbSetStateFlags.Led;
                    ledState = (XusbLedSetting)Utilities.PromptChoice("Enter the LED state to set: ",
                        "Off",
                        "All Blink (Brief)",
                        "Player 1 Blink",
                        "Player 2 Blink",
                        "Player 3 Blink",
                        "Player 4 Blink",
                        "Player 1",
                        "Player 2",
                        "Player 3",
                        "Player 4",
                        "Clockwise Cycle",
                        "Fast Blink",
                        "Slow Blink",
                        "Flip-Flop",
                        "All Blink",
                        "Flash to Off"
                    ) - 1;
                    Console.WriteLine(ledState);
                }

                device.SetState(ledState, vibration, flags);

                Console.WriteLine();
                var key = Utilities.WaitForKey("Press Enter to go back to the previous menu, or press any other key to repeat this test.");
                if (key == ConsoleKey.Enter)
                {
                    return;
                }
            }
        }

        public static void Device_GetCapabilities(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get Capabilities");

            device.GetCapabilities().ToConsole();

            Console.WriteLine();
            Utilities.WaitForKey();
        }

        public static void Device_GetBatteryInformation(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get Battery Information");

            device.GetBatteryInformation().ToConsole();

            Console.WriteLine();
            Utilities.WaitForKey();
        }

        public static void Device_GetAudioDeviceInformation(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Get Audio Device Information");

            device.GetAudioDeviceInformation().ToConsole();

            Console.WriteLine();
            Utilities.WaitForKey();
        }

        public unsafe static void Device_WaitForGuide(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Wait For Guide Button");

            Device_WaitCommon(() => device.WaitForGuideButton());
        }

        public unsafe static void Device_WaitForInput(XusbDevice device)
        {
            Utilities.CycleMenu("XUSB Device - Wait for Input");

            Device_WaitCommon(() => device.WaitForInput());
        }

        private unsafe static void Device_WaitCommon(Func<XusbInputState> func)
        {
            Console.WriteLine("Press any key to stop the test and return to the selection.");

            var timer = new Stopwatch();
            int cursorPosition = Console.CursorTop;
            string timeString = "";
            while (!Console.KeyAvailable)
            {
                Console.SetCursorPosition(0, cursorPosition);

                timer.Start();
                var waitState = func();
                timer.Stop();

                // Clear line
                Console.WriteLine(new string(' ', timeString.Length));
                Console.SetCursorPosition(0, Console.CursorTop - 1);

                timeString = $"Waited for {timer.ElapsedMilliseconds} ms";
                Console.WriteLine(timeString);
                timer.Reset();

                Console.WriteLine("Input State:");
                waitState.ToConsole(2);
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