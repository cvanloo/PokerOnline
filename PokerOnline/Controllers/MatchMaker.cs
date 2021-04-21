using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokerOnline.Models;
using System.Threading;

namespace PokerOnline.Controllers
{
    public class MatchMaker
    {
        private static MatchMaker singletonRef;
        private readonly static int maxTables = 250;
        private readonly static int maxWaitTime = 240; // time in seconds
        private Queue<Player> playerQueue;
        private Table currentTable;

        /// <summary>
        /// Singleton constructor
        /// </summary>
        private MatchMaker()
        {
            playerQueue = new Queue<Player>();
        }

        /// <summary>
        /// Get a reference to the object.
        /// Instantiates a new object, if none exist yet.
        /// </summary>
        /// <returns>Reference to the object.</returns>
        public MatchMaker GetReference()
        {
            if (null == singletonRef)
                singletonRef = new MatchMaker();
            return singletonRef;
        }

        /// <summary>
        /// Join a match.
        /// </summary>
        /// <returns>The table for the match.</returns>
        public Table JoinMatch()
        {
            // TODO: implement method
            throw new NotImplementedException();
        }
    }
}
