using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rgcbot
{
    class RgcEventHandler
    {
        private IRgcInterface _interf;
        private string _username;
        private Dictionary<string, List<string>> _roomusers;

        public RgcEventHandler(IRgcInterface interf)
        {
            _interf = interf;
            _roomusers = new Dictionary<string, List<string>>();
        }

        public void HandleLoggedIn(string username)
        {
            _username = username;
        }

        public void HandleSelfJoinedRoom(string roomid)
        {
        }

        public void HandleJoinedRoom(string roomid, string username)
        {
            if (username == _username)
            {
                return;
            }

            if (!_roomusers.ContainsKey(roomid))
            {
                _roomusers[roomid] = new List<string>();
            }
            if (!_roomusers[roomid].Contains(username))
            {
                _roomusers[roomid].Add(username);
            }
        }

        public void HandleMessage(string roomid, string username, string message)
        {
            if (roomid != "238") // REMOVE THIS CHECK (OR REPLACE WITH 227 - Ro.Community ID)
            {
                return;
            }
            if (message == ".help")
            {
                _interf.SendMessage(roomid, "RGC ChatBot, created by TehLamz0r");
            }
        }

    }
}
