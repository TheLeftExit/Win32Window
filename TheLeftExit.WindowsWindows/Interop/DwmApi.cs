using System.Runtime.InteropServices;

public unsafe static partial class DwmApi
{
    [LibraryImport("dwmapi.dll")]
    public static partial int DwmSetWindowAttribute(nint hwnd, int dwAttribute, nint pvAttribute, uint cbAttribute);
}