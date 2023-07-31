namespace DriveLinker.Views.Templates;

public partial class ErrorMessageTemplate : ContentView
{
	public ErrorMessageTemplate()
	{
		InitializeComponent();
	}

	private async void CopyToClipboardClicked(object sender, EventArgs e)
	{
		if (BindingContext is string message)
		{
			await Clipboard.SetTextAsync(message);

            await Shell.Current.DisplayAlert("Text Copied", "The text has been copied to the clipboard.", "OK");
        }
	}
}