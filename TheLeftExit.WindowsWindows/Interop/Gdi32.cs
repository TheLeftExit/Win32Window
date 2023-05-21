using System.Runtime.InteropServices;

public unsafe static partial class Gdi32
{
    [LibraryImport("gdi32.dll")]
    public static partial nint GetStockObject(GET_STOCK_OBJECT_FLAGS i);
}
