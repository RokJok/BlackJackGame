namespace BlackjackGame
{
    internal class Player
    {
        public List<Hand> PlayerHands { get; private set; }
        public int Balance { get; private set; }
        public int CurrentBet { get; private set; }

        public Player(int initialBalance)
        {
            PlayerHands = new List<Hand> { new Hand() };
            Balance = initialBalance;
            CurrentBet = 0;
        }

        public void Bet(int amount)
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                CurrentBet += amount;
            }
            else
            {
                throw new InvalidOperationException("Insufficient balance for the bet.");
            }
        }

        public void Win(int PlayerIndex)
        {
            Balance += CurrentBet * 2;
            Console.WriteLine($"Player {PlayerIndex} wins {CurrentBet}!");
            CurrentBet = 0;
        }
        public void Loose(int PlayerIndex)
        {
            Console.WriteLine($"Player {PlayerIndex} looses {CurrentBet}!");
            CurrentBet = 0;
        }
        public void Tie(int PlayerIndex)
        {
            Balance += CurrentBet;
            CurrentBet = 0;
            Console.WriteLine($"Player {PlayerIndex} ties with the dealer.");
        }


        public void Hit(Card card, Hand hand)
        {
                hand.AddCard(card);
                Console.WriteLine($"You drew a {card}");
                if (hand.GetSum() > 21)
                {
                    Bust(hand);
                }
        }

        private void Bust(Hand hand)
        {
            hand.statusBusted();
            if (PlayerHands.Count > 1)
                CurrentBet -= CurrentBet / PlayerHands.Count;
            else
                CurrentBet = 0;
            Console.WriteLine("Busted!");
        }

        public void Stand(Hand hand)
        {
            hand.statusStand();
            Console.WriteLine($"You stand with a total of: {hand.GetSum()}");
        }

        public void Split(Hand hand)
        {
            if (hand.Cards.Count == 2 && hand.Cards[0].Face == hand.Cards[1].Face)
            {
                Bet(CurrentBet);
                Hand newHand = new Hand();
                newHand.AddCard(hand.Cards[1]);
                hand.Cards.RemoveAt(1);
                PlayerHands.Add(newHand);
                Console.WriteLine("You have split your hand.");
            }
            else
            {
                throw new InvalidOperationException("Cannot split this hand.");
            }
        }

        public void DoubleDown(Card card, Hand hand)
        {
            if (CurrentBet <= Balance)
            {
                Balance -= CurrentBet;
                CurrentBet *= 2;
                Hit(card, hand);
                if (hand.handStatus != HandStatus.Busted)
                {
                    Stand(hand);
                    Console.WriteLine($"You doubled down with a total of {hand.GetSum()}.");
                }
            }
            else
            {
                Console.WriteLine("You have infuficient balance to double down.");
            }
        }

        public void Surrender(Hand hand)
        {
            if (PlayerHands.Contains(hand))
            {
                hand.statusSurrendered();
                Balance += CurrentBet / 2;
                Console.WriteLine("You have surrendered. Half of your bet is returned.");
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid hand index.");
            }
        }

        public void ResetPlayer()
        {
            PlayerHands.Clear();
            PlayerHands = new List<Hand> { new Hand() };
        }

    }
}
