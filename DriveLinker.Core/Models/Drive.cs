﻿namespace DriveLinker.Core.Models;
public partial class Drive : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int AccountId { get; set; }

    [ObservableProperty]
    private string _letter;

    [ObservableProperty]
    private string _ipAddress;

    [ObservableProperty]
    private string _driveName;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _userName;

    [ObservableProperty]
    private string _key;

    [ObservableProperty]
    private string _iv;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DriveButtonColorAsColor))]
    private string _buttonColor = "#FF0000";

    [ObservableProperty]
    private DateTime _dateCreated = DateTime.Now;

    [Ignore]
    public Color DriveButtonColorAsColor => Color.FromArgb(ButtonColor);

    [Ignore]
    public bool Connected { get; set; }

    [Ignore]
    public DriveInfo DriveInfo { get; set; }
}
