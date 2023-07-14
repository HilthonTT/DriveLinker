using DriveLinker.Core.Models;

namespace DriveLinker.Core.Linker.Interfaces;
public interface ILinker
{
    Task ConnectDriveAsync(Drive drive);
    Task DisconnectDriveAsync(Drive drive);
    bool IsDriveConnected(Drive drive);
}