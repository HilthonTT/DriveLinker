namespace DriveLinker.Views.User;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}