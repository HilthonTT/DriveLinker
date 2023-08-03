namespace DriveLinker.Core.Linker;
public class Linker : ILinker
{
    private const string Green = "#00FF00";
    private const string Red = "#FF0000";
    private readonly IStackTrace _stackTrace;

    public Linker(IStackTrace stackTrace)
    {
        _stackTrace = stackTrace;
    }

    public async Task ConnectDriveAsync(Drive drive)
    {
        string arguments = GetConnectArguments(drive);
        string fileName = GetFileName();

        var process = GetProcess(fileName, arguments);
        process.Start();

        await process.WaitForExitAsync();

        if (IsDriveConnected(drive))
        {
            drive.ButtonColor = Green;
        }
        else
        {
            drive.ButtonColor = Red;
        }

        await GetErrorAsync(process, drive);
    }

    public async Task DisconnectDriveAsync(Drive drive)
    {
        drive.Connected = false;
        drive.ButtonColor = Red;

        string arguments = GetDeleteArguments(drive);
        string fileName = GetFileName();

        var process = GetProcess(fileName, arguments);
        process.Start();

        await process.WaitForExitAsync();
        await GetErrorAsync(process, drive);
    }

    public bool IsDriveConnected(Drive drive)
    {
        string directory = drive.Letter + ':';
        bool directoryExists = Directory.Exists(directory);

        if (directoryExists)
        {
            drive.Connected = true;
            drive.ButtonColor = Green;
        }
        else
        {
            drive.Connected = false;
            drive.ButtonColor = Red;
        }

        return directoryExists;
    }

    private async Task GetErrorAsync(Process process, Drive drive)
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

        _stackTrace.ErrorMessages.Add(message);
    }

    private static Process GetProcess(string fileName, string arguments)
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = fileName,
            Arguments = arguments,
            CreateNoWindow = true,
            RedirectStandardError = true,
        };

        return new Process { StartInfo = startInfo };
    }

    private static string GetConnectArguments(Drive drive)
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

    private static string GetDeleteArguments(Drive drive)
    {
        if (IsWindows())
        {
            return $"use {drive.Letter}: /del";
        }
        else if (IsMacOS())
        {
            return $"umount {drive.Letter}";
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
