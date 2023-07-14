using CommunityToolkit.Mvvm.ComponentModel;
using DriveLinker.Core.Enums;
using SQLite;

namespace DriveLinker.Core.Models;
public partial class Settings : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ObservableProperty]
    private bool _autoLink = true;

    [ObservableProperty]
    private bool _autoMinimize = true;

    [ObservableProperty]
    private Language _language = Language.English;
}
