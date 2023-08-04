using Microsoft.Win32;
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

    private static RegistryKey GetRegistryKey()
    {
        return Registry.CurrentUser.OpenSubKey
             ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
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

    public bool ISValueNull()
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
