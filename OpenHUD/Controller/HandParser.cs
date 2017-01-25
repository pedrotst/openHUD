using OpenHud.Persistence;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenHud
{
    class HandParser
    {
        public HandParser(string gamesFolder)
        {
            string[] files = Directory.GetFiles(gamesFolder, "*", SearchOption.AllDirectories);
            Console.WriteLine("Reading Hands At {0}\n", gamesFolder);
            foreach (var file in files) {
                parseFile(file);
            }
            Console.WriteLine("Hand Reading Completed!");
        }

        private void parseFile(string fileName)
        {
            Console.WriteLine("Reading Hands At {0}", fileName);
            var file = new StreamReader(fileName);
            try
            {
                var strHand = getHand(file);
                while (strHand.Any())
                {
                    parseHand(strHand);
                    strHand = getHand(file);
                    Console.Write(".");
                }
                Console.WriteLine("\nSucess!\n");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            file.Close();
        }

       private void parseHand(Queue<string> strHand)
        {
            var curLine = strHand.Dequeue();

            //get hand number
            var regex = new Regex("#\\d*:");
            var handNo = regex.Match(curLine).ToString().Trim('#', ':', ' ');

            //get poker type
            regex = new Regex(":.*\\(");
            var pokerType = regex.Match(curLine).ToString().Trim(':', '(', ' ');

            //get blinds value
            regex = new Regex("\\(.*\\)");
            var blinds = regex.Match(curLine).ToString().Trim('(', ')');
            regex = new Regex(".*\\/");
            var smallBlind = regex.Match(blinds).ToString().Trim('/', ' ', '$');
            regex = new Regex("\\/.* ");
            var bigBlind = regex.Match(blinds).ToString().Trim('/', ' ', '$');
            regex = new Regex(" .*");
            var currency = regex.Match(blinds).ToString().Trim(' ');

            //get date
            regex = new Regex("\\[.*\\]");
            var date = regex.Match(curLine).ToString().Trim('[', ']');
            if (date == "")
            {
                regex = new Regex("-.*");
                date = regex.Match(curLine).ToString().Trim('[', ']', '-', ' ');
            }

            //get table Name
            curLine = strHand.Dequeue();
            regex = new Regex("\\'.*\\'");
            var tableName = regex.Match(curLine).ToString().Trim('\'');

            //get max Seat
            regex = new Regex("\\' \\d*-max");
            var maxSeat = regex.Match(curLine).ToString().Trim('\'', ' ');
            maxSeat = maxSeat.Substring(0, maxSeat.Length - 4);

            //get button seat
            regex = new Regex("#\\d*");
            var buttonSeat = regex.Match(curLine).ToString().Substring(1);


            //loop to get players in table
            List<Player> players = new List<Player>();
            curLine = strHand.Dequeue();
            while (curLine.Substring(0, 4) == "Seat")
            {
                regex = new Regex(".*:");
                var seat = regex.Match(curLine).ToString().Remove(0, 4).Trim(':', ' ');
                regex = new Regex(":.*\\(");
                var playerName = regex.Match(curLine).ToString().Trim(' ', ':', '(');
                regex = new Regex("\\(.*in");
                var chips = regex.Match(curLine).ToString().Trim('(', '$');
                chips = chips.Substring(0, chips.Length - 3);
                players.Add(new Player(seat, playerName, chips));

                curLine = strHand.Dequeue();
            }

            //ignore lines until Hole Cards
            while (curLine != "*** HOLE CARDS ***")
                curLine = strHand.Dequeue();

            curLine = strHand.Dequeue();

            //get cards dealt
            regex = new Regex("to.*\\[");
            var cardsOwner = regex.Match(curLine).ToString().Substring(3);
            cardsOwner = cardsOwner.Remove(cardsOwner.Length - 2);

            regex = new Regex("\\[.*\\]");
            var cards = regex.Match(curLine).ToString().Trim('[', ']');
            cards = String.Join("", cards.Split(' '));

            Hand hand = new Hand(handNo, pokerType, smallBlind, bigBlind, currency, date, tableName, maxSeat, buttonSeat, players, cardsOwner, cards);
            //hand.Print();
            var db = new DbManager();

            db.populateHand(hand);


            foreach (var line in strHand)
            {
                //get game
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
