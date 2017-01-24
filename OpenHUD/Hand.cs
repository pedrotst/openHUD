using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud
{
    class Hand
    {
        public double handNumber;
        public string pokerType;
        public string blinds;
        public string date;
        public string tableInfos;
        public string buttonSeat;
        public List<Player> players;

        public Hand(string handNumber, string pokerType, string blinds, string date, string tableInfos, string buttonSeat, List<Player> players,
            string cardsOwner, string cards)
        {
            this.handNumber = double.Parse(handNumber);
            this.pokerType = pokerType;
            this.blinds = blinds;
            this.date = date;
            this.tableInfos = tableInfos;
            this.buttonSeat = buttonSeat;
            var card1 = new Card(cards.Substring(0,2));
            var card2 = new Card(cards.Substring(3));
            var cardPlayer = players.Find(p => p.name == cardsOwner);
            cardPlayer.cards = new Card[] { card1, card2 };
            this.players = players;
        }

        public void Print()
        {
            Console.WriteLine("----------------------- Hand #" + handNumber + " ----------------------- ");
            Console.WriteLine(pokerType);
            Console.WriteLine("Blinds: " + blinds);
            Console.WriteLine(date);
            Console.WriteLine(tableInfos);
            Console.WriteLine("Button: " + buttonSeat);
            players.ForEach(p => p.Print());
            Console.WriteLine();
        }

    }
}
