using System.Runtime.InteropServices;

using static User32;
using static DwmApi;

[assembly: System.Runtime.CompilerServices.DisableRuntimeMarshalling]

using var className = new MarshalledString("WindowsWindow");
using var windowName = new MarshalledString("Windows Window");
using var buttonClassName = new MarshalledString("Button");
using var buttonName = new MarshalledString("Click ,e!");

nint hButton;

WNDPROC myWndProc = (hwnd, uMsg, wParam, lParam) =>
{
    switch (uMsg)
    {
        case WINDOW_MESSAGE.WM_CREATE:
            hButton = CreateWindowEx(0, buttonClassName, buttonName,
                WINDOW_STYLE.WS_CHILD | WINDOW_STYLE.WS_VISIBLE,
                50, 50, 100, 30, hwnd, 1, 0, IntPtr.Zero);

            /* Mica experiments - (https://github.com/Brouilles/win32-mica/blob/main/main.cpp)
            var buffer = Marshal.AllocHGlobal(4);

            Marshal.WriteInt32(buffer, 1);
            DwmSetWindowAttribute(hwnd, 20, buffer, 4);

            Marshal.WriteInt32(buffer, 4);
            DwmSetWindowAttribute(hwnd, 38, buffer, 4);
            
            Marshal.FreeHGlobal(buffer);
            */
            return 0;
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
    lpszClassName = className,
    hbrBackground = GetSysColorBrush(SYS_COLOR_INDEX.COLOR_BTNFACE)
};
RegisterClassEx(wndClass);

var hWnd = CreateWindowEx(
    WINDOW_EX_STYLE.WS_EX_OVERLAPPEDWINDOW /* To enable Mica backdrop but hide all controls: WINDOW_EX_STYLE.WS_EX_NOREDIRECTIONBITMAP */,
    className,
    windowName,
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