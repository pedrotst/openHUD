using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud
{
    class GamesParser
    {
        public GamesParser(string gamesFolder)
        {
            string[] files = Directory.GetFiles(gamesFolder, "*", SearchOption.AllDirectories);
            foreach (var file in files) {
                Console.WriteLine(file);
            }
        }
    }
}
