namespace DriveLinker.Views.Main;

public partial class CreatePage : ContentPage
{
	public CreatePage(CreateViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}