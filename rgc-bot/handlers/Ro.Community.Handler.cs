using System;
using System.Collections.Generic;

namespace rgcbot
{
    public class RoCommunityHandler : IRgcEventHandler
    {
        private IRgcInterface _interf;
        private string _username;
        private Dictionary<string, List<string>> _roomusers;
        private Dictionary<string, string> _rooms;

        public RoCommunityHandler(IRgcInterface interf)
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
                OnHelp(username);
            }
            else if (texts[0] == ".alert")
            {
                OnAlert(roomid, username, texts);
            }
        }

        private void OnHelp(string username)
        {
            _interf.SendWhisper(username, "I am an RGC ChatBot, created by TehLamz0r; Available commands: .help .alert");
        }

        private void OnAlert(string roomid, string username, string[] texts)
        {
            if (texts.Length < 2)
            {
                _interf.SendWhisper(username, username + ", please use .whois <nickname> (nickname can be partial)");
                return;
            }

            string tosearch = texts[1].ToLower();
            string message = "";
            for (int i = 2; i < texts.Length; i++)
            {
                message += texts[i] + " ";
            }

            List<string> users = _roomusers[roomid];
            foreach (string s in users)
            {
                if (s.ToLower().Contains(tosearch) || tosearch == "*")
                {
                    _interf.SendWhisper(s, username + "[" + _rooms[roomid] + "] : " + message);
                }
            }
        }
    }
}
