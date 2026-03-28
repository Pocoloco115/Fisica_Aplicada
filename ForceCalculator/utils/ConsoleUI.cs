public static class ConsoleUI
{
    public static Action ClearAction = () => ConsoleUI.Clear();

    public static void Clear()
    {
        ClearAction();
    }
}