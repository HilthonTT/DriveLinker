using PInvoke;

namespace DriveLinker.Helpers;
public partial class WindowsHelper : IWindowsHelper
{
    public void MinimizeWindow()
    {
#if WINDOWS
            var mauiWindow = Application.Current.Windows[0];
            var nativeWindow = mauiWindow.Handler.PlatformView;
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);

            User32.ShowWindow(windowHandle, User32.WindowShowStyle.SW_MINIMIZE);
#endif
    }
}
