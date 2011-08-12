using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace rgcbot
{
    public class RgcInterface : IRgcInterface
    {
        private bool _connected;
        private string _username;
        private string _password;
        private TcpClient _client;
        private NetworkStream _stream;
        private byte[] _buffer;
        private int _index;
        private List<int> _ignoredpackets;

        Dictionary<string, int> appearances = new Dictionary<string, int>();

        public RgcInterface()
        {
            Globals.Debug("Initializing");
            _buffer = new byte[10240];
            EmptyBuffer();

            _ignoredpackets = new List<int>();
            _ignoredpackets.Add(RGC.CLIENT_SIGN_ADD);
            _ignoredpackets.Add(RGC.CLIENT_SIGN_REM);
            _ignoredpackets.Add(RGC.CLIENT_CHAT_CHANNELDATA);
            _ignoredpackets.Add(RGC.CLIENT_SET_CLAN);
            _ignoredpackets.Add(RGC.CLIENT_WC3_KEY);
            _ignoredpackets.Add(RGC.CLIENT_CHAT_CHANNELDATA_REFRESH);
            _ignoredpackets.Add(RGC.CLIENT_SIGN_REM);
            _ignoredpackets.Add(RGC.CLIENT_SIGN_REM);

        }

        public bool Connect(string username, string password)
        {
            _client = new TcpClient();
            
            try
            {
                Globals.Debug("Connecting to " + Globals.HOST + "...");
                _client.Connect(Globals.HOST, Globals.PORT);
                _stream = _client.GetStream();
                _connected = true;
            }
            catch (Exception e)
            {
                _connected = false;
                Globals.Debug("Cannot connect: " + e.Message);
                return _connected;
            }
            
            _username = username;
            _password = password;
            Globals.Debug("Connected!");
            return _connected;
        }

        public void Run()
        {
            while (true)
            {
                byte[] data = new byte[1024];
                int nRead = _stream.Read(data, 0, 1024);

                OnDataReceived(data, nRead);
            }
        }

        public void SendMessage(string message)
        {
        }

        void EmptyBuffer(int startindex = 0)
        {
            for (int i = startindex; i < 10240; i++)
            {
                _buffer[i] = 0;
            }
            _index = startindex;
        }

        private void OnDataReceived(byte[] data, int length)
        {
            Array.Copy(data, 0, _buffer, _index, length);

            string strData = Encoding.ASCII.GetString(_buffer);
            strData = strData.Replace("\0", "");

            while (strData != "")
            {
                int pckLen = Convert.ToInt32(strData.Substring(0, 8)) + 8;

                if (pckLen > strData.Length)
                {
                    break;
                }

                string pckStr = strData.Substring(0, pckLen);
                //Globals.Debug(" <--- " + pckStr);
                ProcessPacket(Encoding.ASCII.GetBytes(pckStr));
                strData = strData.Substring(pckLen);
            }

            if (strData != "")
            {
                Array.Copy(Encoding.ASCII.GetBytes(strData), 0, _buffer, 0, strData.Length);
                EmptyBuffer(strData.Length);
            }
            else
            {
                EmptyBuffer();
            }
        }

        private void ProcessPacket(byte[] data)
        {
            RgcPacket pck = RgcPacket.FromByteArray(data);

            if (_ignoredpackets.Contains(pck.Code))
            {
                return;
            }

            RgcPacket response1 = null;
            RgcPacket response2 = null;

            if (pck.Code == RGC.PPM_REQUEST_VERSION_VALIDATION)
            {
                response1 = new RgcPacketVersionValidation();
                Globals.Debug("Sending version validation...");
            }
            else if (pck.Code == RGC.BOT_REGISTRATION_REQUEST)
            {
                response1 = new RgcPacketBotRegistration();
                Globals.Debug("Sending bot registration...");
            }
            else if (pck.Code == RGC.CLIENT_VALIDATION_SUCCESS)
            {
                response1 = new RgcPacketLogin(_username, _password);
                Globals.Debug("Logging in as " + _username + "...");
            }
            else if (pck.Code == RGC.CLIENT_LOGIN_SUCCESS)
            {
                response1 = new RgcPacketChatJoinAllChannels();
                Globals.Debug("Joining all channels...");
                //response2 = new RgcPacketJoinRoom("ampulamare");
                //Globals.Debug("Joining ampulamare...");
            }
            else if (pck.Code == RGC.CLIENT_CHAT_CHANNEL_ADD)
            {
                Globals.Debug("Joined channel: " + RgcPacket.DecodeString(pck.Strings[2]) + ", id=" + pck.Strings[0]);
            }
            else if (pck.Code == RGC.CLIENT_USER_ADD)
            {
                int i = 2;
                while (i < pck.Strings.Count)
                {
                    i += 1; // skip ip
                    string username = RgcPacket.DecodeString(pck.Strings[i]);

                    if (appearances.ContainsKey(username))
                    {
                        appearances[username]++;
                    }
                    else
                    {
                        appearances[username] = 1;
                    }

                    Globals.Debug(" --- Channel: " + pck.Strings[0] + ", joins: " + username);
                    i += 3; // skip name, level, color
                    if (i >= pck.Strings.Count)
                    {
                        break;
                    }
                    if (pck.Strings[i] == "0")
                    {
                        i += 1; // skip clan, doesn't have any
                    }
                    else
                    {
                        i += 1; // skip clan

                        if (i >= pck.Strings.Count)
                        {
                            break;
                        }

                        if (pck.Strings[i] == "0")
                        {
                            i += 1; // skip prefix, doesn't have one
                        }
                        else
                        {
                            i += 3; // skip prefix
                        }

                        if (i >= pck.Strings.Count)
                        {
                            break;
                        }

                        if (pck.Strings[i] == "0")
                        {
                            i += 1; // skip suffix, doesn't have one
                        }
                        else
                        {
                            i += 3; // skip suffix
                        }
                    }
                }
            }
            else if (pck.Code == RGC.CLIENT_CHAT_MESSAGE)
            {
                Globals.Debug(RgcPacket.DecodeString(pck.Strings[2]) + "[" + pck.Strings[0] + "]: " + RgcPacket.DecodeString(pck.Strings[3]));
            }
            else
            {
                Globals.Debug("! Unknown packet type, code=" + pck.Code + ", length=" + pck.Length);
            }

            if (response1 != null)
            {
                //Globals.Debug(" --> " + response1.EncodedBytes);
                _stream.Write(response1.ToByteArray(), 0, response1.BytesLength());
            }
            if (response2 != null)
            {
                //Globals.Debug(" --> " + response2.EncodedBytes);
                _stream.Write(response2.ToByteArray(), 0, response2.BytesLength());
            }
        }
    }
}
