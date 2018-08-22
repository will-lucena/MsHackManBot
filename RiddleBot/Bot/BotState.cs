﻿using System;
using System.Collections.Generic;

namespace RiddleBot
{
    public class BotState
    {
        private int MAX_TIMEBANK;
        private int TIME_PER_MOVE;
        private int MAX_ROUNDS;

        private int roundNumber;
        private int timebank;
        private string myName;
        private Dictionary<string, Player> players;

        private Field field;

        public BotState()
        {
            this.field = new Field();
            this.players = new Dictionary<string, Player>();
        }

        public void setTimebank(int value)
        {
            this.timebank = value;
        }

        public void setMaxTimebank(int value)
        {
            this.MAX_TIMEBANK = value;
        }

        public void setTimePerMove(int value)
        {
            this.TIME_PER_MOVE = value;
        }

        public void setMyName(string myName)
        {
            this.myName = myName;
        }

        public void setMaxRounds(int value)
        {
            this.MAX_ROUNDS = value;
        }

        public void setRoundNumber(int roundNumber)
        {
            this.roundNumber = roundNumber;
        }

        public int getTimebank()
        {
            return this.timebank;
        }

        public int getRoundNumber()
        {
            return this.roundNumber;
        }

        public Dictionary<string, Player> getPlayers()
        {
            return this.players;
        }

        public Field getField()
        {
            return this.field;
        }

        public string getMyName()
        {
            return this.myName;
        }

        public int getMaxTimebank()
        {
            return this.MAX_TIMEBANK;
        }

        public int getTimePerMove()
        {
            return this.TIME_PER_MOVE;
        }

        public int getMaxRound()
        {
            return this.MAX_ROUNDS;
        }
    }
}
