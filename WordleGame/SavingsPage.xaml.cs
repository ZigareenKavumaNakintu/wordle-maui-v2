
namespace WordleGame
{

    [QueryProperty(nameof(PlayerName), "playerName")]
    public partial class SavingsPage : ContentPage
    {
        private HistoryViewModel historyViewModel;
        public string PlayerName { get; set; }

       private readonly Color LightBackgroundColor = Color.FromRgb(255, 255, 255);
       private readonly  Color DarkBackgroundColor = Color.FromRgb(0, 0, 0);
       private readonly Color LightTextColor = Color.FromRgb(0, 0, 0);
       private readonly Color DarkTextColor = Color.FromRgb(255, 255, 255);
       
        public SavingsPage()
        {
            InitializeComponent();

            // Initialize the HistoryViewModel
            historyViewModel = new HistoryViewModel();

            // Bind the ViewModel to the page
            BindingContext = historyViewModel;

            
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
        /*
        private void setBackgroundColor()
        {
            if (_viewModel.IsDarkTheme == true)
            {
                mainPage.BackgroundColor = DarkBackgroundColor;

                if (historyContent.Children is Label label)
                {
                    label.TextColor = DarkTextColor;


                }
            }
            //if darkmode is turned on
            else if (_viewModel.IsDarkTheme == false)
            {
                mainPage.BackgroundColor = LightBackgroundColor;


                
                    if (historyContent.Children is Label label)
                    {
                        label.TextColor = LightTextColor;


                    }
                
            }

        }*/
        private async void backButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"gamePage?playerName={PlayerName}");

        }
    }
}

