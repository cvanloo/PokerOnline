using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PokerOnline.Models;

namespace PokerOnline.Extensions
{
    public static class TableStateExtension
    {
        /// <summary>
        /// Get the next state.
        /// </summary>
        /// <param name="state">The object to operate on. This is the caller, it is passed automatically.</param>
        /// <returns>The next state.</returns>
        public static Table.TableState GetNext(this Table.TableState state)
        {
            switch (state)
            {
                case Table.TableState.Waiting:
                    return Table.TableState.Preflop;
                case Table.TableState.Preflop:
                    return Table.TableState.Flop;
                case Table.TableState.Flop:
                    return Table.TableState.Turn;
                case Table.TableState.Turn:
                    return Table.TableState.River;
                default:
                    return Table.TableState.GameOver;
            }
        }
    }
}
