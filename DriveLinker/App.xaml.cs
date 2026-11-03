namespace DriveLinker;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// The UI is designed for a dark theme (many labels use hard-coded white text), so pin it
		// instead of following the OS: in Windows light mode that text was invisible.
		UserAppTheme = AppTheme.Dark;
	}

	protected override Window CreateWindow(IActivationState activationState)
	{
		return new Window(new AppShell());
	}
}
