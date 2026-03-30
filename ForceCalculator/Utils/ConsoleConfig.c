#include <windows.h>

__declspec(dllexport) void __cdecl ConfigureConsole(int width, int height)
{
    HANDLE hOut = GetStdHandle(STD_OUTPUT_HANDLE);
    if (hOut == INVALID_HANDLE_VALUE)
        return;

    SMALL_RECT tinyRect = { 0, 0, 0, 0 };
    SetConsoleWindowInfo(hOut, TRUE, &tinyRect);

    COORD bufferSize;
    bufferSize.X = (SHORT)width;
    bufferSize.Y = (SHORT)height;
    SetConsoleScreenBufferSize(hOut, bufferSize);

    SMALL_RECT rect;
    rect.Left   = 0;
    rect.Top    = 0;
    rect.Right  = (SHORT)(width  - 1);
    rect.Bottom = (SHORT)(height - 1);
    SetConsoleWindowInfo(hOut, TRUE, &rect);

    HWND hWnd = GetConsoleWindow();
    if (hWnd != NULL)
    {
        LONG style = GetWindowLong(hWnd, GWL_STYLE);
        style &= ~WS_SIZEBOX;      
        style &= ~WS_MAXIMIZEBOX;  
        SetWindowLong(hWnd, GWL_STYLE, style);
    }
}