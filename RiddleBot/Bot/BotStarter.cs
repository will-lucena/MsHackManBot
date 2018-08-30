using System;
using System.Collections.Generic;

namespace RiddleBot
{
    class BotStarter
    {
        private Random random;

        public BotStarter()
        {
            this.random = new Random();
        }

        /**
         * Return a random character to play as
         * @return A random character
         */
        public CharacterType getCharacter()
        {
            Array characters = Enum.GetValues(typeof(CharacterType));
            return (CharacterType)characters.GetValue(this.random.Next(characters.Length));
        }

        /**
         * Does a move action. Edit this to make your bot smarter.
         * @param state The current state of the game
         * @return A Move object
         */
        public Move doMove(BotState state)
        {
            List<MoveType> validMoveTypes = state.getField().getValidMoveTypes();

            if (validMoveTypes.Count <= 0)
            {
                return new Move(); // No valid moves, pass
            }

            //use IA here
            return PathFinding.getMove(state.getField());
        }

        public static void Main(string[] args)
        {
            BotParser parser = new BotParser();
            parser.run();
        }
    }
}
