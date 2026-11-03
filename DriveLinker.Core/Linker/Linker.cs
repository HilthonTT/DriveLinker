namespace DriveLinker.Core.Linker;

/// <summary>
/// Maps and unmaps network drives with the Windows <c>net use</c> command.
/// </summary>
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
        // net use X: "\\host\share" <password> /user:<user> /persistent:no
        var (exitCode, error) = await RunNetUseAsync(
            $"{drive.Letter}:",
            $@"\\{drive.IpAddress}\{drive.DriveName}",
            drive.Password,
            $"/user:{drive.UserName}",
            "/persistent:no");

        IsDriveConnected(drive);

        await ReportErrorAsync(exitCode, error, drive);
    }

    public async Task DisconnectDriveAsync(Drive drive)
    {
        var (exitCode, error) = await RunNetUseAsync($"{drive.Letter}:", "/delete", "/yes");

        IsDriveConnected(drive);

        await ReportErrorAsync(exitCode, error, drive);
    }

    public bool IsDriveConnected(Drive drive)
    {
        bool directoryExists = Directory.Exists($"{drive.Letter}:\\");

        drive.Connected = directoryExists;
        drive.ButtonColor = directoryExists ? Green : Red;

        return directoryExists;
    }

    private static async Task<(int ExitCode, string Error)> RunNetUseAsync(params string[] arguments)
    {
        var startInfo = new ProcessStartInfo("net")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
        };

        // ArgumentList quotes each value individually, so passwords and share names containing
        // spaces or quotes are passed through intact.
        startInfo.ArgumentList.Add("use");
        foreach (string argument in arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Could not start 'net use'.");

        // Drain both streams while waiting so a full pipe buffer can never block the process.
        var errorTask = process.StandardError.ReadToEndAsync();
        var outputTask = process.StandardOutput.ReadToEndAsync();

        await process.WaitForExitAsync();

        string error = await errorTask;
        await outputTask;

        return (process.ExitCode, error.Trim());
    }

    private async Task ReportErrorAsync(int exitCode, string message, Drive drive)
    {
        if (exitCode is 0)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            message = $"'net use' failed for drive {drive.Letter}: (exit code {exitCode}).";
        }

        _stackTrace.ErrorMessages.Add(message);

        string snackbarText = message.Contains("local device is already in use", StringComparison.OrdinalIgnoreCase)
            ? $"Drive {drive.Letter} is already in use."
            : message;

        await MainThread.InvokeOnMainThreadAsync(() => Shell.Current.DisplaySnackbar(snackbarText));
    }
}
