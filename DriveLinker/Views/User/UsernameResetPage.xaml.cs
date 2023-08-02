namespace DriveLinker.Views.User;

public partial class UsernameResetPage : ContentPage
{
	public UsernameResetPage(UsernameResetViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}