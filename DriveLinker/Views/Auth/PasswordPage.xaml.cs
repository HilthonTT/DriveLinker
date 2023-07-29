namespace DriveLinker.Views.Auth;

public partial class PasswordPage : ContentPage
{
	public PasswordPage(PasswordViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}