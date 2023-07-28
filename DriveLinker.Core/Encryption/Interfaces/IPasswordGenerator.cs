namespace DriveLinker.Core.Encryption.Interfaces;

public interface IPasswordGenerator
{
    Task<string> FetchPasswordAsync();
}
