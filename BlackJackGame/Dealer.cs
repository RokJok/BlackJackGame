namespace BlackjackGame
{
    internal class Dealer
    {
        public Hand DealerHand { get; private set; }
        public DealerStatus status { get; private set; }

        public Dealer()
        {
            DealerHand = new Hand();
            status = DealerStatus.Playing;
        }

        public void AddCard(Card card)
        {
            DealerHand.AddCard(card);
        }

        public bool ShouldHit()
        {
            return DealerHand.GetSum() < 17;
        }

        public bool IsBusted()
        {
            return DealerHand.GetSum() > 21;
        }

        public void ResetDealer()
        {
            DealerHand.ClearHand();
        }

        public void SetBusted()
        {
            status = DealerStatus.Busted;
        }


    }
}