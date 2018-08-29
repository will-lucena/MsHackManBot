using System;
using System.Collections.Generic;

namespace RiddleBot
{
    public class Field
    {
        protected const string EMTPY_FIELD = ".";
        protected const string BLOCKED_FIELD = "x";

        private string myId;
        private string opponentId;
        private int width;
        private int height;

        private string[,] field;
        private Point myPosition;
        private Point opponentPosition;
        private List<Point> enemyPositions;
        private List<Point> snippetPositions;
        private List<Point> bombPositions;
        private List<Point> tickingBombPositions;

        public Field()
        {
            this.enemyPositions = new List<Point>();
            this.snippetPositions = new List<Point>();
            this.bombPositions = new List<Point>();
            this.tickingBombPositions = new List<Point>();
        }

        /**
         * Initializes field
         * @throws Exception: exception
         */
        public void initField()
        {
            try
            {
                this.field = new string[this.width, this.height];
            } catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                throw new Exception("Error: trying to initialize field while field " + "settings have not been parsed yet.");
            }
            clearField();
        }

        /**
         * Clears the field
         */
        public void clearField()
        {
            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    this.field[x, y] = "";
                }
            }

            this.myPosition = null;
            this.opponentPosition = null;
            this.enemyPositions.Clear();
            this.snippetPositions.Clear();
            this.bombPositions.Clear();
            this.tickingBombPositions.Clear();
        }

        /**
         * Parses input string from the engine and stores it in
         * this.field. Also stores several interesting points.
         * @param input String input from the engine
         */
        public void parseFromString(string input)
        {
            clearField();

            string[] cells = input.Split(',');
            int x = 0;
            int y = 0;

            foreach (string cellString in cells)
            {
                this.field[x, y] = cellString;

                foreach (string cellPart in cellString.Split(';'))
                {
                    switch (cellPart[0])
                    {
                        case 'P':
                            parsePlayerCell(cellPart[1], x, y);
                            break;
                        case 'e':
                            // TODO: store spawn points
                            break;
                        case 'E':
                            parseEnemyCell(cellPart[1], x, y);
                            break;
                        case 'B':
                            parseBombCell(cellPart, x, y);
                            break;
                        case 'C':
                            parseSnippetCell(x, y);
                            break;
                    }
                }

                if (++x == this.width)
                {
                    x = 0;
                    y++;
                }
            }
        }

        /**
         * Stores the position of one of the players, given by the id
         * @param id Player ID
         * @param x X-position
         * @param y Y-position
         */
        private void parsePlayerCell(char id, int x, int y)
        {
            if (id == this.myId[0])
            {
                this.myPosition = new Point(x, y);
            }
            else if (id == this.opponentId[0])
            {
                this.opponentPosition = new Point(x, y);
            }
        }

        /**
         * Stores the position of an enemy. The type of enemy AI
         * is also given, but not stored in the starterbot.
         * @param type Type of enemy AI
         * @param x X-position
         * @param y Y-position
         */
        private void parseEnemyCell(char type, int x, int y)
        {
            this.enemyPositions.Add(new Point(x, y));
        }

        /**
         * Stores the position of a bomb that can be collected or is
         * about to explode. The amount of ticks is not stored
         * in this starterbot.
         * @param cell The string that represents a bomb, if only 1 letter it
         *             can be collected, otherwise it will contain a number
         *             2 - 5, that means it's ticking to explode in that amount
         *             of rounds.
         * @param x X-position
         * @param y Y-position
         */
        private void parseBombCell(string cell, int x, int y)
        {
            if (cell.Length <= 1)
            {
                this.bombPositions.Add(new Point(x, y));
            }
            else
            {
                this.tickingBombPositions.Add(new Point(x, y));
            }
        }

        /**
         * Stores the position of a snippet
         * @param x X-position
         * @param y Y-position
         */
        private void parseSnippetCell(int x, int y)
        {
            this.snippetPositions.Add(new Point(x, y));
        }

        /**
         * Return a list of valid moves for my bot, i.e. moves does not bring
         * player outside the field or inside a wall
         * @return A list of valid moves
         */
        public List<MoveType> getValidMoveTypes()
        {
            List<MoveType> validMoveTypes = new List<MoveType>();
            int myX = this.myPosition.x;
            int myY = this.myPosition.y;

            Point up = new Point(myX, myY - 1);
            Point down = new Point(myX, myY + 1);
            Point left = new Point(myX - 1, myY);
            Point right = new Point(myX + 1, myY);

            if (isPointValid(up)) validMoveTypes.Add(MoveType.UP);
            if (isPointValid(down)) validMoveTypes.Add(MoveType.DOWN);
            if (isPointValid(left)) validMoveTypes.Add(MoveType.LEFT);
            if (isPointValid(right)) validMoveTypes.Add(MoveType.RIGHT);

            return validMoveTypes;
        }

        /**
         * Returns whether a point on the field is valid to stand on.
         * @param point Point to test
         * @return True if point is valid to stand on, false otherwise
         */
        public bool isPointValid(Point point)
        {
            int x = point.x;
            int y = point.y;

            return x >= 0 && x < this.width && y >= 0 && y < this.height &&
                    !this.field[x,y].Contains(BLOCKED_FIELD);
        }

        #region getters and setters

        public void setMyId(int id)
        {
            this.myId = id + "";
        }

        public void setOpponentId(int id)
        {
            this.opponentId = id + "";
        }

        public void setWidth(int width)
        {
            this.width = width;
        }

        public void setHeight(int height)
        {
            this.height = height;
        }

        public Point getMyPosition()
        {
            return this.myPosition;
        }

        public Point getOpponentPosition()
        {
            return this.opponentPosition;
        }

        public List<Point> getEnemyPositions()
        {
            return this.enemyPositions;
        }

        public List<Point> getSnippetPositions()
        {
            return this.snippetPositions;
        }

        public List<Point> getBombPositions()
        {
            return this.bombPositions;
        }

        public List<Point> getTickingBombPositions()
        {
            return this.tickingBombPositions;
        }
        #endregion
    }
}
