using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace DriveLinker.Core.Models;
public partial class Settings : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ObservableProperty]
    private bool _autoConnect = true;

    [ObservableProperty]
    private bool _autoMinimize = true;
}
