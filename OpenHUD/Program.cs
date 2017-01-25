using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenHud
{
    class Program
    {
        static void Main(string[] args)
        {
            HandParser fileParser = new HandParser(@"../../hands_example");
            Console.ReadLine();
        }
    }
}
