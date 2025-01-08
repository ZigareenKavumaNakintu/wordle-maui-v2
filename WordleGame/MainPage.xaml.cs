

namespace WordleGame
{
    public partial class MainPage : ContentPage
    {
        private readonly Color LightBackgroundColor = Color.FromRgb(255, 255, 255);
        private readonly Color DarkBackgroundColor = Color.FromRgb(0, 0, 0);
        private readonly Color LightTextColor = Color.FromRgb(0, 0, 0);
        private readonly Color DarkTextColor = Color.FromRgb(255, 255, 255);

        private GameViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();

            viewModel = new GameViewModel();
            BindingContext = viewModel;
            LoadTheme();

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(GameViewModel.IsDarkTheme))
                {
                    ApplyTheme(viewModel.IsDarkTheme);
                }
            };

        }
       

        private  async void StartGame_Clicked(object sender, EventArgs e)
        {

            try
            {
                string playerName = NameEntry.Text?.Trim().ToUpper();
                // await Navigation.PushAsync(new gamePage(playerName));

                if (!string.IsNullOrEmpty(playerName))
                {
                    //move to the next page but take the playerName as a query parameter
                    await Shell.Current.GoToAsync($"gamePage?playerName={playerName}");
                }
                else
                {
                    await DisplayAlert("Error", "Please enter your name before starting the game.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Navigation Error", "An error occurred: {ex.Message}", "OK");
            }
            //await Navigation.PushAsync(new gamePage());

        }
        private void LoadTheme()
        {
            // Get the theme value from Preferences
            viewModel.IsDarkTheme = Preferences.Get("IsDarkTheme", false);

            
            ApplyTheme(viewModel.IsDarkTheme);
        }

        private void ApplyTheme(bool isDarkTheme)
        {
            this.BackgroundColor = isDarkTheme ? DarkBackgroundColor : LightBackgroundColor;

            NameEntry.TextColor = isDarkTheme ? DarkTextColor : LightTextColor;
            NameEntry.PlaceholderColor = isDarkTheme ? DarkTextColor : LightTextColor;

            Login.TextColor = isDarkTheme ? DarkTextColor : LightTextColor;
            Player.TextColor = isDarkTheme ? DarkTextColor : LightTextColor;

            StartGame.BackgroundColor = isDarkTheme ? Colors.LightBlue : LightBackgroundColor;
            StartGame.TextColor = isDarkTheme ? DarkTextColor : LightTextColor;
        }
    }
}

