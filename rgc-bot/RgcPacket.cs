using System;
using System.Collections.Generic;
using System.Linq;
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
            EncodePacket();
        }
        public RgcPacket(int length, int code)
        {
            this.code = code;
            strings = new List<string>();
            EncodePacket();
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
            return "";
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
            this.code = RGC.BOT_REGISTRATION_SUCCESS;
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

    class RgcPacketMessage : RgcPacket
    {
        public RgcPacketMessage(string message)
        {
            this.code = RGC.CLIENT_CHAT_SENDMESSAGE;
            this.AddString("227");
            this.AddString(EncodeString(message));
            EncodePacket();
        }
    }
}
