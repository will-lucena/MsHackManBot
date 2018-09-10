using System;
using System.Collections.Generic;
using System.Text;

namespace RiddleBot
{
    public enum MoveQuality
    {
        MAX,
        USEFULL
    }

    #region secondIA attempt

    public class IA
    {
        private static Move lastMovement = null;

        public static Move getMove(List<Point> snippetPositions, List<MoveType> validMoveTypes, Point myPosition)
        {
            if (lastMovement != null)
            {
                removeLastPositionFromValidMove(ref validMoveTypes, myPosition);
            }
            Point target = getShortestSnippet(snippetPositions, myPosition);
            int horizontalDifference = target.x - myPosition.x;
            int verticalDifference = target.y - myPosition.y;
            var move = chaseTarget(myPosition, validMoveTypes, verticalDifference, horizontalDifference);
            lastMovement = move;

            return move;
        }

        private static void removeLastPositionFromValidMove(ref List<MoveType> validMoveTypes, Point currentPosition)
        {
            switch (lastMovement.moveType)
            {
                case MoveType.UP:
                    if (validMoveTypes.Contains(MoveType.DOWN))
                    {
                        validMoveTypes.Remove(MoveType.DOWN);
                    }
                    break;
                case MoveType.RIGHT:
                    if (validMoveTypes.Contains(MoveType.LEFT))
                    {
                        validMoveTypes.Remove(MoveType.LEFT);
                    }
                    break;
                case MoveType.DOWN:
                    if (validMoveTypes.Contains(MoveType.UP))
                    {
                        validMoveTypes.Remove(MoveType.UP);
                    }
                    break;
                case MoveType.LEFT:
                    if (validMoveTypes.Contains(MoveType.RIGHT))
                    {
                        validMoveTypes.Remove(MoveType.RIGHT);
                    }
                    break;
            }
        }

        private static Point getShortestSnippet(List<Point> snippetPositions, Point myPosition)
        {
            int minDist = int.MaxValue;
            Point currentShortest = null;
            foreach (Point point in snippetPositions)
            {
                var dist = Math.Sqrt(Math.Pow((point.x - myPosition.x), 2) + Math.Pow((point.y - myPosition.y), 2));

                if (dist < minDist)
                {
                    currentShortest = point;
                }
            }
            return currentShortest;
        }

        private static Move chaseTarget(Point myPosition, List<MoveType> validMoveTypes, int verticalDifference, int horizontalDifference)
        {
            Dictionary<MoveQuality, List<Move>> moves = new Dictionary<MoveQuality, List<Move>>();

            moves[MoveQuality.MAX] = new List<Move>();
            moves[MoveQuality.USEFULL] = new List<Move>();

            foreach (MoveType move in validMoveTypes)
            {
                switch (move)
                {
                    case MoveType.UP:
                        if (verticalDifference < 0)
                        {
                            moves[MoveQuality.MAX].Add(new Move(MoveType.UP));
                        }
                        else
                        {
                            moves[MoveQuality.USEFULL].Add(new Move(MoveType.UP));
                        }
                        break;
                    case MoveType.RIGHT:
                        if (horizontalDifference > 0)
                        {
                            moves[MoveQuality.MAX].Add(new Move(MoveType.RIGHT));
                        }
                        else
                        {
                            moves[MoveQuality.USEFULL].Add(new Move(MoveType.RIGHT));
                        }
                        break;
                    case MoveType.DOWN:
                        if (verticalDifference > 0)
                        {
                            moves[MoveQuality.MAX].Add(new Move(MoveType.DOWN));
                        }
                        else
                        {
                            moves[MoveQuality.USEFULL].Add(new Move(MoveType.DOWN));
                        }
                        break;
                    case MoveType.LEFT:
                        if (horizontalDifference < 0)
                        {
                            moves[MoveQuality.MAX].Add(new Move(MoveType.LEFT));
                        }
                        else
                        {
                            moves[MoveQuality.USEFULL].Add(new Move(MoveType.LEFT));
                        }
                        break;
                }
            }

            if (moves[MoveQuality.MAX].Count > 0)
            {
                foreach (Move move in moves[MoveQuality.MAX])
                {
                    switch (move.moveType)
                    {
                        case MoveType.UP:
                            if (verticalDifference < 0)
                            {
                                verticalDifference++;
                                return move;
                            }
                            break;
                        case MoveType.RIGHT:
                            if (horizontalDifference > 0)
                            {
                                horizontalDifference--;
                                return move;
                            }
                            break;
                        case MoveType.DOWN:
                            if (verticalDifference > 0)
                            {
                                verticalDifference--;
                                return move;
                            }
                            break;
                        case MoveType.LEFT:
                            if (horizontalDifference < 0)
                            {
                                horizontalDifference++;
                                return move;
                            }
                            break;
                    }
                }
            }
            else if (moves[MoveQuality.USEFULL].Count > 0)
            {
                foreach (Move move in moves[MoveQuality.USEFULL])
                {
                    switch (move.moveType)
                    {
                        case MoveType.UP:
                            verticalDifference++;
                            return move;
                        case MoveType.RIGHT:
                            horizontalDifference--;
                            return move;
                        case MoveType.DOWN:
                            verticalDifference--;
                            return move;
                        case MoveType.LEFT:
                            horizontalDifference++;
                            return move;
                    }
                }
            }
            return null;
        }
    }


    #endregion

    #region aStarIA attempt
    /*
    public class Node
    {
        public int x { get; set; }
        public int y { get; set; }
        public int cost { get; set; }

        public Node(int x, int y, int currentCost)
        {
            this.x = x;
            this.y = y;
            this.cost = currentCost;
        }

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.cost = 0;
        }

        public Move toMove(Node other)
        {
            if (other.x > this.x && other.y == this.y)
            {
                return new Move(MoveType.LEFT);
            }

            if (other.x < this.x && other.y == this.y)
            {
                return new Move(MoveType.RIGHT);
            }

            if (other.y > this.y && other.x == this.x)
            {
                return new Move(MoveType.DOWN);
            }

            if (other.y < this.y && other.x == this.x)
            {
                return new Move(MoveType.UP);
            }
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            Node other = (Node)obj;
            if (other.x != this.x)
            {
                return false;
            }
            if (other.y != this.y)
            {
                return false;
            }
            return true;
        }
    }

    public class AStar
    {
        public static Node aStarAlgorithm(Node initial, Node target, Field field)
        {
            List<Node> openNodes = new List<Node>();
            List<Node> closeNodes = new List<Node>();

            openNodes.Add(initial);
            Node actual;

            while (openNodes.Count > 0)
            {
                actual = openNodes[0];
                openNodes.RemoveAt(0);
                closeNodes.Add(actual);

                if (actual == target)
                {
                    return actual;
                }
                else
                {
                    List<Node> newNodes = getChilds(actual, openNodes, closeNodes, field);
                    openNodes.AddRange(newNodes);

                    openNodes.Sort((Node a, Node b) => {
                        float aInfluence = p * fitnessFunction(a, target) + (1 - p) * a.cost;
                        float bInfluence = p * fitnessFunction(b, target) + (1 - p) * b.cost;

                        if (aInfluence > bInfluence)
                        {
                            return (int)aInfluence;
                        }
                        return (int)bInfluence;
                    });
                }
            }
            return null;
        }

        const float p = 0.5f; // Value between 0 and 1

        private static float fitnessFunction(Node a, Node target)
        {
            if (a == target)
            {
                return 0;
            }
            return 1;
        }

        private static List<Node> getChilds(Node actual, List<Node> openNodes, List<Node> closedNodes, Field field)
        {
            List<Node> childs = getPosibilities(actual, field);
            for (int i = 0; i < childs.Count; i++)
            {
                for (int k = 0; k < openNodes.Count; k++)
                {
                    if (openNodes[k] == childs[i])
                    {
                        if (childs[i].cost > openNodes[k].cost)
                        {
                            childs.RemoveAt(i);
                        }
                        break;
                    }
                }
            }
            for (int i = 0; i < childs.Count; i++)
            {
                for (int k = 0; k < closedNodes.Count; k++)
                {
                    if (closedNodes[k] == childs[i])
                    {
                        if (childs[i].cost > closedNodes[k].cost)
                        {
                            childs.RemoveAt(i);
                        }
                        break;
                    }

                }
            }
            return childs;
        }

        private static List<Node> getPosibilities(Node actual, Field field)
        {
            List<Node> nodes = new List<Node>();

            List<MoveType> availableMoves = field.getValidMoveTypes(actual.x, actual.y);

            foreach(MoveType move in availableMoves)
            {
                switch (move)
                {
                    case MoveType.UP:
                        nodes.Add(new Node(actual.x, actual.y + 1, actual.cost + 1));
                        break;
                    case MoveType.DOWN:
                        nodes.Add(new Node(actual.x, actual.y - 1, actual.cost + 1));
                        break;
                    case MoveType.RIGHT:
                        nodes.Add(new Node(actual.x + 1, actual.y, actual.cost + 1));
                        break;
                    case MoveType.LEFT:
                        nodes.Add(new Node(actual.x - 1, actual.y, actual.cost + 1));
                        break;
                }
            }

            return nodes;
        }
    }
    /**/
    #endregion

    #region firstIA attempt
    /*
    public static class PathFinding
    {
        private static Field field;
        private static Point currentTarget;
        private static List<Point> path;
        
        public static Move getMove(Field field)
        {
            if (!field.getSnippetPositions().Contains(currentTarget))
            {
                path = updateField(field);
            }
            return makeMove(field.getMyPosition());
        }

        private static Move makeMove(Point currentPosition)
        {
            Point targetPoint = path[path.Count - 1];
            path.RemoveAt(path.Count - 1);

            if (currentPosition.x == targetPoint.x && currentPosition.y < targetPoint.y)
            {
                return new Move(MoveType.DOWN);
            }
            else if (currentPosition.x == targetPoint.x && currentPosition.y > targetPoint.y)
            {
                return new Move(MoveType.UP);
            }
            else if (currentPosition.y == targetPoint.y && currentPosition.x > targetPoint.x)
            {
                return new Move(MoveType.LEFT);
            }
            else if (currentPosition.y == targetPoint.y && currentPosition.x < targetPoint.x)
            {
                return new Move(MoveType.RIGHT);
            }
            return null;
        }

        public static List<Point> updateField(Field field)
        {
            PathFinding.field = field;
            return recalculateInfos();
        }

        private static List<Point> recalculateInfos()
        {
            currentTarget = field.getSnippetPositions()[0];
            return calculateCosts(currentTarget);
        }

        private static List<Point> calculateCosts(Point targetSnippet)
        {
            List<Point> parents = new List<Point>();
            targetSnippet.currentCost = 0;
            parents.Add(targetSnippet);
            int i = 0;
            while (!parents.Contains(field.getMyPosition()))
            {
                parents = calcNineGrid(parents[i], parents);
                i++;
            }
            return parents;
        }

        private static List<Point> calcNineGrid(Point targetSnippet, List<Point> parents)
        {
            for (int i = targetSnippet.x - 1; i < targetSnippet.x + 2; i++)
            {
                if (!isOutOfBound(i, field.getWidth()))
                {
                    for (int j = targetSnippet.y - 1; j < targetSnippet.y + 2; j++)
                    {
                        if (!isOutOfBound(j, field.getHeight()) && field.isPointValid(i, j))
                        {
                            Point point = new Point(i, j, targetSnippet.currentCost + 1);
                            if (!parents.Contains(point))
                            {
                                parents.Add(point);
                            }
                        }
                    }
                }
            }
            return parents;
        }

        private static bool isOutOfBound(int value, int bound)
        {
            return value > bound || value < 0;
        }
    }
    /**/

    #endregion
}