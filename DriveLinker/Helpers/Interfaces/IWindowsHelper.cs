namespace DriveLinker.Helpers.Interfaces;

public interface IWindowsHelper
{
    bool ISValueNull();
    void MinimizeWindow();
    void ToggleStartup(bool isChecked);
}