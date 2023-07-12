namespace DriveLinker.Views;

public partial class DrivesPage : ContentPage
{
	public DrivesPage(DrivesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}