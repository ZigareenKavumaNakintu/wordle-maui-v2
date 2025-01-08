using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WordleGame
{
    //viewmodel that will store the game status in files for each player
    public class HistoryViewModel: INotifyPropertyChanged
    {
        private GameViewModel _viewModel;
        public ObservableCollection<PlayerHistory> _playerHistories { get; set; }

        public ObservableCollection<PlayerHistory> playerHistories
        {
            get => _playerHistories;
            set
            {
                _playerHistories = value;
               
                OnPropertyChanged(nameof(playerHistories));  // Notifies the UI that the collection has changed
            }
        }
        public HistoryViewModel() {
            playerHistories = new ObservableCollection<PlayerHistory>();

        }

        //method that saves the game status of the particular player
        public void writePlayerHistory(string playerName, PlayerHistory playerHistory)
        {
            try
            {
                //store the status to each player in the file
                string fileName = $"{playerName}_history.json";
                string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

                // Debugging the file path and data being saved
                System.Diagnostics.Debug.WriteLine($"Saving history for player: {playerName}");
                System.Diagnostics.Debug.WriteLine($"Target file path: {targetFile}");
                System.Diagnostics.Debug.WriteLine($"Player History: {JsonSerializer.Serialize(playerHistory)}");

                // If the player already has history, load it
                List<PlayerHistory> historyList = new();
                if (File.Exists(targetFile))
                {
                    string existingData = File.ReadAllText(targetFile);
                    historyList = JsonSerializer.Deserialize<List<PlayerHistory>>(existingData) ?? new List<PlayerHistory>();
                }
                //add it to the historyList
                historyList.Add(playerHistory);

                //save and write  the data to a json file 
                string jsonlist = JsonSerializer.Serialize(historyList);
                File.WriteAllText(targetFile, jsonlist);
                

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving player history: {ex.Message}");
            }

        }

        //method to load the game status history of the player
        public void LoadPlayerHistory(string playerName)
        {
            try
            {
                string fileName = $"{playerName}_history.json";
                string targetFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, fileName);

                System.Diagnostics.Debug.WriteLine($"Loading history for player {playerName} from {targetFile}");

                if (File.Exists(targetFile))
                {
                    //all the data that is in the file is read and then added to the historyList which then fills the observable collection
                    string existingData = File.ReadAllText(targetFile);
                    var historyList = JsonSerializer.Deserialize<List<PlayerHistory>>(existingData) ?? new List<PlayerHistory>();
                    playerHistories = new ObservableCollection<PlayerHistory>(historyList);

                    
                    foreach (var history in playerHistories)
                    {
                        System.Diagnostics.Debug.WriteLine($"PlayerName: {history.PlayerName}, TimeTaken: {history.TimeTaken}");
                    }
                }
                else
                {
                  //if the file doesnt exist make a new observable collection
                    playerHistories = new ObservableCollection<PlayerHistory>();
                }
            }
            catch (JsonException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading player history: {ex.Message}");
                playerHistories = new ObservableCollection<PlayerHistory>();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
