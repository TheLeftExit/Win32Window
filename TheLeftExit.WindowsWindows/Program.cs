using System.Runtime.InteropServices;

using static User32;
using static Gdi32;

[assembly: System.Runtime.CompilerServices.DisableRuntimeMarshalling]

const string className = "WindowsWindow";
const string windowName = "Windows Window";

var classNameBuffer = Marshal.StringToHGlobalUni(className);
var windowNameBuffer = Marshal.StringToHGlobalUni(windowName);

WNDPROC myWndProc = (hwnd, uMsg, wParam, lParam) =>
{
    switch (uMsg)
    {
        case WINDOW_MESSAGE.WM_CLOSE:
            DestroyWindow(hwnd);
            return 0;

        case WINDOW_MESSAGE.WM_DESTROY:
            PostQuitMessage(0);
            return 0;

        default:
            return DefWindowProc(hwnd, uMsg, wParam, lParam);
    }
};

var wndClass = new WNDCLASSEXW
{
    cbSize = (uint)Marshal.SizeOf<WNDCLASSEXW>(),
    style = WNDCLASS_STYLES.CS_HREDRAW | WNDCLASS_STYLES.CS_VREDRAW,
    lpfnWndProc = Marshal.GetFunctionPointerForDelegate(myWndProc),
    lpszClassName = classNameBuffer,
    hbrBackground = GetStockObject(GET_STOCK_OBJECT_FLAGS.WHITE_BRUSH)
};
RegisterClassEx(wndClass);

var hWnd = CreateWindowEx(
    WINDOW_EX_STYLE.WS_EX_OVERLAPPEDWINDOW,
    classNameBuffer,
    windowNameBuffer,
    WINDOW_STYLE.WS_OVERLAPPEDWINDOW,
    CW_USEDEFAULT,
    CW_USEDEFAULT,
    CW_USEDEFAULT,
    CW_USEDEFAULT,
    default,
    default,
    default,
    default
);

ShowWindow(hWnd, SHOW_WINDOW_CMD.SW_SHOWDEFAULT);
UpdateWindow(hWnd);

MSG msg;
while(GetMessage(out msg, 0, 0, 0))
{
    TranslateMessage(msg);
    DispatchMessage(msg);
}

DestroyWindow(hWnd);

Marshal.FreeHGlobal(windowNameBuffer);
Marshal.FreeHGlobal(classNameBuffer);