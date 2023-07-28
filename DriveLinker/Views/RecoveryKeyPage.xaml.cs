namespace DriveLinker.Views;

public partial class RecoveryKeyPage : ContentPage
{
	public RecoveryKeyPage(RecoveryKeyViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}