namespace DriveLinker.Views;

public partial class DrivePage : ContentPage
{
	public DrivePage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}