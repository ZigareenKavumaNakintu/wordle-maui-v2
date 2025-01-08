using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleGame
{
    //class thet holds the variables to be display in the history page
    public class PlayerHistory
    {
        public int Attempts {  get; set; }
        public string CorrectWord {  get; set; }
        public int TimeTaken { get; set; }
        public bool State { get; set; }
        public string PlayerName {  get; set; }
    }
}
