using DriveLinker.Views.Templates;

namespace DriveLinker;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

        MessagingCenter.Subscribe<DriveListingTemplate, Drive>(this, "ToggleLinkMessage", async (sender, drive) =>
        {
            // Call the ToggleLinkCommand in the MainViewModel
            if (BindingContext is MainViewModel mainViewModel)
            {
                await mainViewModel.ToggleLinkCommand.ExecuteAsync(drive);
            }
        });

        MessagingCenter.Subscribe<DriveTemplate, Drive>(this, "ToggleLinkMessage", async (sender, drive) =>
        {
            // Call the ToggleLinkCommand in the MainViewModel
            if (BindingContext is MainViewModel mainViewModel)
            {
                await mainViewModel.ToggleLinkCommand.ExecuteAsync(drive);
            }
        });
    }
}

