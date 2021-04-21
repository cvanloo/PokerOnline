using System;
using System.Collections.Generic;
using System.Text;

namespace PokerOnline.Models
{
    public class Card : IComparable
    {
        /* Member/Fields */

        private Suit suit;
        private Value value;

        /// <summary>
        /// The suit of a card
        /// </summary>
        public enum Suit
        {
            Club,    // ♣
            Diamond, // ♦
            Heart,   // ♥
            Spade   // ♠
        }

        /// <summary>
        /// The value of a card
        /// </summary>
        public enum Value
        {
            Two   = 0,
            Three = 1,
            Four  = 2,
            Five  = 3,
            Six   = 4,
            Seven = 5,
            Eight = 6,
            Nine  = 7,
            Ten   = 8,
            Jack  = 9,
            Queen = 10,
            King  = 11,
            Ace   = 12
        }

        /* Constructors */

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="suit">The suit the card belongs to</param>
        /// <param name="value">The value of the card</param>
        public Card(Suit suit, Value value)
        {
            this.suit = suit;
            this.value = value;
        }

        /* Getter/Setter */

        public Suit GetSuit
        {
            get { return suit; }
        }

        public Value GetValue
        {
            get { return value; }
        }

        /* Methods */

        /// <summary>
        /// Test if a card is equal.
        /// Two cards are equal when the suit and the value are the same.
        /// </summary>
        /// <param name="obj">Card to compare</param>
        /// <returns>True when both card are equal.</returns>
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj is Card)
            {
                Card card = (Card) obj;

                if (this.GetSuit == card.GetSuit && this.GetValue == card.GetValue)
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Compare this card to another card.
        /// </summary>
        /// <param name="obj">The card to compare to.</param>
        /// <returns>
        /// Less than zero: This instance precedes the other object in the sort order.
        /// Zero: This instance occurs in the same position in the sort order.
        /// Greater then zero: This instance follows the other object in the sort order.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (null == obj) return 1;

            Card otherCard = obj as Card;
            
            if (otherCard.GetSuit > GetSuit)
                return -1;

            if (otherCard.GetSuit < GetSuit)
                return 1;

            if (otherCard.GetSuit == GetSuit)
            {
                if (otherCard.GetValue > GetValue)
                    return -1;

                if (otherCard.GetValue < GetValue)
                    return 1;

                return 0;
            }

            return 1;
        }
    }
}
