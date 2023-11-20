
namespace BlackjackGame
{
    internal class GameManager
    {
        private Deck deck;
        private List<Player> playerList;
        private Dealer dealer;

        public GameManager(int playerCount, int initialBalance)
        {
            deck = new Deck();
            dealer = new Dealer();
            deck.Shuffle();

            playerList = new List<Player>();

            for (int i = 0; i < playerCount; i++)
            {
                playerList.Add(new Player(initialBalance));
            }
        }

        public void Start()
        {
            DealInitialCards();
            PlayRound();
        }

        private void PlayRound()
        {
            bool playing = true;
            while (playing)
            {
                PlayerTurns();
                DealerTurn();
                DetermineWinners();
                playing = PlayAgain();
                if (playing)
                {
                    ResetGame();
                    if (playerList.Count > 0)
                        Start();
                    else
                    {
                        playing = false;
                        Console.WriteLine("No players left. Game over!");
                    }
                }
            }
        }

        private void ResetGame()
        {
            deck = new Deck();
            deck.Shuffle();

            foreach (Player player in playerList)
            {
                if (player.Balance > 0)
                    player.ResetPlayer();
                else
                {
                    playerList.Remove(player);
                    if (playerList.Count == 0)
                        break;
                }
            }

            dealer.ResetDealer();
        }
        private bool PlayAgain()
        {
            Console.WriteLine("Do you want to play again? (Y/N)");
            string answer = Console.ReadLine();
            if (answer == "Y")
            {
                Console.Clear();
                return true;
            }
            else
                Console.WriteLine("Bye!");
            return false;
        }

        private void PlayerTurns()
        {
            foreach (Player player in playerList)
            {
                HandActions(player);
            }
        }

        private void HandActions(Player player)
        {
            foreach (Hand hand in player.PlayerHands)
            {
                while (!hand.IsFinished())
                {
                    Console.Clear();
                    DisplayPlayers();
                    DisplayDealer(false, dealer.DealerHand);
                    Console.WriteLine();
                    DisplayTurn(player);
                    DisplayHand(hand);
                    DisplayHandTotal(hand);
                    Console.WriteLine();
                    Console.WriteLine("Choose an action: (H)it, (S)tand, (D)ouble down, or (Sp)lit");
                    string action = Console.ReadLine();
                    if (action != null)
                        PerformAction(player, hand, action);

                }
            }
        }

        private void DisplayTurn(Player player)
        {
            Console.WriteLine($"Player {playerList.IndexOf(player) + 1}'s turn");
        }

        private void DisplayDealer(bool showFullHand, Hand dealersHand)
        {
            if (showFullHand)
            {
                Console.WriteLine($"Dealer's hand: {dealersHand}");
                Console.WriteLine($"Dealer's total: {dealer.DealerHand.GetSum()}");
            }
            else
                Console.WriteLine($"Dealer's hand: {dealersHand.Cards[0]}, ?");
        }

        private void DisplayHandTotal(Hand hand)
        {
            Console.WriteLine($"Your total: {hand.GetSum()}");
        }

        private void DisplayHand(Hand hand)
        {
            Console.WriteLine($"Your hand: {hand}");
        }
        private void DisplayPlayerBalance(Player player)
        {
            Console.Write($" Balance: {player.Balance}");
        }
        private void DisplayPlayerBet(Player player)
        {
            Console.WriteLine($" Bet: {player.CurrentBet}");
        }
        private void DisplayPlayerStatus(Player player)
        {
            foreach (Hand hand in player.PlayerHands)
            {
                Console.WriteLine($"Hand {player.PlayerHands.IndexOf(hand) + 1} status: {hand.handStatus}");
            }
        }
        private void DisplayPlayers()
        {
            foreach (Player player in playerList)
            {
                Console.WriteLine($"Player {playerList.IndexOf(player) + 1}");
                DisplayPlayerBalance(player);
                DisplayPlayerBet(player);
                DisplayPlayerStatus(player);
            }
        }

        private void PerformAction(Player player, Hand hand, string action)
        {
            switch (action.ToUpper())
            {
                case "H":
                    player.Hit(deck.Draw(), hand);
                    PressKeyToContinue();
                    break;
                case "S":
                    player.Stand(hand);
                    PressKeyToContinue();
                    break;
                case "D":
                    player.DoubleDown(deck.Draw(), hand);
                    PressKeyToContinue();
                    break;
                case "SP":
                    player.Split(hand);
                    PressKeyToContinue();
                    break;
                default:
                    Console.WriteLine("Invalid action. Please choose again.");
                    break;
            }
        }

        private void DealInitialCards()
        {
            foreach (Player player in playerList)
            {
                InitialBet(player);
                player.PlayerHands[0].AddCard(deck.Draw());
                player.PlayerHands[0].AddCard(deck.Draw());
                player.PlayerHands[0].CheckBlackjack();
            }

            dealer.AddCard(deck.Draw());
            dealer.AddCard(deck.Draw());
        }

        private void InitialBet(Player player)
        {
            Console.WriteLine($"How much Player {playerList.IndexOf(player) + 1}  would like to bet?");
            Console.WriteLine($"Your balance: {player.Balance}");
            int amount = int.Parse(Console.ReadLine());
            player.Bet(amount);
        }

        private void DealerTurn()
        {
            while (dealer.ShouldHit())
            {
                Console.Clear();
                DisplayPlayers();
                Console.WriteLine();
                DisplayDealer(true, dealer.DealerHand);
                Card newCard = deck.Draw();
                dealer.AddCard(newCard);
                Console.WriteLine($"Dealer draws: {newCard}");
                PressKeyToContinue();
            }

            if (dealer.IsBusted())
            {
                Console.Clear();
                DisplayDealer(true, dealer.DealerHand);
                Console.WriteLine("Dealer is bust!");
                dealer.SetBusted();
                PressKeyToContinue();
            }
            else
            {
                Console.Clear();
                DisplayPlayers();
                Console.WriteLine();
                DisplayDealer(true, dealer.DealerHand);
                Console.WriteLine($"Dealer stands with a total of {dealer.DealerHand.GetSum()}");
                PressKeyToContinue();
            }
        }

        private void DetermineWinners()
        {
            foreach (Player player in playerList)
            {
                int playerIndex = GetPlayerIndex(player);

                foreach (Hand hand in player.PlayerHands)
                {
                    if (hand.handStatus == HandStatus.Busted)
                    {
                        player.Loose(playerIndex);
                        continue;
                    }

                    if (dealer.status == DealerStatus.BlackJack)
                    {
                        if (hand.handStatus != HandStatus.BlackJack)
                        {
                            player.Loose(playerIndex);
                        }
                        else
                        {
                            player.Tie(playerIndex);
                        }

                        continue;
                    }

                    if (dealer.status == DealerStatus.Busted)
                    {
                        player.Win(playerIndex);
                        continue;
                    }

                    if (hand.handStatus == HandStatus.BlackJack)
                    {
                        player.Win(playerIndex);
                        continue;
                    }

                    if (hand.handStatus == HandStatus.Stood)
                    {
                        int dealerTotal = dealer.DealerHand.GetSum();
                        int handTotal = hand.GetSum();

                        if (handTotal > dealerTotal)
                        {
                            player.Win(playerIndex);
                        }
                        else if (handTotal == dealerTotal)
                        {
                            player.Tie(playerIndex);
                        }
                        else
                        {
                            player.Loose(playerIndex);
                        }
                    }
                }
            }
        }
        private int GetPlayerIndex(Player player)
        {
            return playerList.IndexOf(player) + 1;
        }

        private void PressKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
