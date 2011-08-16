using System;
using System.Collections.Generic;
using System.Text;
using rgcbot;

namespace rgc_bot_app
{
    class Program
    {
        static void Main(string[] args)
        {
            IRgcInterface interf = Globals.GetInterface();

            interf.Connect("Ro.Community", "ytinummoc.or");
            interf.Run();

            Globals.Debug("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
