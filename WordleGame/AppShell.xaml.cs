namespace WordleGame
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("MainPage",typeof(MainPage));
            Routing.RegisterRoute("gamePage", typeof(gamePage));
            Routing.RegisterRoute("SavingsPage", typeof(SavingsPage));
            Routing.RegisterRoute("Settings", typeof(Settings));
        }
    }
}
