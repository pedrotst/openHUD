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
        public Card[] cards = null;

        public Player(string seat, string name, string chips)
        {
            this.seat = int.Parse(seat);
            this.name = name;
            this.chips = double.Parse(chips, CultureInfo.InvariantCulture);
        }

        public void Print()
        {
            Console.Write("Seat {0}: {1} (Chips: {2})", this.seat, this.name, this.chips);
            if (cards == null)
                Console.WriteLine();
            else
                Console.WriteLine("[{0}{1} {2}{3}]", cards[0].card.Item1, cards[0].card.Item2, cards[1].card.Item1, cards[0].card.Item2);
        }
    }
}
