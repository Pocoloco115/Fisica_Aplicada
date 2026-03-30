using System;

class Program
{
    static void Main()
    {
        Init();
        Menu.Start();
    }

    private static void Init()
    {
        Console.Clear();
        Console.Title = "Coulomb's Force Calculator";
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.Green;
    }
}