using System;
using SharpXusb;

namespace SharpXusbTestApp
{
    static class Utilities
    {
        public static void CycleMenu(string headerText, bool padHeader = true)
        {
            if (padHeader)
            {
                // Padding between previous section and new section
                Console.WriteLine();
            }

            // Write header
            string dashes = new('-', headerText.Length);
            Console.WriteLine(dashes);
            Console.WriteLine(headerText);
            Console.WriteLine(dashes);

            // Padding between header and contents
            Console.WriteLine();
        }

        public static bool PromptYesNo(string message)
        {
            while (true)
            {
                Console.Write($"{message} (y/n) ");

                string entry = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(entry))
                {
                    Console.WriteLine("Invalid entry, please try again.");
                }
                else
                {
                    var lower = entry.ToLowerInvariant();
                    if (lower == "y" || lower == "yes")
                    {
                        return true;
                    }
                    else if (lower == "n" || lower == "no")
                    {
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid entry, please try again.");
                    }
                }
            }
        }

        public static int PromptChoice(params string[] args)
        {
            return PromptChoice(int.MinValue, int.MaxValue, args);
        }

        public static int PromptChoice(int min, int max, params string[] args)
        {
            if (args == null || args.Length < 1)
            {
                throw new ArgumentException($"No strings supplied to PromptChoice.", nameof(args));
            }

            while (true)
            {
                // Write message
                if (args.Length == 1)
                {
                    // Choices unspecified
                    Console.Write(args[0]);
                }
                else
                {
                    // Choices specified

                    // Title
                    Console.WriteLine(args[0]);

                    // Options
                    for (int i = 1; i < args.Length; i++)
                    {
                        Console.WriteLine($"{i}. " + args[i]);
                    }

                    // Choice prompt
                    Console.Write("Selection: ");
                }

                string entry = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(entry) || !int.TryParse(entry, out int selection)
                    || selection < (args.Length == 1 ? 0 : 1)
                    || (args.Length == 1 ? (selection < min || selection > max) : selection >= args.Length))
                {
                    Console.WriteLine("Invalid entry, please try again.");
                }
                else
                {
                    return selection;
                }
            }
        }

        public static ConsoleKey WaitForKey(string message = "Press any key to continue...")
        {
            Console.WriteLine(message);
            return Console.ReadKey(intercept: true).Key;
        }
    }
}