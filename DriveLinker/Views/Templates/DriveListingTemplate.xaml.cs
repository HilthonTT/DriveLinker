namespace DriveLinker.Views.Templates;

public partial class DriveListingTemplate : ContentView
{
	public DriveListingTemplate()
	{
		InitializeComponent();
	}

    private void OnButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is Drive drive)
        {
            WeakReferenceMessenger.Default.Send(new Message(drive));
        }
    }
}