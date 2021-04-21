using System.Collections.Generic;

namespace PokerOnline.Models
{
    public class CardHand
    {
        public List<Card> Cards { get; private set; }

        /// <summary>
        /// The worth of a card-hand.
        /// </summary>
        public enum Worth
        {
            FiveNotMatching = 0, // Five not matching cards
            Pair            = 1, // Two cards of the same Value
            TwoPair         = 2, // Two `Pair`s
            ThreeOfAKind    = 3, // Three cards with the same Value
            Straight        = 4, // 5 cards of sequential value (eg. 8, 9, 10, Jack, Queen)
            Flush           = 5, // 5 cards of the same suite (without sequential value)
            FullHouse       = 6, // 3 cards of one value, 2 cards of another value
            StraightFlush   = 7, // 5 cards of the same suite with sequential value
            RoyalFlush      = 8  // 10, Jack, Queen, King and Ace all of the same suite
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CardHand() 
        {
            Cards = new List<Card>();
        }

        /// <summary>
        /// Add a card to the hand.
        /// </summary>
        /// <param name="card">Card to add.</param>
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        /// <summary>
        /// Add a range of cards to the end.
        /// </summary>
        /// <param name="cards"></param>
        public void AddCards(IEnumerable<Card> cards)
        {
            Cards.AddRange(cards);
        }

        /// <summary>
        /// Remove a card from the hand.
        /// </summary>
        /// <param name="card">Card to remove.</param>
        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        /// <summary>
        /// Evaluate the worth of the hand.
        /// </summary>
        /// <returns>Worth of the hand.</returns>
        public Worth EvaluateWorth()
        {
            bool royal, flush, straight, three, full;
            royal = flush = straight = three = full = false;

            int pairs = 0;

            int k;

            // Sort cards
            Cards.Sort();

            // Check for a flush
            k = 0;
            while (k < 4 && Cards[k].GetSuit == Cards[k+1].GetSuit)
                k++;
            if (4 == k)
                flush = true;

            // Check for a straight
            k = 0;
            while (k < 4 && Cards[k].GetValue == Cards[k+1].GetValue-1)
                k++;
            if (4 == k)
                straight = true;

            // Check for threes and fullhouse
            for (int i = 0; i < 3; i++)
            {
                k = i;
                while (k < i+2 && Cards[k].GetValue == Cards[k+1].GetValue)
                    k++;

                if (k == i+2)
                {
                    three = true;
                    
                    if (i == 0)
                    {
                        if (Cards[3].GetValue == Cards[4].GetValue)
                            full = true;
                    }
                    else if (i == 1)
                    {
                        if (Cards[0].GetValue == Cards[4].GetValue)
                            full = true;
                    }
                    else
                    {
                        if (Cards[0].GetValue == Cards[1].GetValue)
                            full = true;
                    }
                }
            }

            if (straight && flush && royal)
                return Worth.RoyalFlush;
            if (straight && flush)
                return Worth.StraightFlush;
            else if (full)
                return Worth.FullHouse;
            else if (flush)
                return Worth.Flush;
            else if (straight)
                return Worth.Straight;
            else if (three)
                return Worth.ThreeOfAKind;

            // Check for pairs
            for (k = 0; k < 4; k++)
            {
                if (Cards[k].GetValue == Cards[k+1].GetValue)
                    pairs++;
            }

            if (2 == pairs)
                return Worth.TwoPair;
            else if (1 == pairs)
                return Worth.Pair;
            else
                return Worth.FiveNotMatching;
        }
    }
}
