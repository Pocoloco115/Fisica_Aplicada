using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CoulombPhysics;
public static class Menu
{
    private static string tittle = @"
  /$$$$$$            /$$                     /$$ /$$   /$$              
 /$$__  $$          | $$                    | $$|__/  | $$              
| $$  \__/  /$$$$$$ | $$  /$$$$$$$ /$$   /$$| $$ /$$ /$$$$$$    /$$$$$$ 
| $$       |____  $$| $$ /$$_____/| $$  | $$| $$| $$|_  $$_/   /$$__  $$
| $$        /$$$$$$$| $$| $$      | $$  | $$| $$| $$  | $$    | $$  \ $$
| $$    $$ /$$__  $$| $$| $$      | $$  | $$| $$| $$  | $$ /$$| $$  | $$
|  $$$$$$/|  $$$$$$$| $$|  $$$$$$$|  $$$$$$/| $$| $$  |  $$$$/|  $$$$$$/
 \______/  \_______/|__/ \_______/ \______/ |__/|__/   \___/   \______/                                                                                                                                                                                                                                                                            
";

    public static void Start()
    {
        DisplayMenu();
    }

    private static void WriteCenteredBlock(string text)
    {
        int width = Console.WindowWidth;
        if (width <= 0)
        {
            Console.Write(text);
            return;
        }

        string[] rawLines = text.Replace("\r", "").Split('\n');

        int maxLen = 0;
        for (int i = 0; i < rawLines.Length; i++)
        {
            int len = rawLines[i].TrimEnd().Length;
            if (len > maxLen) maxLen = len;
        }

        foreach (string raw in rawLines)
        {
            string line = raw.TrimEnd();
            int pad = Math.Max(0, (width - maxLen) / 2);
            Console.WriteLine(new string(' ', pad) + line);
        }
    }

    private static void WriteCenteredLine(string text)
    {
        int width = Console.WindowWidth;
        if (width <= 0)
        {
            Console.WriteLine(text);
            return;
        }

        text = text.TrimEnd();
        int pad = Math.Max(0, (width - text.Length) / 2);
        Console.WriteLine(new string(' ', pad) + text);
    }

    private static string ReadCenteredLine(string prompt)
    {
        string line = prompt.TrimEnd();
        int width = Console.WindowWidth;
        int pad = width > 0 ? Math.Max(0, (width - line.Length) / 2) : 0;

        Console.Write(new string(' ', pad) + line);
        return Console.ReadLine();
    }

    public static void DisplayMenu()
    {
        string[] options =
        {
            "Calculate the force between charges",
            "Exit"
        };

        int selected = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            WriteCenteredBlock(tittle);
            Console.WriteLine();
            WriteCenteredLine("Use ↑ ↓ to navigate and Enter to select");
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                string line = (i == selected ? $"> {options[i]}" : $"  {options[i]}");

                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                    WriteCenteredLine(line);
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    WriteCenteredLine(line);
                }
            }

            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
            {
                selected--;
                if (selected < 0) selected = options.Length - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selected++;
                if (selected >= options.Length) selected = 0;
            }
            else if (key == ConsoleKey.Enter)
            {
                switch (selected)
                {
                    case 0:
                        GetData();
                        break;
                    case 1:
                        return;
                }
            }

        } while (true);
    }

    private static void GetData()
    {
        Console.CursorVisible = true;
        try
        {
            Console.Clear();
            WriteCenteredLine("--- MAIN CHARGE DATA ---");
            Console.WriteLine();
            char mainChargeName = char.Parse(ReadCenteredLine("Enter the main charge name (Char): "));
            double mainChargeValue = double.Parse(ReadCenteredLine("Enter the main charge value: "));
            char mainChargeSign = char.Parse(ReadCenteredLine("Enter the main charge sign (+ or -): "));
            double mainChargeX = double.Parse(ReadCenteredLine("Enter the main charge X position: "));
            double mainChargeY = double.Parse(ReadCenteredLine("Enter the main charge Y position: "));

            MyVector2 mainChargePosition = new MyVector2(mainChargeX, mainChargeY);
            Charge mainCharge = new Charge(mainChargeValue, mainChargePosition, mainChargeName, mainChargeSign);

            Console.Clear();
            WriteCenteredLine("--- OTHER CHARGES DATA ---");
            Console.WriteLine();
            int numberOfCharges = int.Parse(ReadCenteredLine("Enter the number of other charges: "));

            CoulombCalcHandler handler = new CoulombCalcHandler(numberOfCharges, mainCharge);

            for (int i = 0; i < numberOfCharges; i++)
            {
                Console.WriteLine();
                WriteCenteredLine($"--- Data for Charge {i + 1} ---");
                double chargeValue = double.Parse(ReadCenteredLine("Value: "));
                char chargeSign = char.Parse(ReadCenteredLine("Sign (+ or -): "));
                char chargeName = char.Parse(ReadCenteredLine("Name: "));
                double chargeX = double.Parse(ReadCenteredLine("X position: "));
                double chargeY = double.Parse(ReadCenteredLine("Y position: "));

                MyVector2 chargePosition = new MyVector2(chargeX, chargeY);
                handler.AddCharge(new Charge(chargeValue, chargePosition, chargeName, chargeSign));
            }

            List<List<string>> results = handler.CalculateForces();
            ShowAndSaveResults(results);
        }
        catch (Exception)
        {
            Console.WriteLine();
            WriteCenteredLine("Invalid data. Press any key to return to menu.");
            Console.ReadKey();
        }
        Console.CursorVisible = false;
    }

    private static void ShowAndSaveResults(List<List<string>> results)
    {
        string filePath = "output_results.txt";
        StringBuilder sb = new StringBuilder();

        Console.Clear();
        WriteCenteredLine("CALCULATION RESULTS");
        Console.WriteLine();

        string header  = "┌─────────────────────────────────────────────────────────────────────────────┐";
        string divider = "├─────────────────────────────────────────────────────────────────────────────┤";
        string footer  = "└─────────────────────────────────────────────────────────────────────────────┘";

        Action<string> print = (s) =>
        {
            WriteCenteredLine(s);
            sb.AppendLine(s);
        };

        print(header);
        print("│                           COULOMB FORCE ANALYSIS                            │");
        print(divider);

        foreach (var group in results)
        {
            foreach (var line in group)
            {
                print($"│ {line,-75} │");
            }
            print(divider);
        }
        print(footer);

        Console.WriteLine();
        WriteCenteredLine($"Results have been saved to: {Path.GetFullPath(filePath)}");
        WriteCenteredLine("Press any key to return...");
        Console.ReadKey();
    }
}