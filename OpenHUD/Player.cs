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
        private int seat;
        private string name;
        private double chips;
        private Card[] cards = new Card[2];

        public Player(string seat, string name, string chips)
        {
            this.seat = int.Parse(seat);
            this.name = name;
            this.chips = double.Parse(chips, CultureInfo.InvariantCulture);
        }

        public void Print()
        {
            Console.WriteLine("Seat {0}: {1} (Chips: {2})", this.seat, this.name, this.chips);
        }
    }
}
