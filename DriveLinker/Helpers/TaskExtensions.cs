using System.Diagnostics;

namespace DriveLinker.Helpers;

public static class TaskExtensions
{
    /// <summary>
    /// Runs <paramref name="task"/> without awaiting it (e.g. from a constructor), making sure an
    /// exception is logged instead of being silently dropped or crashing the process.
    /// </summary>
    public static async void FireAndForget(this Task task)
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Background task failed: {ex}");
        }
    }
}
