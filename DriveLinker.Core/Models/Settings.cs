﻿namespace DriveLinker.Core.Models;
public partial class Settings : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ObservableProperty]
    private bool _autoLink = true;

    [ObservableProperty]
    private bool _autoMinimize = false;

    [ObservableProperty]
    private Language _language = Language.English;
}
