using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using OpenHud.Model;
using OpenHud.Persistence;

namespace OpenHud.Controller
{
    class HandParser
    {
        public HandParser(string gamesFolder)
        {
            var files = Directory.GetFiles(gamesFolder, "*", SearchOption.AllDirectories);
            Console.WriteLine("Reading Hands At {0}\n", gamesFolder);
            foreach (var file in files) {
                ParseFile(file);
            }
            Console.WriteLine("Hand Reading Completed!");
        }

        private void ParseFile(string fileName)
        {
            Console.WriteLine("Reading Hands At {0}", fileName);
            var file = new StreamReader(fileName);
            try
            {
                var strHand = GetHand(file);
                while (strHand.Any())
                {
                    ParseHand(strHand);
                    strHand = GetHand(file);
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

       private void ParseHand(Queue<string> strHand)
        {
            int actionNumber = 0;
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

            //get button Seat
            regex = new Regex("#\\d*");
            var buttonSeat = regex.Match(curLine).ToString().Substring(1);


            //loop to get Players in table
            var players = new Dictionary<string, Player>();
            curLine = strHand.Dequeue();
            string playerName;
            while (curLine.Substring(0, 4) == "Seat")
            {
                regex = new Regex(".*:");
                var seat = regex.Match(curLine).ToString().Remove(0, 4).Trim(':', ' ');
                regex = new Regex(":.*\\(");
                playerName = regex.Match(curLine).ToString().Trim(' ', ':', '(');
                regex = new Regex("\\(.*in");
                var chips = regex.Match(curLine).ToString().Trim('(', '$');
                chips = chips.Substring(0, chips.Length - 3);
                players.Add(playerName, new Player(seat, playerName, chips));

                curLine = strHand.Dequeue();
            }
            // initialize actions map with all players as keys and empty actions

            //get blinds
            regex = new Regex(".*:");
            playerName = regex.Match(curLine).ToString().Trim(':');
            while (playerName != "")
            {
                regex = new Regex(":[^\\$]*");
                var action = regex.Match(curLine).ToString().Trim().Substring(2);
                if (action != "sits out" && action != "is sitting out")
                {
                    regex = new Regex("\\$.*");
                    var value = regex.Match(curLine).ToString().Trim('$');
                    players[playerName].Actions.Add(new PlayerAction(action, value, "Pre-Flop", actionNumber++));
                }
                curLine = strHand.Dequeue();
                regex = new Regex(".*:");
                playerName = regex.Match(curLine).ToString().Trim(':');
            }



            //ignore lines until Hole Cards
            while (curLine != "*** HOLE CARDS ***")
                curLine = strHand.Dequeue();

            curLine = strHand.Dequeue();

            //get Cards dealt
            regex = new Regex("to.*\\[");
            var cardsOwner = regex.Match(curLine).ToString().Substring(3);
            cardsOwner = cardsOwner.Remove(cardsOwner.Length - 2);

            regex = new Regex("\\[.*\\]");
            var cards = regex.Match(curLine).ToString().Trim('[', ']');
            SetPlayerCards(players, cardsOwner, cards);

            //ignore lines until Summary
            while (curLine != "*** SUMMARY ***")
                curLine = strHand.Dequeue();

            curLine = strHand.Dequeue();// summary line
            curLine = strHand.Dequeue();// Board line (optional)
            string board = null;
            if (curLine.StartsWith("Board"))
            {
                regex = new Regex("\\[.*\\]");
                board = regex.Match(curLine).ToString().Trim('[', ']');
                board = String.Join("", board.Split(' ')); // remove blanks
            }

            curLine = strHand.Dequeue();// seats summary
            while (curLine.StartsWith("Seat"))
            {
                regex = new Regex("\\[.*\\]");
                cards = regex.Match(curLine).ToString().Trim('[', ']');
                if (cards != "")
                {
                    regex = new Regex(":.* \\[");
                    cardsOwner = regex.Match(curLine).ToString().Trim(':', ']');
                    cardsOwner = cardsOwner.Substring(0, cardsOwner.Length - 9);
                    regex = new Regex("^[^\\(]*");
                    cardsOwner = regex.Match(cardsOwner).ToString().Trim();
                    SetPlayerCards(players, cardsOwner, cards);
                }
                curLine = strHand.Any() ? strHand.Dequeue() : "";
            }

            var hand = new Hand(handNo, pokerType, smallBlind, bigBlind, currency, date, tableName, maxSeat, buttonSeat, players.Values.ToList(), board);
            var db = new DbManager();

            db.PopulateHand(hand);


            foreach (var line in strHand)
            {
                //get game
            }

        }

        private Queue<string> GetHand(StreamReader file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var buffer = new Queue<string>();
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

        private void SetPlayerCards(Dictionary<string, Player> players, string cardsOwner, string cards)
        {
            cards = string.Join("", cards.Split(' ')); // remove blanks
            players[cardsOwner].Cards = cards;
        }

 

    }
}
