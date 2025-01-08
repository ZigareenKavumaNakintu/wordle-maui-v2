
//using WordleGame.ViewModels;

namespace WordleGame;
[QueryProperty(nameof(PlayerName), "playerName")]
public partial class gamePage : ContentPage
{
    private int cols = 5;
    private int rows = 6;
    private int guess = 0;
    // bool isWin = true;
    private string _playerName;
    private GameViewModel viewModel;
    private HistoryViewModel historyViewModel;
    private readonly Color LightBackgroundColor = Color.FromRgb(255, 255, 255);
    private readonly Color DarkBackgroundColor = Color.FromRgb(0, 0, 0);
    private readonly Color LightTextColor = Color.FromRgb(0, 0, 0);
    private readonly Color DarkTextColor = Color.FromRgb(255, 255, 255);


    public string PlayerName
    {
        get => _playerName;

        set
        {
            _playerName = value;
            OnPropertyChanged(nameof(PlayerName));
            playerMessage();
        }
    }
    public gamePage()
    {
        InitializeComponent();
        viewModel = new GameViewModel();
        historyViewModel = new HistoryViewModel();
        BindingContext = viewModel;

        if (!string.IsNullOrEmpty(PlayerName))
        {
            System.Diagnostics.Debug.WriteLine($"PlayerName: {PlayerName}");
            playerMessage();  // This will display the player's name
        }

        // playerName = name;
        // InitializeViewModel();
        historyViewModel.LoadPlayerHistory(PlayerName);
        setBackgroundColor();
        SetLabelTextColor();
    }

    private void setBackgroundColor()
    {
        if(viewModel.IsDarkTheme == true)
        {
            mainPage.BackgroundColor = DarkBackgroundColor;
            pageMessage.TextColor = DarkTextColor;

            foreach (var child in GridContent.Children)
            {
                if (child is Frame frame)
                {
                    // Set frame background color
                    frame.BackgroundColor = Colors.White;

                    // Check if the content of the frame is an Entry, and apply the theme
                    if (frame.Content is Entry entry)
                    {
                        entry.TextColor = DarkTextColor; // Set text color for Entry
                        entry.BackgroundColor = Colors.White; // Set background color for Entry
                    }
                }
                else if (child is Label label)
                {
                    // Apply text color to labels
                    label.TextColor = DarkTextColor;
                }
                else if (child is Button button)
                {
                    // Apply text color to buttons
                    button.TextColor = DarkTextColor;
                }
            }
        }
        //if darkmode is turned on
        else if (viewModel.IsDarkTheme == false)
        {
            mainPage.BackgroundColor = LightBackgroundColor;
            pageMessage.TextColor = LightTextColor;

            foreach (var child in GridContent.Children)
            {
                if (child is Frame frame)
                {
                    // Set frame background color
                    frame.BackgroundColor = Colors.White;

                    // Check if the content of the frame is an Entry, and apply the theme
                    if (frame.Content is Entry entry)
                    {
                        entry.TextColor = LightTextColor; // Set text color for Entry
                        entry.BackgroundColor = Colors.White; // Set background color for Entry
                    }
                }
                else if (child is Label label)
                {
                    // Apply text color to labels
                    label.TextColor = LightTextColor;
                }
                else if (child is Button button)
                {
                    // Apply text color to buttons
                    button.TextColor = LightTextColor;
                }
            }
        }

    }

    private void playerMessage()
    {
        if (!string.IsNullOrEmpty(PlayerName))
        {
           pageMessage.Text = $"Welcome, {PlayerName}!";
        }
    }


    protected override async void OnAppearing()
    {

        base.OnAppearing();
       // DisplayAlert("Welcome", $"Hello, {playerName}! Let's play Wordle!", "OK");
        await viewModel.MakeWordList();
        viewModel.StartTimerCommand.Execute(null); //start the timer
        makeGrid();
    }
    private void makeGrid()
    {

        //clear the grid at the beginning
        GridContent.RowDefinitions.Clear();
        GridContent.ColumnDefinitions.Clear();
        GridContent.Children.Clear();

        for (int i = 0; i < rows; i++)
        {
            GridContent.AddRowDefinition(new RowDefinition { Height = GridLength.Auto });

        }
        for (int j = 0; j < cols; j++)
        {
            GridContent.AddColumnDefinition(new ColumnDefinition { Width = GridLength.Star });
        }

        //add entries to the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {

                var letterEntry = new Entry
                {
                    //Placeholder = $"Row {i + 1}, Col {j + 1}",
                    BackgroundColor = Colors.LightBlue,
                    TextColor = Colors.Black,
                    
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center

                };

                letterEntry.TextChanged += (sender, args) => OnEntryTextChanged(sender, args, i, j);
                var entryFrame = new Frame
                {
                    Content = letterEntry,
                    Padding = 0,
                    CornerRadius = 8,
                    BorderColor = Colors.Black,
                    BackgroundColor = Colors.Transparent,
                    HasShadow = false,
                    Margin = new Thickness(5)
                };

                //add the content to the grid
                //GridContent.Add(letterEntry,i,j);
                GridContent.Children.Add(entryFrame);
                Grid.SetRow(entryFrame, i);
                Grid.SetColumn(entryFrame, j);

               
            }
        }

    }
    // method is faulty
    //method to automatically move to the next entry in a row to input a letter
    private  async void OnEntryTextChanged(object? sender, TextChangedEventArgs args, int row, int col)
    {
        // Check if sender is null or not of type Entry
        if (sender is not Entry currentEntry)
        {
            System.Diagnostics.Debug.WriteLine("Sender is null or not an Entry.");
            return;
        }

        // Move to the next column only if a single character is entered
        if (!string.IsNullOrEmpty(args.NewTextValue) && args.NewTextValue.Length == 1)
        {
            var nextColumn = col + 1;

            // Check if nextColumn is within bounds
            if (nextColumn < cols)
            {
                var nextFrame = getPosition(row, nextColumn);
                var nextEntry = nextFrame?.Content as Entry;
                if (nextEntry != null)
                {
                    // Add a slight delay to ensure smooth focus transition
                    await Task.Delay(100);
                    nextEntry.Focus();
                }
            }
            /*
            else
            {
                // Optionally handle focus when reaching the last column of the row
                System.Diagnostics.Debug.WriteLine("End of the row reached.");
            }*/
        }
        else if (string.IsNullOrEmpty(args.NewTextValue))
        {
            // If the entry is cleared, focus remains on the current Entry
            await Task.Delay(100);
            currentEntry.Focus();
        }
    }
    

    //method to make the letters into a string and form a word meant to be a guess
    private async Task<string> formWord(int rowNumber)
    {
        string inputWord = "";
        bool isLetterCorrect = true;

        //iterate through the grid and check through the row and get the word with datachecking
        foreach (var frame in GridContent.Children.OfType<Frame>())
        {
            if (Grid.GetRow(frame) == rowNumber)
            {
                var entry = frame.Content as Entry;

                if (entry != null)
                {
                    string input = entry.Text?.Trim() ?? "";

                    if (string.IsNullOrEmpty(input) || input.Length > 1)
                    {
                        isLetterCorrect = false;

                    }
                    //if the input is a number
                    else if (char.IsDigit(input[0]))
                    {
                        isLetterCorrect = false;

                    }
                    else
                    {
                        //change all the letter to lower case incase the input is uppercase
                        inputWord += input.ToLower();

                    }

                }
            }
        }
        if (!isLetterCorrect)
        {
            await Shell.Current.DisplayAlert("Invalid Input", "Please enter only letters.", "OK");
            return string.Empty;
        }

        //ifthe word is equal to 5 return the word that has been input
        if (isLetterCorrect && inputWord.Length == 5)
        {
            return inputWord;
        }

        else
        {
            await Shell.Current.DisplayAlert("Invalid Input", "Please enter 5 letters only.", "OK");
            return string.Empty;
        }

    }
    //method to get the current position of the entry in the frame
    private Frame getPosition(int row, int col)
    {
        foreach (var child in GridContent.Children) {

            if (child is Frame frame && Grid.GetRow(frame) == row && Grid.GetColumn(frame) == col)
            {

                return frame;
            }
        }
        return null;
    }
    //method to compare the wrord and logic with the colours
    private async Task CompareWords(int rowNumber)
    {
        string playerGuess = await formWord(rowNumber);

        if (string.IsNullOrEmpty(playerGuess))
        {
            return;
        }

        string correctWord = viewModel.currentWord.ToLower();
        int greenCount = 0;
        //iterate through the 5 columns and check if every latter corresponds with the letter in the correct words and apply the logic
        for (int col = 0; col < cols; col++)
        {
            var gameFrame = getPosition(rowNumber, col);
            var letterEntry = gameFrame?.Content as Entry;

            if (letterEntry != null) {
                //make sure the letter is set to lower case incase of invalid userinput
                string inputLetter = letterEntry.Text?.Trim().ToLower();

                if (!string.IsNullOrEmpty(inputLetter) && inputLetter.Length == 1)
                {
                    {
                        //System.Diagnostics.Debug.WriteLine($"Correct Word Letter: {correctWord[col]}");
                        if (inputLetter == correctWord[col].ToString())
                        {
                            letterEntry.BackgroundColor = Colors.Green;
                            gameFrame.BackgroundColor = Colors.Green;
                            greenCount++;//keeps track of the entries that change to green
                        }

                        else if (correctWord.Contains(inputLetter))
                        {
                            letterEntry.BackgroundColor = Colors.Yellow;
                            gameFrame.BackgroundColor = Colors.Yellow;
                        }

                        else
                        {
                            letterEntry.BackgroundColor = Colors.DarkGrey;
                            gameFrame.BackgroundColor = Colors.DarkGrey;
                        }
                    }
                }

            }
        }
        //if all the letters are correct then end the bgame and the player wins
        if(greenCount == cols)
        {
            await EndGame(rowNumber,true);
        }
      
    }


    private async void Submit_Clicked(object sender, EventArgs e)
    {
        await CompareWords(guess);
      //  await EndGame(guess);
       
        //makes the player input on the next row
        if (guess < rows - 1)
        {
            guess++;
            var newFrame = getPosition(guess, 0);
            var nextEntry = newFrame?.Content as Entry;
            nextEntry?.Focus();
        }

        //if the player has finished all their guesses end the game
        else if(guess >= rows - 1)
        {
            try
            {
                await EndGame(rows, false);
            }
            catch (Exception ex)
            {
               await Shell.Current.DisplayAlert("Error in EndGame:", ex.Message,"OK");

            }
        }
    }

    //method to end the game
    private async Task EndGame(int rowNumber, bool isWin)
    {
        //stop the timer(toggle the state of the timer)
        viewModel.StartTimerCommand.Execute(null);

        SaveHistory(viewModel.currentWord, rowNumber + 1, isWin);
        if (isWin)
        {
            await Shell.Current.DisplayAlert("Game Over", $"Well Done. You took {rowNumber + 1} guesses", "ok");

        }
        else
        {
            await Shell.Current.DisplayAlert("Game Over" ,$"Bad Luck. The word was {viewModel.currentWord}", "OK");

        }
      

        foreach (var frame in GridContent.Children.OfType<Frame>()) {
            var input = frame.Content as Entry;

            if (input != null)
            {
                //stop the entries into the next row after ending game
                input.IsEnabled = false;
            }
        }

        //disable the button
        Submit.IsEnabled = false;

    }

    public void SaveHistory(string correctWord, int guesses, bool isWin)
    {
        //get the time taken by the player from the viewmodel
        int timePlayed = viewModel.Count;

       // string playerName = _playerName;

        var gameHistory = new PlayerHistory
        {
            CorrectWord = correctWord,
            Attempts = guesses,
            TimeTaken = timePlayed,
            State = isWin,
            PlayerName = PlayerName

        };

        historyViewModel.writePlayerHistory(PlayerName, gameHistory);
       // saveGame(gameHistory);


    }
    /*
    private void saveGame(PlayerHistory playerHistory)
    {
        var historyViewModel = new HistoryViewModel();

       // historyViewModel.Load
    }
    */
    private void ResetGame()
    {
        guess = 0;
        //greenCount = 0;

        //clear the grid and start the game again
        foreach (var frame in GridContent.Children.OfType<Frame>())
        {
            var input = frame.Content as Entry;

            if (input != null)
            {
               
                input.IsEnabled = true;
                input.Text = string.Empty;
                input.BackgroundColor = Colors.Transparent;
                ((Frame)frame).BackgroundColor = Colors.Transparent;
            }
        }

        viewModel.MakeWordList();
        makeGrid();
        viewModel.ResetTimer();
        //re-enable the button
        Submit.IsEnabled = true;
      
    }

    //restart the game
    private void Restart_Clicked(object sender, EventArgs e)
    {
        ResetGame();
        viewModel.ResetTimer();
        viewModel.StartTimerCommand.Execute(null); //start the timer
    }

    
    //Method to navigate to the history page
    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"Navigating to SavingsPage with playerName: {PlayerName}");
        await Shell.Current.GoToAsync($"SavingsPage?playerName={PlayerName}");
    }

    private  async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"Settings?playerName={PlayerName}");
    }

    private void SetLabelTextColor()
    {
        if (viewModel.IsDarkTheme)
        {
            // Set text color to dark if in dark mode
            TimerDisplay.TextColor = Colors.White;
        }
        else
        {
            // Set text color to light if in light mode
            TimerDisplay.TextColor = Colors.Black;
        }
    }
}


