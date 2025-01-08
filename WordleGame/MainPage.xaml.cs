namespace WordleGame
{
    public partial class MainPage : ContentPage
    {
        Color LightBackgroundColor = Color.FromRgb(255, 255, 255);
        Color DarkBackgroundColor = Color.FromRgb(0, 0, 0);

        private GameViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();

            viewModel = new GameViewModel();
            BindingContext = viewModel;

            bool isDarkTheme = Preferences.Get("IsDarkTheme", true);
            this.BackgroundColor = isDarkTheme ? LightBackgroundColor : DarkBackgroundColor;

        }

        private  async void StartGame_Clicked(object sender, EventArgs e)
        {

            try
            {
                string playerName = NameEntry.Text?.Trim();
                // await Navigation.PushAsync(new gamePage(playerName));

                if (!string.IsNullOrEmpty(playerName))
                {
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
    }
}
