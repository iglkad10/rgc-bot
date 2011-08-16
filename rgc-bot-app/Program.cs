using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rgc_bot_app
{
    class Program
    {
        static void Main(string[] args)
        {
            rgcbot.IRgcInterface interf = rgcbot.Globals.GetInterface();

            interf.Connect("Ro.Community", "ytinummoc.or");
            interf.Run();

        }
    }
}
