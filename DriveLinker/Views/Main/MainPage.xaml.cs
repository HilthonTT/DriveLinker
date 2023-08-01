namespace DriveLinker.Views.Main;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

        WeakReferenceMessenger.Default.Register<Message>(this,  async (r, m) =>
        {
            if (BindingContext is MainViewModel mainViewModel)
            {
                await mainViewModel.ToggleLinkAsync(m.Drive);
            }
        });
    }
}

