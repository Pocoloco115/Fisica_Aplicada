using System;
using System.IO;
using PhysicsUtils; 

public static class Program
{
    public static void Main(string[] args)
    {
        string filePath = "output.txt";
        bool keepRunning = true;

        while (keepRunning)
        {
            Console.WriteLine("=== Universal SI Prefix Converter ===");
            Console.WriteLine("Enter the numeric value to convert:");

            if (!double.TryParse(Console.ReadLine(), out double value))
            {
                Console.WriteLine("Invalid number. Press any key to try again...");
                Console.ReadKey();
                Console.Clear();
                continue;
            }

            Console.WriteLine("Enter the prefix to convert FROM (e.g., kilo, mega, micro). Empty = base (no prefix):");
            string fromPrefix = Console.ReadLine();

            Console.WriteLine("Enter the prefix to convert TO (e.g., milli, micro, kilo). Empty = base (no prefix):");
            string toPrefix = Console.ReadLine();

            Console.WriteLine("Optionally, enter the physical unit name (e.g., C, m, F) just for display (or leave empty):");
            string unitName = Console.ReadLine();

            try
            {
                double convertedValue = SiPrefixConverter.Convert(value, fromPrefix, toPrefix);

                string fromLabel = string.IsNullOrWhiteSpace(fromPrefix) ? "" : fromPrefix;
                string toLabel = string.IsNullOrWhiteSpace(toPrefix) ? "" : toPrefix;
                string unitLabel = string.IsNullOrWhiteSpace(unitName) ? "" : unitName;

                string resultLine = $"{value} {fromLabel}{unitLabel} is equal to {convertedValue} {toLabel}{unitLabel}";

                Console.WriteLine("\nRESULT: " + resultLine);

                File.AppendAllText(filePath, $"{DateTime.Now}: {resultLine}{Environment.NewLine}");

                Console.WriteLine("\nDo you want to perform another conversion? (y/n):");
                string response = Console.ReadLine()?.ToLower();

                if (response == "n" || response == "exit")
                {
                    keepRunning = false;
                    Console.WriteLine("Closing program... Goodbye!");
                }
                else
                {
                    Console.Clear();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("\nERROR: " + ex.Message);
                Console.WriteLine("Press any key to try again...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
