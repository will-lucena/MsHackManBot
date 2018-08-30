using System;
using System.Collections.Generic;
using System.Text;

namespace RiddleBot
{
    public class IA
    {
        private static Point oldTarget = null;

        public static Move getMove(List<Point> snippetPositions, List<MoveType> validMoveTypes, Point myPosition)
        {
            //already have target
            if (snippetPositions.Contains(oldTarget))
            {
                return chaseTarget(myPosition, validMoveTypes);
            }
            else if (!(snippetPositions.Count == 0))
            {
                oldTarget = snippetPositions[0];
                return chaseTarget(myPosition, validMoveTypes);
            }
            return new Move(MoveType.PASS);
        }

        private static Move chaseTarget(Point myPosition, List<MoveType> validMoveTypes)
        {
            if (oldTarget.y < myPosition.y && validMoveTypes.Contains(MoveType.UP))
            {
                return new Move(MoveType.UP);
            }
            if (oldTarget.y > myPosition.y && validMoveTypes.Contains(MoveType.DOWN))
            {
                return new Move(MoveType.DOWN);
            }
            if (oldTarget.x > myPosition.x && validMoveTypes.Contains(MoveType.RIGHT))
            {
                return new Move(MoveType.RIGHT);
            }
            if (oldTarget.x < myPosition.x && validMoveTypes.Contains(MoveType.LEFT))
            {
                return new Move(MoveType.LEFT);
            }
            return new Move(MoveType.PASS);
        }
    }
}
