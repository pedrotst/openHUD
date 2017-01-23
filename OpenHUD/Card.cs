using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud
{
    class Card
    {
        public enum Suits { Heart, Spade, Diamond, Club};
        private Tuple<int, Suits> card;

        public Card(int number, Suits suit)
        {
            this.card = new Tuple<int, Suits>(number, suit);
        }

        public Card(string card)
        {
            var number = int.Parse(card.Substring(0));
            this.card = new Tuple<int, Suits>(number, Suits.Club);
        }
    }
}
