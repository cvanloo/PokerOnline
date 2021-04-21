using System;
using System.Collections.Generic;
using System.Text;

namespace PokerOnline.Models
{
    public class Deck
    {
        private Stack<Card> cards;

        /// <summary>
        /// Constructor
        /// Initializes a 52 card deck.
        /// </summary>
        public Deck()
        {
            Reset();
        }

        /// <summary>
        /// Creates a standart 52 card deck.
        /// </summary>
        /// <returns>The 52 cards in an array.</returns>
        private Card[] CreateCardDeck()
        {
            // 4 suits, 13 values per suit = 52 cards

            Card[] cards = new Card[52];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    cards[i * 13 + j] = new Card((Card.Suit)i, (Card.Value)j);
                }
            }

            return cards;
        }

        /// <summary>
        /// Randomly shuffles the cards in the array.
        /// Time complexity is O(n).
        /// </summary>
        public void ShuffleCards()
        {
            Card[] cards = this.cards.ToArray();
            Random rand = new Random();

            // Fisher-Yates suffle algorithm
            for (int i = 0; i < cards.Length; i++)
            {
                int j = rand.Next(0, cards.Length);

                // Swap cards
                Card swap = cards[i];
                cards[i] = cards[j];
                cards[j] = swap;
            }

            this.cards = new Stack<Card>(cards);
        }

        ///// <summary>
        ///// Gives `count` number of cards to each player
        ///// </summary>
        ///// <param name="count">Number of cards to give to each player</param>
        //public void DealCards(ICollection<Player> players, int count = 1)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        foreach (Player player in players)
        //        {
        //            player.AddCard(cards.Pop());
        //        }
        //    }
        //}

        /// <summary>
        /// Removes `count` number of cards from the stack (deck).
        /// </summary>
        /// <param name="count">Number of cards to remove</param>
        /// <returns>Removed cards.</returns>
        public Card[] Deal(int count = 1)
        {
            if (this.cards.Count < count) count = this.cards.Count;

            Card[] cards = new Card[count];

            for (int i = 0; i < count; i++)
            {
                cards[i] = this.cards.Pop();
            }

            return cards;
        }

        /// <summary>
        /// Resets the card deck.
        /// </summary>
        public void Reset()
        {
            cards = new Stack<Card>(CreateCardDeck());
        }
    }
}
