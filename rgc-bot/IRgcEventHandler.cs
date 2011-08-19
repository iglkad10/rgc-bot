using System;
using System.Collections.Generic;
using System.Text;

namespace rgcbot
{
    public interface IRgcEventHandler
    {
        void HandleLoggedIn(string username);
        void HandleSelfJoinedRoom(string roomid, string roomname);
        void HandleJoinedRoom(string roomid, string username);
        void HandleLeftRoom(string roomid, string username);
        void HandleMessage(string roomid, string username, string message);
    }
}
