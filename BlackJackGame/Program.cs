namespace BlackjackGame
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Blackjack!");
            Console.WriteLine("How many players are playing?");

            if (int.TryParse(Console.ReadLine(), out int playerCount) && playerCount > 0)
            {
                Console.WriteLine("And what is the initial balance of the players?");
                if (int.TryParse(Console.ReadLine(), out int initialBalance) && initialBalance > 0)
                {
                    GameManager gm = new GameManager(playerCount, initialBalance);
                    gm.Start();

                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid positive integer for the initial balance.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid positive integer for the number of players.");
            }


        }
        //TODO : Add win condition and replay option
    }
}
