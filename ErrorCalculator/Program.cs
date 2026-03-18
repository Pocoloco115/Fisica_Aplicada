using System;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        bool keepRunning = true;

        while (keepRunning)
        {
            Console.WriteLine("=== Exponential Approximation ===");
            Console.WriteLine("1. Calculate approximation and errors");
            Console.WriteLine("2. Exit");
            Console.Write("Select an option: ");
            
            string input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    Menu();
                    break;
                case "2":
                    keepRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.\n");
                    break;
            }
        }
    }

    static void Menu()
    {
        string filePath = "output.txt";

        Console.WriteLine("\nEnter the value of x:");
        if (!double.TryParse(Console.ReadLine(), out double x))
        {
            Console.WriteLine("Invalid number. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Enter the number of terms n:");
        if (!int.TryParse(Console.ReadLine(), out int n) || n < 1)
        {
            Console.WriteLine("Invalid number (must be positive integer). Press any key to continue...");
            Console.ReadKey();
            return;
        }

        // Show table in console
        Console.WriteLine($"\nExponential approximation for x = {x}  (n = {n} terms)");
        Console.WriteLine("───────────────────────────────────────────────────────────────");
        Console.WriteLine("| Term | Approximation | Absolute Error | Relative Error (%) |");
        Console.WriteLine("|------|---------------|----------------|--------------------|");

        double actualValue = Math.Exp(x);

        for (int i = 0; i < n; i++)
        {
            double approximation = ApproximateExponential(x, i);
            double absoluteError  = Math.Abs(actualValue - approximation);
            double relativeError  = (actualValue != 0) 
                ? (absoluteError / Math.Abs(actualValue)) * 100 
                : 0;

            Console.WriteLine($"| {i + 1,4} | {approximation,13:F6} | {absoluteError,14:E6} | {relativeError,18:F2} |");
        }

        Console.WriteLine("───────────────────────────────────────────────────────────────");

        // Save full nice table to file
        SaveTableToFile(filePath, x, n);

        Console.WriteLine($"\nResults have been saved to: {filePath}");
        Console.WriteLine("Press any key to return to menu...");
        Console.ReadKey();
        Console.Clear();
    }

    static void SaveTableToFile(string filePath, double x, int n)
    {
        double trueValue = Math.Exp(x);
        var sb = new StringBuilder();

        // Header block
        sb.AppendLine($"Exponential series approximation");
        sb.AppendLine($"x = {x,-20}   terms = {n,-6}   {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine("───────────────────────────────────────────────────────────────");
        sb.AppendLine("┌──────┬────────────────┬───────────────────┬─────────────────────┐");
        sb.AppendLine("│ Term │  Approximation │   Absolute Error  │ Relative Error (%)  │");
        sb.AppendLine("├──────┼────────────────┼───────────────────┼─────────────────────┤");

        for (int i = 0; i < n; i++)
        {
            double approx   = ApproximateExponential(x, i);
            double absErr   = Math.Abs(trueValue - approx);
            double relErr   = (trueValue != 0) ? (absErr / Math.Abs(trueValue)) * 100 : 0;

            sb.AppendLine($"│ {i+1,4} │ {approx,14:F6} │ {absErr,17:E6} │ {relErr,19:F4} │");
        }

        sb.AppendLine("└──────┴────────────────┴───────────────────┴─────────────────────┘");
        sb.AppendLine($"True value (Math.Exp({x})) = {trueValue:F10}");
        sb.AppendLine("───────────────────────────────────────────────────────────────");
        sb.AppendLine();

        // Write to file (appends if file already exists)
        File.AppendAllText(filePath, sb.ToString());
    }

    static double ApproximateExponential(double x, int terms)
    {
        // terms = 0 → 1
        // terms = 1 → 1 + x
        // terms = 2 → 1 + x + x²/2!
        // etc.
        double sum = 1.0;
        double term = 1.0;   // current power(x)/factorial term

        for (int k = 1; k <= terms; k++)
        {
            term *= x / k;
            sum += term;
        }

        return sum;
    }
}