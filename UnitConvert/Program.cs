using System;
using System.Collections.Generic;
using System.Linq;

public static class Program
{
    static Dictionary<string, int> conversionFactors = new Dictionary<string, int>
    {
        { "deca", 1 },
        { "hecto", 2},
        { "kilo", 3},
        { "mega", 6},
        { "giga", 9},
        { "tera", 12},
        { "peta", 15},
        { "exa", 18},
        { "zetta", 21},
        { "yotta", 24},
        { "romma", 27},
        { "quecca", 30 },
        { "deci", -1 },
        { "centi", -2 },
        { "milli", -3 },
        { "micro", -6 },
        { "nano", -9 },
        { "pico", -12 },
        { "femto", -15 },
        { "atto", -18 },
        { "zepto", -21 },
        { "yocto", -24 },
        { "ronto", -27 },
        { "quecto", -30 }

    };
    static void Main(string[] args)
    {
        string filePath = "output.txt";
        bool keepRunning = true;

        while (keepRunning) 
        {
            Console.WriteLine("=== Unit Converter ===");
            Console.WriteLine("Enter the value to convert:");
            
            if (!double.TryParse(Console.ReadLine(), out double value))
            {
                Console.WriteLine("Invalid number. Press any key to try again...");
                Console.ReadKey();
                Console.Clear();
                continue;
            }

            Console.WriteLine("Enter the unit to convert from (e.g., Kilo, Mega, etc.):");
            string fromUnit = Console.ReadLine();

            Console.WriteLine("Enter the unit to convert to:");
            string toUnit = Console.ReadLine();

            try
            {
                double convertedValue = UnitConvert(fromUnit, toUnit, value);
                string resultLine = $"{value} {fromUnit} is equal to {convertedValue} {toUnit}";
                
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

    static double UnitConvert(string fromUnit, string toUnit, double value)
    {
        if (!conversionFactors.ContainsKey(fromUnit.ToLower()) || !conversionFactors.ContainsKey(toUnit.ToLower()))
        {
            throw new ArgumentException("Invalid unit provided. Please use one of the following units: " + string.Join(", ", conversionFactors.Keys));
        }

        int fromFactor = conversionFactors[fromUnit];
        int toFactor = conversionFactors[toUnit];

        double convertedValue = value * Math.Pow(10, fromFactor - toFactor);
        return convertedValue;
    }
}
