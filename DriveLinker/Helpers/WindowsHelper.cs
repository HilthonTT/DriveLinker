using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DriveLinker.Helpers;

/// <summary>
/// Windows-specific window management and "start with Windows" support.
/// </summary>
public partial class WindowsHelper : IWindowsHelper
{
    private const string AppName = "DriveLinker";

    public void MinimizeWindow()
    {
#if WINDOWS
        IntPtr windowHandle = GetMainWindowHandle();
        if (windowHandle != IntPtr.Zero)
        {
            NativeMethods.ShowWindow(windowHandle, NativeMethods.SW_MINIMIZE);
        }
#endif
    }

    public void CenterWindow()
    {
#if WINDOWS
        IntPtr windowHandle = GetMainWindowHandle();
        if (windowHandle == IntPtr.Zero || !NativeMethods.GetWindowRect(windowHandle, out var windowRect))
        {
            return;
        }

        int screenWidth = NativeMethods.GetSystemMetrics(NativeMethods.SM_CXSCREEN);
        int screenHeight = NativeMethods.GetSystemMetrics(NativeMethods.SM_CYSCREEN);

        int windowWidth = windowRect.Right - windowRect.Left;
        int windowHeight = windowRect.Bottom - windowRect.Top;

        int x = (screenWidth - windowWidth) / 2;
        int y = (screenHeight - windowHeight) / 2;

        NativeMethods.SetWindowPos(windowHandle, IntPtr.Zero, x, y, 0, 0,
            NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOZORDER);
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

    public bool GetCheckedValue()
    {
        return File.Exists(GetShortcutPath());
    }

    private static void CreateShortcut()
    {
        string shortcutPath = GetShortcutPath();
        if (File.Exists(shortcutPath))
        {
            return;
        }

        string executablePath = Environment.ProcessPath ?? Process.GetCurrentProcess().MainModule!.FileName;

        // Late-bound WScript.Shell keeps the build free of a COMReference, which the
        // `dotnet` CLI (and therefore CI) cannot resolve.
        Type shellType = Type.GetTypeFromProgID("WScript.Shell")
            ?? throw new PlatformNotSupportedException("WScript.Shell is not available.");

        dynamic shell = Activator.CreateInstance(shellType)!;
        try
        {
            dynamic shortcut = shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = executablePath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(executablePath);
            shortcut.Description = "DriveLinker startup shortcut.";
            shortcut.Save();
        }
        finally
        {
            Marshal.FinalReleaseComObject(shell);
        }
    }

    private static void DeleteShortcut()
    {
        File.Delete(GetShortcutPath());
    }

    private static string GetShortcutPath()
    {
        string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        return Path.Combine(startupFolder, $"{AppName}.lnk");
    }

#if WINDOWS
    private static IntPtr GetMainWindowHandle()
    {
        var nativeWindow = Application.Current?.Windows.FirstOrDefault()?.Handler?.PlatformView;
        return nativeWindow is null ? IntPtr.Zero : WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
    }

    private static class NativeMethods
    {
        public const int SW_MINIMIZE = 6;
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOZORDER = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
    }
#endif
}
