
namespace WordleGame;
[QueryProperty(nameof(PlayerName), "playerName")]
public partial class Settings : ContentPage
{
   // private GameViewModel viewModel;
    private GameViewModel _viewModel;
    private bool isDarkTheme;
    public string PlayerName { get; set; }
    
    public Settings()
    {
        InitializeComponent();
        BindingContext = _viewModel;
        //setBackgroundColor();


    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"gamePage?playerName={PlayerName}");
    }
    /*
    private void setBackgroundColor()
    {
        if (_viewModel.IsDarkTheme == true)
        {
            mainPage.BackgroundColor = DarkBackgroundColor;
            ThemeChange.BackgroundColor = DarkBackgroundColor;
            BackButton.BackgroundColor = Colors.LightBlue;


        }
        //if darkmode is turned on
        else if (_viewModel.IsDarkTheme == false)
        {
            mainPage.BackgroundColor = LightBackgroundColor;
            ThemeChange.BackgroundColor = LightBackgroundColor;
            BackButton.BackgroundColor = Colors.LightBlue;


        }
    */



    
    }

