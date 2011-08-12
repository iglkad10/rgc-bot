using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rgcbot
{
    public class RGC
    {
        public static int PPM_REQUEST_VERSION_VALIDATION = 9002;
        public static int PPM_REPLY_VERSION_VALIDATION = 9001;

        public static int CLIENT_LOGIN = 1001;
        public static int CLIENT_LOGIN_SUCCESS = 1002;
        
        public static int CLIENT_CHAT_SENDMESSAGE = 1101;
        public static int CLIENT_CHAT_CHANNEL_ADD = 1105;
        public static int CLIENT_CHAT_CHANNEL_JOINREQUEST = 1106;
        public static int CLIENT_CHAT_JOINALLCHANNELS = 1116;
        public static int CLIENT_CHAT_CHANNELDATA = 1117;
        public static int CLIENT_CHAT_CHANNELDATA_REFRESH = 1120;

        public static int CLIENT_USER_ADD = 1201;

        public static int BOT_REGISTRATION_SUCCESS = 902;
        public static int BOT_REGISTRATION_REQUEST = 904;

        public static int CLIENT_VALIDATE_ID = 1504;
        public static int CLIENT_VALIDATION_SUCCESS = 1505;
        public static int CLIENT_VALIDATE_BOT = 1509;


        public static int CLIENT_SET_CLAN = 3301;
        public static int CLIENT_WC3_KEY = 1013;
    }

    public class Globals
    {
        private static IRgcInterface interf = null;

        public static string HOST = "node1.chat.europe.rankedgaming.com";
        public static int PORT = 37281;

        public static IRgcInterface GetInterface()
        {
            if (interf == null)
            {
                interf = new RgcInterface();
            }
            return interf;
        }

        public static void Debug(string message)
        {
            Console.WriteLine(message);
        }
    }

}
