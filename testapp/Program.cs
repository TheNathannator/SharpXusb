using System;

namespace SharpXusbTestApp
{
    static class Program
    {
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            while (true)
            {
                Console.Clear();
                Utilities.CycleMenu("XUSB Tests");

                int choice = Utilities.PromptChoice("Select a device type: ", "Bus", "Input Device", "Exit");
                switch (choice)
                {
                    case 1: Tests.BusMain(); break;
                    case 2: Tests.DeviceMain(); break;
                    case 3: return;
                }
            }
        }

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = args.ExceptionObject as Exception;
            Console.WriteLine("An unhandled exception has occured:");
            Console.WriteLine(ex.ToString());
            Utilities.WaitForKey("Press any key to exit...");
        }
    }
}
