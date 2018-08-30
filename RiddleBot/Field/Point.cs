namespace RiddleBot
{
    public class Point
    {
        public int x { get; }
        public int y { get; }
        public int currentCost { get; set; }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Point(int x, int y, int cost)
        {
            this.x = x;
            this.y = y;
            this.currentCost = cost;
        }
    }
}
