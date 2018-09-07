using System;
using System.IO;

namespace RiddleBot
{
    /* settings input
        settings player_names player0,player1
        settings your_bot player0
        settings timebank 10000
        settings time_per_move 100
        settings your_botid 0
        settings field_width 19
        settings field_height 15
        settings max_rounds 250
     */

    public static class InputParser
    {
        static InputParser()
        {
            Console.SetIn(new StreamReader((Console.OpenStandardInput(512))));
        }

        public static void inputType(string input)
        {
            var line = input.Split(' ');

            switch(line[0])
            {
                case "settings":
                    parseSettings(line[1], line[2]);
                    break;
                case "action":
                    parseAction(line[1], line[2]);
                    break;
                case "update":
                    parseUpdate(line[1], line[2]);
                    break;
            }
        }

        private static void parseSettings(string type, string value)
        {
            switch (type)
            {
                case "player_names":
                    var players = value.Split(',');
                    Settings.player1Name = players[0];
                    Settings.player2Name = players[1];
                    break;
                case "timebank":
                    Settings.timeBank = int.Parse(value);
                    break;
                case "time_per_move":
                    Settings.timeBank = int.Parse(value);
                    break;
                case "your_botid":
                    Settings.myId = int.Parse(value);
                    break;
                case "field_width":
                    Settings.fieldWidth = int.Parse(value);
                    break;
                case "field_height":
                    Settings.fieldHeight = int.Parse(value);
                    break;
                case "max_rounds":
                    Settings.maxRounds = int.Parse(value);
                    break;
                default:
                    break;
            }
        }

        private static void parseAction(string type, string value)
        {
            switch (type)
            {
                case "move":
                    var players = value.Split(',');
                    Settings.player1Name = players[0];
                    Settings.player2Name = players[1];
                    break;
                case "character":
                    Output.print(CharacterType.BIXIETTE.ToString().ToLower());
                    break;
                default:
                    break;
            }
        }

        private static void parseUpdate(string type, string value)
        {
            case "your_botid":
                    Settings.myId = int.Parse(value);
            break;
                case "field_width":
                    Settings.fieldWidth = int.Parse(value);
            break;
                case "field_height":
                    Settings.fieldHeight = int.Parse(value);
            break;
                case "max_rounds":
                    Settings.maxRounds = int.Parse(value);
            break;
            default:
                    break;
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
