using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud.Model
{
    class Player
    {
        public int Seat;
        public string Name { get; private set;}
        public double Chips;
        public string Cards;
        public List<PlayerAction> Actions;

        public Player(string seat, string name, string chips)
        {
            this.Seat = int.Parse(seat);
            this.Name = name;
            this.Chips = double.Parse(chips, CultureInfo.InvariantCulture);
            Cards = null;
            this.Actions = new List<PlayerAction>();
        }
    }
}
