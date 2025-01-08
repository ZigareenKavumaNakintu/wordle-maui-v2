namespace WordleGame
{

    [QueryProperty(nameof(PlayerName), "playerName")]
    public partial class SavingsPage : ContentPage
    {
        private HistoryViewModel historyViewModel;
        public string PlayerName { get; set; }

        Color LightBackgroundColor = Color.FromRgb(255, 255, 255);
        Color DarkBackgroundColor = Color.FromRgb(0, 0, 0);
        public SavingsPage()
        {
            InitializeComponent();

            // Initialize the HistoryViewModel
            historyViewModel = new HistoryViewModel();

            // Bind the ViewModel to the page
            BindingContext = historyViewModel;

            bool isDarkTheme = Preferences.Get("IsDarkTheme", true);
            this.BackgroundColor = isDarkTheme ? LightBackgroundColor : DarkBackgroundColor;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Verify that the PlayerName is correctly passed
            System.Diagnostics.Debug.WriteLine($"PlayerName in SavingsPage: {PlayerName}");
            // Log the player name

            historyViewModel.LoadPlayerHistory(PlayerName);  // Load the history

            // Log the count of history items after loading
            Console.WriteLine($"Loaded {historyViewModel.playerHistories.Count} history items.");
            System.Diagnostics.Debug.WriteLine($"Loaded {historyViewModel.playerHistories.Count} history items.");

            // Check if data is being populated
            System.Diagnostics.Debug.WriteLine($"Number of histories: {historyViewModel.playerHistories.Count}");
        }

        private async void backButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"gamePage?playerName={PlayerName}");

        }
    }
}

