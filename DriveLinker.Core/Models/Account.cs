namespace DriveLinker.Core.Models;
public partial class Account : ObservableObject, IAccount
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [ObservableProperty]
    private string _username;
}
