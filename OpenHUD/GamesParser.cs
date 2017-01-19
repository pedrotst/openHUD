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
            var hand = getHand(file);
            if (!hand.Any())
                return;

            var fstLine = hand.FirstOrDefault();
            hand.RemoveAt(0);

            //get hand number
            var regex = new Regex("#\\d*:");
            var handNo = regex.Match(fstLine).ToString().Trim('#',':');
            Console.WriteLine(handNo);

            //get poker type
            //get blinds value
            //get date
            foreach(var line in hand)
            {
                //get game
            }

        }

        private List<string> getHand(StreamReader file)
        {
            List<string> buffer = new List<string>();
            var line = file.ReadLine();
            while (!String.IsNullOrEmpty(line) && !line.Equals('\n') && !line.Equals("\r\n"))
            {
                buffer.Add(line);
                line = file.ReadLine();
            }

            file.ReadLine();
            file.ReadLine();

            return buffer;
        }

 

    }
}
