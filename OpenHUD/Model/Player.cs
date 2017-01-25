using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud
{
    class Player
    {
        public int seat;
        public string name { get; private set;}
        public double chips;
        public string cards;

        public Player(string seat, string name, string chips)
        {
            this.seat = int.Parse(seat);
            this.name = name;
            this.chips = double.Parse(chips, CultureInfo.InvariantCulture);
            cards = null;
        }
    }
}
