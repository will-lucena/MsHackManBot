using System;
using System.Collections.Generic;
using System.Text;

namespace RiddleBot
{
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
}
