namespace DriveLinker.Views;

public partial class PasswordPage : ContentPage
{
	public PasswordPage(PasswordViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}