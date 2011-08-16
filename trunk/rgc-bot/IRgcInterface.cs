using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rgcbot
{
    public interface IRgcInterface
    {
        bool Connect(string username, string password);
        void Run();

        void SendMessage(string roomid, string message);
        void SendWhisper(string username, string message);
    }
}
