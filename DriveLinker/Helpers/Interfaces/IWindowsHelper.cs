namespace DriveLinker.Helpers.Interfaces;

public interface IWindowsHelper
{
    bool GetCheckedValue();
    void MinimizeWindow();
    void ToggleStartup(bool isChecked);
}