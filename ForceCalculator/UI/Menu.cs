using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class Menu
{
    private static string tittle = @"
     ____             _                 _       _____                     ____      _            _       _             
    / ___| ___  _   _| | ___  _ __ ___ | |__   |  ___|__  _ __ ___ ___   / ___|__ _| | ___ _   _| | __ _| |_ ___  _ __ 
    | |   / _ \| | | | |/ _ \| '_ ` _ \| '_ \  | |_ / _ \| '__/ __/ _ \ | |   / _` | |/ __| | | | |/ _` | __/ _ \| '__|
    | |__| (_) | |_| | | (_) | | | | | | |_) | |  _| (_) | | | (_|  __/ | |__| (_| | | (__| |_| | | (_| | || (_) | |   
     \____\___/ \__,_|_|\___/|_| |_| |_|_.__/  |_|  \___/|_|  \___\___|  \____\__,_|_|\___|\__,_|_|\__,_|\__\___/|_|   
                                                                                                                                                                                                                                            
    ";

    public static void Start()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        DisplayMenu();
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
        Console.WriteLine(tittle);
        Console.WriteLine("\nUse ↑ ↓ to navigate and Enter to select\n");

        for (int i = 0; i < options.Length; i++)
        {
            if (i == selected)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine($"> {options[i]}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.WriteLine($"  {options[i]}");
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
            Console.WriteLine("--- INPUT DATA --- \n");

            Console.Write("Enter the main charge value: ");
            double mainChargeValue = double.Parse(Console.ReadLine());
            Console.Write("Enter the main charge sign (+ or -): ");
            char mainChargeSign = char.Parse(Console.ReadLine());
            Console.Write("Enter the main charge name: ");
            char mainChargeName = char.Parse(Console.ReadLine());
            Console.Write("Enter the main charge X position: ");
            double mainChargeX = double.Parse(Console.ReadLine());
            Console.Write("Enter the main charge Y position: ");
            double mainChargeY = double.Parse(Console.ReadLine());

            Vector2 mainChargePosition = new Vector2(mainChargeX, mainChargeY);
            Charge mainCharge = new Charge(mainChargeValue, mainChargePosition, mainChargeName, mainChargeSign);

            Console.Write("\nEnter the number of other charges: ");
            int numberOfCharges = int.Parse(Console.ReadLine());

            CoulombCalcHandler handler = new CoulombCalcHandler(numberOfCharges, mainCharge);

            for (int i = 0; i < numberOfCharges; i++)
            {
                Console.WriteLine($"\n--- Data for Charge {i + 1} ---");
                Console.Write("Value: ");
                double chargeValue = double.Parse(Console.ReadLine());
                Console.Write("Sign (+ or -): ");
                char chargeSign = char.Parse(Console.ReadLine());
                Console.Write("Name: ");
                char chargeName = char.Parse(Console.ReadLine());
                Console.Write("X position: ");
                double chargeX = double.Parse(Console.ReadLine());
                Console.Write("Y position: ");
                double chargeY = double.Parse(Console.ReadLine());

                Vector2 chargePosition = new Vector2(chargeX, chargeY);
                handler.AddCharge(new Charge(chargeValue, chargePosition, chargeName, chargeSign));
            }

            List<List<string>> results = handler.CalculateForces();
            ShowAndSaveResults(results);
        }
        catch (Exception)
        {
            Console.WriteLine("\nInvalid data. Press any key to return to menu.");
            Console.ReadKey();
        }
        Console.CursorVisible = false;
    }

    private static void ShowAndSaveResults(List<List<string>> results)
    {
        string filePath = "output_results.txt";
        StringBuilder sb = new StringBuilder();

        Console.Clear();
        Console.WriteLine("CALCULATION RESULTS\n");

        string header  = "┌─────────────────────────────────────────────────────────────────────────────┐";
        string divider = "├─────────────────────────────────────────────────────────────────────────────┤";
        string footer  = "└─────────────────────────────────────────────────────────────────────────────┘";

        Action<string> print = (s) => { Console.WriteLine(s); sb.AppendLine(s); };

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

        File.WriteAllText(filePath, sb.ToString());

        Console.WriteLine($"\nResults have been saved to: {Path.GetFullPath(filePath)}");
        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }
}