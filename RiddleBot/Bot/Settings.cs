using System;
using System.Collections.Generic;
using System.Text;

namespace RiddleBot
{
    public static class Settings
    {
        public static string[] playersName
        {   get
            {
                return playersName[index];
            }
            set
            {

            }
        }
        public static int myId { get; set; }
        public static int timeBank { get; set; }
        public static int timePerMove { get; set; }
        public static int fieldWidth { get; set; }
        public static int fieldHeight { get; set; }
        public static int maxRounds { get; set; }
    }
}
