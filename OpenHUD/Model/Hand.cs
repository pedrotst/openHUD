using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud
{
    class Hand
    {
        public double handNumber;
        public string pokerType;
        public double smallBlind;
        public double bigBlind;
        public string currency;
        public DateTime timestamp;
        public string tableName;
        public string maxSeat;
        public string buttonSeat;
        public string board;
        public List<Player> players;

        public Hand(string handNumber, string pokerType, string smallBlind, string bigBlind, string currency, 
            string date, string tableName, string maxSeat, string buttonSeat, List<Player> players,
            string cardsOwner, string cards, string board)
        {
            this.handNumber = double.Parse(handNumber);
            this.pokerType = pokerType;
            this.smallBlind = double.Parse(smallBlind, CultureInfo.InvariantCulture);
            this.bigBlind = double.Parse(bigBlind, CultureInfo.InvariantCulture);
            this.currency = currency;
            this.timestamp = DateTime.ParseExact(date, "yyyy/MM/dd HH:mm:ss ET", CultureInfo.InvariantCulture);
            this.tableName = tableName;
            this.maxSeat = maxSeat;
            this.buttonSeat = buttonSeat;
            this.board = board;
            var cardPlayer = players.Find(p => p.name == cardsOwner);
            cardPlayer.cards = cards;
            this.players = players;
        }

    }
}
