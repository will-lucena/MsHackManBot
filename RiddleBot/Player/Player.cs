namespace RiddleBot
{
    public class Player
    {
        public string name { get; }
        public int bombs { get; set; }
        public int snippets { get; set; }

        public Player(string playerName)
        {
            this.name = playerName;
        }
    }
}
