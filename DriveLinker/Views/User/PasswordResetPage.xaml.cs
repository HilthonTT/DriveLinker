namespace DriveLinker.Views.User;

public partial class PasswordResetPage : ContentPage
{
	public PasswordResetPage(PasswordResetViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}