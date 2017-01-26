using System;
using System.Collections.Generic;
using System.Globalization;

namespace OpenHud.Model
{
    class Hand
    {
        public double HandNumber;
        public string PokerType;
        public double SmallBlind;
        public double BigBlind;
        public string Currency;
        public DateTime Timestamp;
        public string TableName;
        public string MaxSeat;
        public string ButtonSeat;
        public string Board;
        public List<Player> Players;

        public Hand(string handNumber, string pokerType, string smallBlind, string bigBlind, string currency, string date, 
            string tableName, string maxSeat, string buttonSeat, List<Player> players, string board)
        {
            this.HandNumber = double.Parse(handNumber);
            this.PokerType = pokerType;
            this.SmallBlind = double.Parse(smallBlind, CultureInfo.InvariantCulture);
            this.BigBlind = double.Parse(bigBlind, CultureInfo.InvariantCulture);
            this.Currency = currency;
            this.Timestamp = DateTime.ParseExact(date, "yyyy/MM/dd HH:mm:ss ET", CultureInfo.InvariantCulture);
            this.TableName = tableName;
            this.MaxSeat = maxSeat;
            this.ButtonSeat = buttonSeat;
            this.Board = board;
            this.Players = players;
        }

    }
}
