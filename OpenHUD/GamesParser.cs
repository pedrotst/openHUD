using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenHud
{
    class GamesParser
    {
        public GamesParser(string gamesFolder)
        {
            string[] files = Directory.GetFiles(gamesFolder, "*", SearchOption.AllDirectories);
            foreach (var file in files) {
                parseFile(file);
            }
        }

        private void parseFile(string fileName)
        {
            var file = new StreamReader(fileName);
            parseHand(file);
            file.Close();
        }

       private void parseHand(StreamReader file)
        {

            var strHand = getHand(file);
            Queue<Hand> hands = new Queue<Hand>();

            while (strHand.Any())
            {
                if (!strHand.Any())
                    return;

                var curLine = strHand.Dequeue();

                //get hand number
                var regex = new Regex("#\\d*:");
                var handNo = regex.Match(curLine).ToString().Trim('#', ':', ' ');

                //get poker type
                regex = new Regex(":.*\\(");
                var pokerType = regex.Match(curLine).ToString().Trim(':', '(', ' ');

                //get blinds value
                regex = new Regex("\\(.*\\)");
                var blinds = regex.Match(curLine).ToString().Trim('(', ')', ' ');

                //get date
                regex = new Regex("-.*");
                var date = regex.Match(curLine).ToString().Trim('-', ' ');

                //get table Infos
                curLine = strHand.Dequeue();
                var tableInfos = curLine.Substring(0,curLine.IndexOf("#"));
                regex = new Regex("-.*");
                var buttonSeat = curLine.Substring(curLine.IndexOf("#")).Substring(0, curLine.IndexOf(" "));


                //loop to get players in table
                List<Player> players = new List<Player>();
                curLine = strHand.Dequeue();
                while(curLine.Substring(0, 4).CompareTo("Seat") == 0)
                {
                    regex = new Regex(".*:");
                    var seat = regex.Match(curLine).ToString().Remove(0,4).Trim(':', ' ');
                    regex = new Regex(":.*\\(");
                    var playerName = regex.Match(curLine).ToString().Trim(' ', ':', '(');
                    regex = new Regex("\\(.*in");
                    var chips = regex.Match(curLine).ToString().Trim('(', '$');
                    Console.WriteLine(seat, playerName, chips);
                    curLine = strHand.Dequeue();
                }

                Hand hand = new Hand(handNo, pokerType, blinds, date, tableInfos, buttonSeat, players);
                hand.Print();
                hands.Enqueue(hand);

                foreach (var line in strHand)
                {
                    //get game
                }
                strHand = getHand(file);
            }

        }

        private Queue<string> getHand(StreamReader file)
        {
            Queue<string> buffer = new Queue<string>();
            var line = file.ReadLine();
            while (!String.IsNullOrEmpty(line) && !line.Equals('\n') && !line.Equals("\r\n"))
            {
                buffer.Enqueue(line);
                line = file.ReadLine();
            }

            file.ReadLine();
            file.ReadLine();

            return buffer;
        }

 

    }
}
