namespace DriveLinker.Helpers.Interfaces;

public interface IWindowsHelper
{
    void CenterWindow();
    bool GetCheckedValue();
    void MinimizeWindow();
    void ToggleStartup(bool isChecked);
}