
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

            

            if(viewModel.IsDarkTheme == true)
            {
                darkTheme();
            }
            else
            {
                lightTheme();
            }

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

        private void darkTheme()
        {
           mainPage.BackgroundColor = DarkBackgroundColor;
           NameEntry.TextColor = DarkTextColor;
           NameEntry.PlaceholderColor = DarkTextColor;
           Login.TextColor = DarkTextColor;
           Player.TextColor = DarkTextColor;
           StartGame.BackgroundColor = Colors.LightBlue;
           StartGame.TextColor = DarkTextColor;

        }

        private void lightTheme()
        {
            mainPage.BackgroundColor = LightBackgroundColor;
            NameEntry.TextColor = LightTextColor;
            NameEntry.PlaceholderColor = LightTextColor;
            Login.TextColor = LightTextColor;
            Player.TextColor = LightTextColor;
            StartGame.BackgroundColor = LightBackgroundColor;
            StartGame.TextColor = LightTextColor;
        }
    }
}
