using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud.Model
{
    class Card
    {
        public enum Suits { Hearts = 'h', Spades = 's', Diamonds = 'd', Clubs = 'c'};

        public enum Ranks {
            Deuce = '2', Three, Four, Five, Six, Seven, Eight, Nine,
            Ten = 'T', Jack = 'J', Queen = 'Q', King = 'K', Ace = 'A'
        }

        public Tuple<char, char> card;

        public Card(string card)
        {
            var cardArray = card.ToCharArray();
            var cardCh = cardArray[0];
            var cardSuit = cardArray[1];
            this.card = new Tuple<char, char>(cardCh, cardSuit);
        }

    }
}
