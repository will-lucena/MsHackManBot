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
            return IA.getMove(state.getField().getSnippetPositions(), state.getField().getValidMoveTypes(), state.getField().getMyPosition());
            /*
            Field field = state.getField();
            Node current = new Node(field.getMyPosition().x, field.getMyPosition().y, 0);
            Node target = new Node(field.getSnippetPositions()[0].x, field.getSnippetPositions()[0].y);

            return AStar.aStarAlgorithm(current, target, field).toMove(current);
            /**/

            /*
            // Get random but valid move type
            MoveType randomMoveType = validMoveTypes[this.random.Next(validMoveTypes.Count)];

            Player me = state.getPlayers()[state.getMyName()];

            if (me.bombs <= 0) {
                return new Move(randomMoveType); // No bombs available
            }

            int bombTicks = this.random.Next(4) + 2; // random number 2 - 5

            return new Move(randomMoveType, bombTicks); // Drop bomb if available
            /**/
        }

        public static void Main(string[] args)
        {
            BotParser parser = new BotParser();
            parser.run();
        }
    }
}
