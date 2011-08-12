using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace rgcbot
{
    public class RgcInterface : NetworkStreamHandler, IRgcInterface
    {
        private NetworkStream _stream;
        private bool _connected;
        private string _username;
        private string _password;
        private TcpClient _client;
        private byte[] _buffer;
        private int _index;

        public RgcInterface()
        {
            Globals.Debug("Initializing");
            DataAvailable += OnDataReceived;
            _buffer = new byte[4000];
            EmptyBuffer();
        }

        public bool Connect(string username, string password)
        {
            _client = new TcpClient();
            
            try
            {
                Globals.Debug("Connecting to " + Globals.HOST + "...");
                _client.Connect(Globals.HOST, Globals.PORT);
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
            _stream = _client.GetStream();
            BackgroundWorker streamWorker = new BackgroundWorker();
            streamWorker.WorkerSupportsCancellation = true;
            streamWorker.DoWork += ReadFromStream;
            streamWorker.RunWorkerCompleted += (s, a) =>
            {
                if (_connected)
                {
                    streamWorker.RunWorkerAsync(_stream);
                }
            };
            streamWorker.RunWorkerAsync(_stream);
            StreamError += (ex, stream) =>
            {
                if (ex is IOException || ex is InvalidOperationException || ex is ObjectDisposedException)
                {
                    _connected = false;
                    Globals.Debug("Lost connection: " + ex.Message);
                }
                else
                {
                    throw ex;
                }
            };

            while (streamWorker.IsBusy)
            {
                Thread.Sleep(1000);
            }
        }

        public void SendMessage(string message)
        {
        }

        void EmptyBuffer()
        {
            for (int i = 0; i < 4000; i++)
            {
                _buffer[i] = 0;
            }
            _index = 0;
        }

        private void OnDataReceived(byte[] data, int length)
        {
            Array.Copy(data, 0, _buffer, _index, length);

            string strData = Encoding.ASCII.GetString(_buffer);
            strData = strData.Replace("\0", "");

            int pos = 0;
            while ((pos = strData.IndexOf("000")) != -1)
            {
                int pckLen = Convert.ToInt32(strData.Substring(0, 8)) + 8;

                if (pckLen > strData.Length)
                {
                    break;
                }

                string pckStr = strData.Substring(pos, pckLen);
                Globals.Debug(" <--- " + pckStr);
                ProcessPacket(Encoding.ASCII.GetBytes(pckStr));
                strData = strData.Substring(pckLen);
            }

            if (strData != "")
            {
                Array.Copy(Encoding.ASCII.GetBytes(strData), 0, _buffer, 0, strData.Length);
                _index = strData.Length;
            }
            else
            {
                EmptyBuffer();
            }
        }

        private void ProcessPacket(byte[] data)
        {
            RgcPacket pck = RgcPacket.FromByteArray(data);
            RgcPacket response1 = null;
            RgcPacket response2 = null;

            if (pck.Code == RGC.PPM_REQUEST_VERSION_VALIDATION)
            {
                response1 = new RgcPacketVersionValidation();
            }
            else if (pck.Code == RGC.BOT_REGISTRATION_REQUEST)
            {
                response1 = new RgcPacketBotRegistration();
                response2 = new RgcPacketClientValidateId();
            }
            else if (pck.Code == RGC.CLIENT_VALIDATION_SUCCESS)
            {
                response1 = new RgcPacketLogin(_username, _password);
            }
            else if (pck.Code == RGC.CLIENT_LOGIN_SUCCESS)
            {
                response1 = new RgcPacketJoinRoom("Ro.Community");
            }

            if (response1 != null)
            {
                Globals.Debug(" --> " + response1.EncodedBytes);
                _stream.Write(response1.ToByteArray(), 0, response1.BytesLength());
            }
            if (response2 != null)
            {
                Globals.Debug(" --> " + response2.EncodedBytes);
                _stream.Write(response2.ToByteArray(), 0, response2.BytesLength());
            }
        }
    }
}
