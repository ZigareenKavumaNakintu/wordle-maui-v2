namespace WordleGame;

public partial class Settings : ContentPage
{
    //private GameViewModel _viewModel;
    private bool isDarkTheme;
    Color LightBackgroundColor = Color.FromRgb(255, 255, 255);
    Color DarkBackgroundColor = Color.FromRgb(0,0,0);
    public Settings()
	{
		InitializeComponent();
		BindingContext = new GameViewModel();
        //load the preferences
        isDarkTheme = Preferences.Get("IsDarkTheme", true);
        ThemeSwitch.IsToggled = isDarkTheme;
        this.BackgroundColor = isDarkTheme ? LightBackgroundColor : DarkBackgroundColor;    
	}

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("gamePage");
    }

    private void ThemSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        isDarkTheme = e.Value;
        Preferences.Set("IsDarkTheme",isDarkTheme);

        if (isDarkTheme) {
            this.BackgroundColor = LightBackgroundColor;
            
            
        }
        else
        {
            this.BackgroundColor = DarkBackgroundColor;
        }
    }
}