namespace DriveLinker.Views.Auth;

public partial class RecoveryKeyPage : ContentPage
{
	public RecoveryKeyPage(RecoveryKeyViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}