namespace DriveLinker.Core.Models;
public partial class Account : ObservableObject
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [ObservableProperty]
    private string _username;

    [Indexed]
    public int SettingsId { get; set; }
}
