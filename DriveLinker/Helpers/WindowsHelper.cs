using PInvoke;
using System.Diagnostics;
using IWshRuntimeLibrary;

namespace DriveLinker.Helpers;
public partial class WindowsHelper : IWindowsHelper
{
    private const string AppName = "DriveLinker";
    private const string Key = $"{nameof(WindowsHelper)}_IsChecked";

    public void MinimizeWindow()
    {
#if WINDOWS
            var mauiWindow = Application.Current.Windows[0];
            var nativeWindow = mauiWindow.Handler.PlatformView;
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);

            User32.ShowWindow(windowHandle, User32.WindowShowStyle.SW_MINIMIZE);
#endif
    }


    public void CenterWindow()
    {
#if WINDOWS
        var mauiWindow = Application.Current.Windows[0];
        var nativeWindow = mauiWindow.Handler.PlatformView;
        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);

        var screenWidth = User32.GetSystemMetrics(User32.SystemMetric.SM_CXSCREEN);
        var screenHeight = User32.GetSystemMetrics(User32.SystemMetric.SM_CYSCREEN);

        User32.GetWindowRect(windowHandle, out RECT windowRect);
        int windowWidth = windowRect.right - windowRect.left;
        int windowHeight = windowRect.bottom - windowRect.top;

        int x = (screenWidth - windowWidth) / 2;
        int y = (screenHeight - windowHeight) / 2;

        User32.SetWindowPos(windowHandle, IntPtr.Zero, x, y, 0, 0, User32.SetWindowPosFlags.SWP_NOSIZE | User32.SetWindowPosFlags.SWP_NOZORDER);
#endif
    }

    public void ToggleStartup(bool isChecked)
    {
        if (OperatingSystem.IsWindows() is false)
        {
            return;
        }

        if (isChecked)
        {
            CreateShortcut();
        }
        else
        {
            DeleteShortcut();
        }
    }

    private static void CreateShortcut()
    {
        string executablePath = Process.GetCurrentProcess().MainModule.FileName;
        string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

        string shortcutPath = Path.Combine(startupFolder, $"{AppName}.lnk");

        dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell"));

        var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
        shortcut.TargetPath = executablePath;
        shortcut.WorkingDirectory = Path.GetDirectoryName(executablePath);
        shortcut.Description = "DriveLinker startup shortcut.";
        shortcut.Save();
    }

    private static void DeleteShortcut()
    {
        string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        string shortcutPath = Path.Combine(startupFolder, $"{AppName}.lnk");

        System.IO.File.Delete(shortcutPath);
    }

    public bool GetCheckedValue()
    {
        string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        string shortcutPath = Path.Combine(startupFolder, $"{AppName}.lnk");

        if (System.IO.File.Exists(shortcutPath) is false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
