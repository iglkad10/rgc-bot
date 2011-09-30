using System;
using System.Collections.Generic;
using System.Text;

namespace rgcbot
{
    public interface IRgcInterface
    {
        bool Connect(string username, string password);
        void Run();

        void AddHandler(IRgcEventHandler handler);

        void SendMessage(string roomid, string message);
        void SendWhisper(string username, string message);
    }
}
