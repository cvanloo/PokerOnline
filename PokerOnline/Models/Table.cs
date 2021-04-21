using System;
using System.Collections.Generic;
using System.Linq;
using PokerOnline.Extensions;

namespace PokerOnline.Models
{
    public class Table
    {
        private static Table singletonRef; // TMP, TODO: Later a "Match-Maker" should create (multiple) tables 
                                           // and add player from a queue together into games
		public EventHandler TableStateChanged;

        private Deck deck;

        public TableState State { get; private set; }
        public List<Player> Players { get; private set; }
        public int Pot { get; private set; }
        public Player CurrPlayer { get; private set; }
        public int CurrBetValue { get; set; }
        public List<Card> TableCards { get; private set; }

        public List<Player> GetActivePlayers
        {
            get { return Players.Where(p => p.State != Player.PlayerState.Retired).ToList(); }
        }

        /// <summary>
        /// State of the table
        /// </summary>
        public enum TableState
        {
            Waiting, 
            Preflop, // First betting round
            Flop,    // 3 Cards are laid open on the table, second betting round begins
            Turn,    // A 4th card is laid open on the table, third betting round begins
            River,   // A 5th card is laid open on the table, fourth betting round begins
            GameOver
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Table()
        {
            Reset();
        }

        /// <summary>
        /// Get a reference to the object. A new object is created, if it doesn't exist yet.
        /// </summary>
        /// <returns>A reference to the object.</returns>
        public static Table GetReference()
        {
            if (null == singletonRef)
                singletonRef = new Table();

            return singletonRef;
        }

        /// <summary>
        /// Puts the table into it's default state.
        /// </summary>
        public void Reset()
        {
            deck = new Deck();
            deck.ShuffleCards();
            State = TableState.Waiting;
            Players = new List<Player>();
            Pot = 0;
            CurrPlayer = null;
            CurrBetValue = 0;
            TableCards = new List<Card>();
			TableStateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void StartGame()
        {
            State = State.GetNext();
            
            // Select a random smallblind
            List<Player> activePlayers = GetActivePlayers;
            int smallIndex = new Random().Next(0, activePlayers.Count);
            
            Player player = activePlayers[smallIndex];
            player.State = Player.PlayerState.SmallBlind;
            CurrPlayer = player;

            // Make the next player bigblind
            smallIndex++;
            if (smallIndex >= activePlayers.Count)
                smallIndex = 0;

            player = activePlayers[smallIndex];
            player.State = Player.PlayerState.BigBlind;

            // Give two cards to each player
            for (int i = 0; i < 2; i++)
            {
                foreach (Player p in activePlayers)
                {
                    p.AddCard(deck.Deal()[0]);
                }
            }


			TableStateChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the state after a player made a move
        /// </summary>
        public void UpdateState()
        {
            if (TableState.GameOver != State)
            {
                // Check if all player have the same bet value
                bool sameBet = CheckPlayerBet() && CurrBetValue > 0;

                if (sameBet)
                // All player have the same bet, which ends the betting round
                {
                    // Put all bets in the pot
                    AddPlayerBetsToPot();

                    // Change State
                    State = State.GetNext();

                    // Draw table cards
                    DealTableCards();

                    if (TableState.River == State)
                    {
                        // Check who won
                        Player winner = FindWinner();
                        winner.State = Player.PlayerState.Winner;

                        winner.AddChips(Pot);
                        Pot = 0;
                        
                        // Game over
                        State = TableState.GameOver;
                    }
                }

                // Change the current player
                SetNextPlayer();

				TableStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Join the table.
        /// </summary>
        /// <param name="player">Player to join the table.</param>
        /// <returns>The table.</returns>
        public Table Join(Player player)
        {
            Players.Add(player);

			TableStateChanged?.Invoke(this, EventArgs.Empty);
            
			return this;
        }

        /// <summary>
        /// Update the active player.
        /// </summary>
        private void SetNextPlayer()
        {
            List<Player> activePlayers = GetActivePlayers;
            int newIndex = activePlayers.IndexOf(CurrPlayer) + 1;

            if (newIndex >= activePlayers.Count)
                newIndex = 0;

            CurrPlayer = activePlayers[newIndex];
        }
        
        /// <summary>
        /// Deals the table cards (flop, turn, river) according to the current tablestate
        /// </summary>
        private void DealTableCards()
        {
            switch (State)
            {
                case TableState.Flop:
                    TableCards.AddRange(deck.Deal(3));
                    break;
                case TableState.Turn:
                case TableState.River:
                    TableCards.AddRange(deck.Deal());
                    break;
            }
        }

        /// <summary>
        /// Checks if all player have the same bet.
        /// </summary>
        /// <returns>True if all player have the same bet.</returns>
        private bool CheckPlayerBet()
        {
            foreach (Player player in GetActivePlayers)
            {
                if (player.Bet != CurrBetValue)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Add all players bets to the pot.
        /// </summary>
        private void AddPlayerBetsToPot()
        {
            foreach (Player p in GetActivePlayers)
            {
                Pot += p.Bet;
                p.Bet = 0;
            }

            CurrBetValue = 0;
        }

        /// <summary>
        /// Get the winner
        /// </summary>
        /// <returns>The player who won</returns>
        private Player FindWinner()
        {
            CardHand.Worth? worth = null;
            Player winner = null;

            foreach (Player p in GetActivePlayers)
            {
                p.AddCards(TableCards);
                CardHand.Worth nextWorth = p.GetHandsWorth();
                if (null == worth || worth < nextWorth)
                {
                    worth = nextWorth;
                    winner = p;
                }
            }

            return winner;
        }
    }
}
