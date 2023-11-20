namespace BlackjackGame
{
    internal class Hand
    {
        public List<Card> Cards { get; set; }
        public HandStatus handStatus { get; set; }

        public Hand()
        {
            Cards = new List<Card>();
        }
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public int GetSum()
        {
            int sum = 0;
            int aceCount = 0;

            foreach (var card in Cards)
            {
                sum += card.Value;
                if (card.Face.Equals("Ace"))
                {
                    aceCount++;
                }
            }

            while (sum > 21 && aceCount > 0)
            {
                sum -= 10;
                aceCount--;
            }

            return sum;
        }
        public Boolean IsFinished()
        {
            if ((handStatus == HandStatus.Stood) || (handStatus == HandStatus.Busted) || (handStatus == HandStatus.Surrendered) || (handStatus == HandStatus.BlackJack))
            {
                return true;
            }
            else
                return false;
        }

        public void statusStand()
        {
            handStatus = HandStatus.Stood;
        }
        public void statusSurrendered()
        {
            handStatus = HandStatus.Surrendered;
        }
        public void statusBusted()
        {
            handStatus = HandStatus.Busted;
        }

        public override string ToString()
        {
            return string.Join(", ", Cards);
        }
        public void CheckBlackjack()
        {
            if (GetSum() == 21)
            {
                handStatus = HandStatus.BlackJack;
            }
        }

        public void ClearHand()
        {
            Cards.Clear();
            Cards = new List<Card>();
            handStatus = HandStatus.Playing;
        }

    }
}