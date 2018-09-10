namespace RiddleBot
{
    public class Move
    {
        public MoveType moveType { get; set; }
        private int bombTicks;

        public Move()
        {
            moveType = MoveType.PASS;
        }

        public Move(MoveType moveType)
        {
            this.moveType = moveType;
            this.bombTicks = -1;
        }

        public Move(MoveType moveType, int bombTicks)
        {
            this.moveType = moveType;
            this.bombTicks = bombTicks;
        }

        public override string ToString()
        {
            if (this.moveType == MoveType.PASS || this.bombTicks < 0)
            {
                return this.moveType.ToString();
            }

            return string.Format("%s;drop_bomb %d", this.moveType, this.bombTicks);
        }
    }
}
