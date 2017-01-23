using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud
{
    class Hand
    {
        private string handNumber;
        private string pokerType;
        private string blinds;
        private string date;
        private string tableInfos;
        private string buttonSeat;
        private List<Player> players;

        public Hand(string handNumber, string pokerType, string blinds, string date, string tableInfos, string buttonSeat, List<Player> players)
        {
            this.handNumber = handNumber;
            this.pokerType = pokerType;
            this.blinds = blinds;
            this.date = date;
            this.tableInfos = tableInfos;
            this.buttonSeat = buttonSeat;
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
        }

    }
}
