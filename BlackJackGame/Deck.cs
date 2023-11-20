namespace BlackjackGame
{
    internal class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            Dictionary<string, int> cardValues = new Dictionary<string, int>
            {
                {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6},
                {"7", 7}, {"8", 8}, {"9", 9}, {"10", 10},
                {"Jack", 10}, {"Queen", 10}, {"King", 10}, {"Ace", 11}
            };

            foreach (var suit in suits)
            {
                foreach (var fvalue in cardValues)
                {
                    string face = fvalue.Key;
                    int value = fvalue.Value;
                    cards.Add(new Card(suit, face, value));
                }
            }
        }

        public void Shuffle()
        {
            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        public Card Draw()
        {
            Card card = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return card;
        }


    }

}