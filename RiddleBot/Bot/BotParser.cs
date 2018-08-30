using System;
using System.IO;

namespace RiddleBot
{
    public class BotParser
    {
        private BotStarter bot;
        private BotState currentState;

        public BotParser()
        {
            Console.SetIn(new StreamReader((Console.OpenStandardInput(512))));
            this.bot = new BotStarter();
            this.currentState = new BotState();
        }

        /**
         *
         * Run will keep reading output from the engine.
         * Will either update the bot state or get actions.
         */
        public void run()
        {
            while (true)
            {
                string line = Console.ReadLine();

                if (line.Length == 0) continue;

                string[] parts = line.Split(' ');
                switch (parts[0])
                {
                    case "settings":
                        parseSettings(parts[1], parts[2]);
                        break;
                    case "update":
                        if (parts[1].Equals("game"))
                        {
                            parseGameData(parts[2], parts[3]);
                        }
                        else
                        {
                            parsePlayerData(parts[1], parts[2], parts[3]);
                        }
                        break;
                    case "action":
                        if (parts[1].Equals("character"))
                        {  // return character
                            Console.WriteLine(this.bot.getCharacter().ToString().ToLower());
                        }
                        else if (parts[1].Equals("move"))
                        {  // return move
                            Move move = this.bot.doMove(this.currentState);
                            if (move != null)
                            {
                                Console.WriteLine(move.ToString());
                            }
                            else
                            {
                                Console.WriteLine("no_moves");
                            }
                        }
                        break;
                    default:
                        Console.Error.WriteLine("Unknown command");
                        break;
                }
            }
        }

        /**
         * Parses all the game settings given by the engine
         * @param key Type of setting given
         * @param value Value
         */
        private void parseSettings(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case "timebank":
                        int time = int.Parse(value);
                        this.currentState.setMaxTimebank(time);
                        this.currentState.setTimebank(time);
                        break;
                    case "time_per_move":
                        this.currentState.setTimePerMove(int.Parse(value));
                        break;
                    case "player_names":
                        string[] playerNames = value.Split(',');
                        foreach (string playerName in playerNames)
                        {
                            this.currentState.getPlayers().Add(playerName, new Player(playerName));
                        }
                        break;
                    case "your_bot":
                        this.currentState.setMyName(value);
                        break;
                    case "your_botid":
                        int myId = int.Parse(value);
                        int opponentId = 2 - (myId + 1);
                        this.currentState.getField().setMyId(myId);
                        this.currentState.getField().setOpponentId(opponentId);
                        break;
                    case "field_width":
                        this.currentState.getField().setWidth(int.Parse(value));
                        break;
                    case "field_height":
                        this.currentState.getField().setHeight(int.Parse(value));
                        break;
                    case "max_rounds":
                        this.currentState.setMaxRounds(int.Parse(value));
                        break;
                    default:
                        Console.Error.WriteLine(string.Format("Cannot parse settings input with key '%s'", key));
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(string.Format("Cannot parse settings value '%s' for key '%s'", value, key));
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        /**
         * Parse data about the game given by the engine
         * @param key Type of game data given
         * @param value Value
         */
        private void parseGameData(string key, string value)
        {
            try
            {
                switch (key)
                {
                    case "round":
                        this.currentState.setRoundNumber(int.Parse(value));
                        break;
                    case "field":
                        this.currentState.getField().initField();
                        this.currentState.getField().parseFromString(value);
                        break;
                    default:
                        Console.Error.WriteLine(string.Format("Cannot parse game data input with key '%s'", key));
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(string.Format("Cannot parse game data value '%s' for key '%s'", value, key));
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        /**
         * Parse data about given player that the engine has sent
         * @param playerName Player name that this data is about
         * @param key Type of player data given
         * @param value Value
         */
        private void parsePlayerData(string playerName, string key, string value)
        {
            Player player = this.currentState.getPlayers()[playerName];

            if (player == null)
            {
                Console.Error.WriteLine(string.Format("Could not find player with name %s", playerName));
                return;
            }

            try
            {
                switch (key)
                {
                    case "bombs":
                        player.bombs = int.Parse(value);
                        break;
                    case "snippets":
                        player.snippets = int.Parse(value);
                        break;
                    default:
                        Console.Error.WriteLine(string.Format("Cannot parse %s data input with key '%s'", playerName, key));
                        break;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(string.Format("Cannot parse %s data value '%s' for key '%s'", playerName, value, key));
                Console.Error.WriteLine(e.StackTrace);
            }
        }
    }
}
