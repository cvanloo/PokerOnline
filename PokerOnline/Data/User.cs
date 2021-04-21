using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerOnline.Data
{
    public class User
    {
        public string Username { get; set; }
        public byte[] PwHash { get; set; }
    }
}
