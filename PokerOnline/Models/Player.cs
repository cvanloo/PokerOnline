using System.Collections.Generic;

namespace PokerOnline.Models
{
    public class Player
    {
        private Table table;
        private int chips;
        
        public CardHand Hand { get; private set; } = new CardHand();
        public string Username { get; private set; }
        public int Bet { get; set; } = 0;
        public PlayerState State { get; set; } = PlayerState.Player;

        public bool IsAllIn
        {
            get { return 0 == chips; }
        }

        public int GetChips
        {
            get { return chips; }
        }

        /// <summary>
        /// State of the player
        /// </summary>
        public enum PlayerState
        {
            Player,
            //Dealer,
            SmallBlind,
            BigBlind,
            Retired,
            Winner
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="username">Name of the player</param>
        /// <param name="chips">Value of chips</param>
        public Player(string username, int chips)
        {
            Username = username;
            this.chips = chips;
        }

        /// <summary>
        /// Make the player join a table.
        /// </summary>
        /// <param name="table">Table to join</param>
        public void JoinTable(Table table)
        {
            this.table = table.Join(this);
        }

        /// <summary>
        /// Add a card to the players hand.
        /// </summary>
        /// <param name="card">Card to add</param>
        public void AddCard(Card card)
        {
            Hand.AddCard(card);
        }

        /// <summary>
        /// Add multiple cards to the players hand.
        /// </summary>
        /// <param name="cards"></param>
        public void AddCards(IEnumerable<Card> cards)
        {
            Hand.AddCards(cards);
        }

        /* Player actions */

        /// <summary>
        /// Folding: Lay cards down and retire from the game
        /// </summary>
        public void Fold()
        {
            Hand.Cards.Clear();
            State = PlayerState.Retired;

            // After retiring, the player keeps his bet, except for what's already in the pot
            chips += Bet;
            Bet = 0;

            table.UpdateState();
        }

        /// <summary>
        /// Calling: Match the bet
        /// </summary>
        public void Call()
        {
            int difference = table.CurrBetValue - Bet;

            if (difference > chips)
                difference = chips; // Counts as All-in

            chips -= difference;
            Bet += difference;

            table.UpdateState();
        }

        /// <summary>
        /// Raising: Raise the bet. The bet has to be at least doubled
        /// </summary>
        /// <param name="raise">New bet value. Has to be at least double the old bet.</param>
        public void Raise(int raise)
        {
            if (raise >= (2 * table.CurrBetValue) && (raise - table.CurrBetValue) <= chips)
            {
                table.CurrBetValue = raise;
                Call();
            }
        }

        /// <summary>
        /// Checking: Do nothing. Only possible if the bet is already matched.
        /// </summary>
        public void Check()
        {
            if (Bet == table.CurrBetValue)
            {
                table.UpdateState();
            }
        }

        /// <summary>
        /// Get the worth of the players hand.
        /// </summary>
        /// <returns>Worth of the hand.</returns>
        public CardHand.Worth GetHandsWorth()
        {
            return Hand.EvaluateWorth();
        }

        /// <summary>
        /// Add chips to the player.
        /// </summary>
        /// <param name="chips">Number of chips to add.</param>
        public void AddChips(int chips)
        {
            this.chips += chips;
        }
    }
}
