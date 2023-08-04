namespace DriveLinker.Views.Auth;

public partial class AuthPage : ContentPage
{
    private const string Key = nameof(AuthPage);
    private readonly IWindowsHelper _windowsHelper;
    private readonly IMemoryCache _cache;

    public AuthPage(
        AuthViewModel viewModel,
        IWindowsHelper windowsHelper,
        IMemoryCache cache)
	{
        InitializeComponent();
		BindingContext = viewModel;
        _windowsHelper = windowsHelper;
        _cache = cache;
    }

    protected override void OnAppearing()
    {
        // Checks if first boot

        bool isFirstBoot = _cache.Get<bool>(Key);
        if (isFirstBoot is false)
        {
            _windowsHelper.CenterWindow();
            _cache.Set(Key, true);
        }
    }
}