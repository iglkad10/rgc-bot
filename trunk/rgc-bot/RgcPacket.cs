using System;
using System.Collections.Generic;
using System.Text;

namespace rgcbot
{
    class RgcPacket
    {
        protected int length;
        protected int code;
        protected List<string> strings;
        protected string encodedbytes;

        public RgcPacket()
        {
            strings = new List<string>();
        }
        public RgcPacket(int length, int code)
        {
            this.code = code;
            strings = new List<string>();
        }
        public void AddString(string data)
        {
            this.length += data.Length + 1;
            strings.Add(data);
        }

        protected void EncodePacket()
        {
            encodedbytes = code.ToString();
            foreach (string s in strings)
            {
                encodedbytes += " " + s;
            }
            length = encodedbytes.Length;
            string strLen = length.ToString().PadLeft(8, '0');
            encodedbytes = strLen + encodedbytes;
        }

        public int Length { get { return length; } set { length = value; } }
        public int Code { get { return code; } set { code = value; } }
        public List<string> Strings { get { return strings; } }
        public string EncodedBytes { get { return encodedbytes; } }

        public static RgcPacket FromByteArray(byte[] bytes)
        {
            RgcPacket pck = new RgcPacket();
            pck.length = Convert.ToInt32(Encoding.ASCII.GetString(bytes, 0, 8));

            int index = 0;
            while (index < pck.length && bytes[8 + index] != ' ')
            {
                index++;
            }
            pck.code = Convert.ToInt32(Encoding.ASCII.GetString(bytes, 8, index));

            pck.encodedbytes = Encoding.ASCII.GetString(bytes);

            if (index < pck.length)
            {
                index = pck.encodedbytes.IndexOf(' ');
                char[] separator = { ' ' };
                string[] texts = Encoding.ASCII.GetString(bytes, index, pck.length + 8 - index).Split(separator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < texts.Length; i++)
                {
                    pck.AddString(texts[i]);
                }
            }

            return pck;
        }
        public static string EncodeString(string message)
        {
            string ret = "";
            for (int i = 0; i < message.Length; i++)
            {
                int c = (int)message[i];
                ret += c.ToString("x2");
            }
            return ret;
        }
        public static string DecodeString(string message)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= message.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(message.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        public byte[] ToByteArray()
        {
            return Encoding.ASCII.GetBytes(encodedbytes);
        }
        public int BytesLength()
        {
            return encodedbytes.Length;
        }
    }

    class RgcPacketVersionValidation : RgcPacket
    {
        public RgcPacketVersionValidation()
        {
            this.code = RGC.PPM_REPLY_VERSION_VALIDATION;
            AddString("4");
            EncodePacket();
        }
    }

    class RgcPacketBotRegistration : RgcPacket
    {
        public RgcPacketBotRegistration()
        {
            this.code = RGC.CLIENT_VALIDATE_BOT;
            EncodePacket();
        }
    }

    class RgcPacketClientValidateId : RgcPacket
    {
        public RgcPacketClientValidateId()
        {
            this.code = RGC.CLIENT_VALIDATE_ID;
            AddString("894212");
            AddString("1313140885");
            AddString("55544641");
            EncodePacket();
        }
    }

    class RgcPacketLogin : RgcPacket
    {
        public RgcPacketLogin(string username, string password)
        {
            this.code = RGC.CLIENT_LOGIN;
            AddString(EncodeString(username));
            AddString(EncodeString(password));
            EncodePacket();
        }
    }

    class RgcPacketJoinRoom : RgcPacket
    {
        public RgcPacketJoinRoom(string roomname)
        {
            this.code = RGC.CLIENT_CHAT_CHANNEL_JOINREQUEST;
            AddString(EncodeString(roomname));
            EncodePacket();
        }
    }

    class RgcPacketChatJoinAllChannels : RgcPacket
    {
        public RgcPacketChatJoinAllChannels()
        {
            this.code = RGC.CLIENT_CHAT_JOINALLCHANNELS;
            EncodePacket();
        }
    }

    class RgcPacketMessage : RgcPacket
    {
        public RgcPacketMessage(string roomid, string message)
        {
            this.code = RGC.CLIENT_CHAT_SENDMESSAGE;
            this.AddString(roomid);
            this.AddString(EncodeString(message));
            EncodePacket();
        }
    }

    class RgcPacketWhisper : RgcPacket
    {
        public RgcPacketWhisper(string username, string message)
        {
            this.code = RGC.CLIENT_CHAT_WHISPER_TO;
            this.AddString(EncodeString(username));
            this.AddString(EncodeString(message));
            EncodePacket();
        }
    }
}
