namespace DriveLinker.Views;

public partial class DrivePage : ContentPage
{
	public DrivePage(DriveViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}