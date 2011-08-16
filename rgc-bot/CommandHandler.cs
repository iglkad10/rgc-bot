using System;
using System.Collections.Generic;

namespace rgcbot
{
    class RgcEventHandler
    {
        private IRgcInterface _interf;
        private string _username;
        private Dictionary<string, List<string>> _roomusers;
        private Dictionary<string, string> _rooms;

        public RgcEventHandler(IRgcInterface interf)
        {
            _interf = interf;
            _roomusers = new Dictionary<string, List<string>>();
            _rooms = new Dictionary<string, string>();
        }

        public void HandleLoggedIn(string username)
        {
            _username = username;
        }

        public void HandleSelfJoinedRoom(string roomid, string roomname)
        {
            Globals.Debug("Joined channel: " + roomname + ", id=" + roomid, ConsoleColor.White);

            _rooms[roomid] = roomname;
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

            Globals.Debug(_rooms[roomid] + ": join: " + username, ConsoleColor.Green);
        }

        public void HandleLeftRoom(string roomid, string username)
        {
            if (username == _username)
            {
                return;
            }

            if (_roomusers.ContainsKey(roomid))
            {
                _roomusers[roomid].Remove(username);
            }

            Globals.Debug(_rooms[roomid] + ": left: " + username, ConsoleColor.Red);
        }

        public void HandleMessage(string roomid, string username, string message)
        {
            Globals.Debug(username + "[" + _rooms[roomid] + "]: " + message);

            char[] separator = { ' ' };
            string[] texts = message.Split(separator);

            
            if (roomid != "238") // REMOVE THIS CHECK (OR REPLACE WITH 227 - Ro.Community ID)
            {
                return;
            }

            if (texts[0] == ".help")
            {
                OnHelp(roomid, username);
            }
            else if (texts[0] == "?")
            {
                AiTalk(roomid);
            }
        }

        private void AiTalk(string roomid)
        {
        }

        private void OnHelp(string roomid, string username)
        {
            _interf.SendMessage(roomid, username + ", I am an RGC ChatBot, created by TehLamz0r");
        }

    }
}
