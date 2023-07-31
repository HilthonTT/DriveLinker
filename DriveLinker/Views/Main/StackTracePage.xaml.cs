namespace DriveLinker.Views.Main;

public partial class StackTracePage : ContentPage
{
	public StackTracePage(StackTraceViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}