using CommunityToolkit.Maui.Alerts;
using DriveLinker.Core.Linker.Interfaces;
using DriveLinker.Core.Models;
using System.Diagnostics;

namespace DriveLinker.Core.Linker;
public class Linker : ILinker
{
    public async Task ConnectDriveAsync(Drive drive)
    {
        string arguments = GetArguments(drive);
        string fileName = GetFileName();

        var startInfo = new ProcessStartInfo()
        {
            FileName = fileName,
            Arguments = arguments,
            CreateNoWindow = true,
            RedirectStandardError = true,
        };

        var process = new Process { StartInfo = startInfo, };
        process.Start();

        await process.WaitForExitAsync();

        await GetErrorAsync(process, drive);
    }

    public bool IsDriveConnected(Drive drive)
    {
        string directory = drive.Letter + ':';
        bool directoryExists = Directory.Exists(directory);

        return directoryExists;
    }

    private static async Task GetErrorAsync(Process process, Drive drive)
    {
        string message = await process.StandardError.ReadToEndAsync();

        if (process.ExitCode is 0)
        {
            return;
        }

        if (message.Contains("local device is already in use"))
        {
            await Shell.Current
                .DisplaySnackbar($"Drive {drive.Letter} is already in use.");
        }
        else
        {
            await Shell.Current
                .DisplaySnackbar(message);
        }
    }

    private static string GetArguments(Drive drive)
    {
        if (IsWindows())
        {
            return $"use {drive.Letter}: " +
                $"\"\\\\{drive.IpAddress}\\{drive.DriveName}\" " +
                $"{drive.Password} " +
                $"/user:{drive.UserName} " +
                $"/persistent:no";
        }
        else if (IsMacOS())
        {
            return $"mount_smbfs //{drive.UserName}:" +
                $"{drive.Password}" +
                $"@{drive.IpAddress}" +
                $"/{drive.DriveName}" +
                $" /Volumes/{drive.Letter}";
        }
        else
        {
            return "";
        }
    }

    private static string GetFileName()
    {
        if (OperatingSystem.IsWindows())
        {
            return "net";
        }
        else if (IsMacOS())
        {
            return "mount";
        }
        else
        {
            return "";
        }
    }

    private static bool IsWindows()
    {
        return OperatingSystem.IsWindows();
    }

    private static bool IsMacOS()
    {
        return OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst();
    }
}
