namespace DriveLinker.Views.Auth;

public partial class AuthPage : ContentPage
{
    public AuthPage(AuthViewModel viewModel)
	{
        InitializeComponent();
		BindingContext = viewModel;
    }
}