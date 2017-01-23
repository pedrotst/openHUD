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

        public Card(int card, Suits suit)
        {
            this.card = new Tuple<int, Suits>(card, suit);
        }
    }
}
