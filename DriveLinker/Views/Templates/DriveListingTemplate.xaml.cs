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
            MessagingCenter.Send(this, "ToggleLinkMessage", drive);
        }
    }
}