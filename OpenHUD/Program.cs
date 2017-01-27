using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHud.Controller;

namespace OpenHud
{
    class Program
    {
        private static void Main(string[] args)
        {
            var fileParser = new HandParser(@"../../hands_example");
            // var fileParser = new HandParser(@"P:\Desv\PokerStars\HandHistory\pedroabreu1");
            Console.ReadLine();
        }
    }
}
