using CommunityToolkit.Maui.Alerts;
using DriveLinker.Core.Linker.Interfaces;
using DriveLinker.Core.Models;
using System.Diagnostics;

namespace DriveLinker.Core.Linker;
public class Linker : ILinker
{
    public async Task ConnectDriveAsync(Drive drive)
    {
        string arguments = $"use {drive.Letter}: \"\\\\{drive.IpAddress}\\{drive.DriveName}\" {drive.Password} /user:{drive.UserName} /persistent:no";

        var startInfo = new ProcessStartInfo()
        {
            FileName = "net",
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
}
