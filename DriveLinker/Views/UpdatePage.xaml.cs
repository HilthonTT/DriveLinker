namespace DriveLinker.Views;

public partial class UpdatePage : ContentPage
{
	public UpdatePage(UpdateViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}