using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Diagnostics;

using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WordleGame
{
    public class GameViewModel :INotifyPropertyChanged
    {
        private GetWordList getWords;
        private Random word;
        private string _currentWord;
        private int count;
        private System.Timers.Timer timer;
        private bool isPlaying = false;
        private bool isDarkTheme;
        private double fontSize;
        public ObservableCollection<string> Words { get; set; }
        public Command StartTimerCommand { get; }
        public Command ChangeThemeCommand { get; set; }

        //properties definition
        public string currentWord
        {
            get => _currentWord;

            set
            {
                if (_currentWord != value)
                {
                    _currentWord = value;
                    OnPropertyChanged();
                }
            }

        }
          
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            set
            {
                isPlaying = value;
                OnPropertyChanged(nameof(StartStop));//notify if the state of the timer changes
            }
        }
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(displayTimer));
            }
        }

        public string StartStop
        {
            get
            {
                if (isPlaying)
                    return "Stop Timer";
                else
                    return "Start Timer";
            }
        }

        public bool IsDarkTheme
        {
            get => Preferences.Get("IsDarkTheme", false);
            set
            {
                if(isDarkTheme == value)

                {  return; }
                   
                else if(isDarkTheme != value)
                {
                    isDarkTheme = value;
                    OnPropertyChanged(nameof(IsDarkTheme));
                    Preferences.Set("IsDarkTheme", value);
                }

               // IsDarkTheme = value;

            }
        }

        public double FontSize
        {
            get => fontSize;
            set
            {
                if (fontSize != value)
                {
                    fontSize = value;
                    OnPropertyChanged();
                }


            }
        }
        
        public GameViewModel()
        {
            getWords = new GetWordList();
            word = new Random();
            Words = new ObservableCollection<string>();

            IsDarkTheme = Preferences.Get("IsDarkTheme", IsDarkTheme);
            FontSize = Preferences.Get("FontSize", FontSize);

            //Setting a timer that will measure the amount of time a player takes to finish the game
            timer = new System.Timers.Timer
            {
                Interval = 1000

            };
            timer.Elapsed += (s, e) =>
            {
                ++Count;
            };

            StartTimerCommand = new Command(() =>
            {
                if (isPlaying)
                    timer.Stop();
                else
                    timer.Start();
                IsPlaying = !isPlaying;//toggles the playing state
            });
        }

        
       

        //method to display the time
        public string displayTimer
        {
            get => $"{Count /60:D2}: {Count %60:D2}";
        }
        //method to load words into memory and store them in list
        public async Task MakeWordList()
        {
            await getWords.EnsureWordListExistsAsync();

            
            List<string> wordList = await getWords.LoadWordsAsync();

            foreach (var word in wordList)
            {
                 Words.Add(word);
            }
                 RandomiseWords();
            }

        //method to randomise the words 
        public void RandomiseWords()
        {
            if (Words.Count > 0)
            {
                currentWord = Words[word.Next(Words.Count)];
            }
        }

        public void  ResetTimer()
        {
            Count = 0;
            timer.Stop();
            IsPlaying = false;
        }

        public void SaveChanges()
        {
            Preferences.Set("IsDarkTheme", IsDarkTheme);
            Preferences.Set("FontSize", FontSize);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}






