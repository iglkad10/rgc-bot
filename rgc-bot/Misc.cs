using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace rgcbot
{
    public class RGC
    {
        public static int PPM_REQUEST_VERSION_VALIDATION = 9002;
        public static int PPM_REPLY_VERSION_VALIDATION = 9001;

        public static int CLIENT_LOGIN = 1001;
        public static int CLIENT_LOGIN_SUCCESS = 1002;
        public static int CLIENT_REMOTE_IP = 1009;
        public static int CLIENT_LOGIN_END = 1010;
        public static int CLIENT_TIME = 1011;
        public static int CLIENT_JOINALL_END = 1012;
        
        public static int CLIENT_CHAT_SENDMESSAGE = 1101;
        public static int CLIENT_CHAT_MESSAGE = 1102;
        public static int CLIENT_CHAT_CHANNEL_ADD = 1105;
        public static int CLIENT_CHAT_CHANNEL_JOINREQUEST = 1106;
        public static int CLIENT_CHAT_ERROR = 1109;
        public static int CLIENT_CHAT_WHISPER_TO = 1113;
        public static int CLIENT_CHAT_JOINALLCHANNELS = 1116;
        public static int CLIENT_CHAT_CHANNELDATA = 1117;
        public static int CLIENT_CHAT_CHANNELDATA_REFRESH = 1120;
        public static int CLIENT_CHAT_CHANNELAL = 1122;
        public static int CLIENT_CHAT_CHANNELCOUNT = 1124;

        public static int CLIENT_USER_ADD = 1201;
        public static int CLIENT_USER_REM = 1202;
        public static int CLIENT_SIGN_ADD = 1203;
        public static int CLIENT_SIGN_REM = 1204;

        public static int CLIENT_GAME_HOSTED = 1304;

        public static int BOT_REGISTRATION_SUCCESS = 902;
        public static int BOT_REGISTRATION_REQUEST = 904;

        public static int CLIENT_VALIDATE_ID = 1504;
        public static int CLIENT_VALIDATION_SUCCESS = 1505;
        public static int CLIENT_VALIDATE_BOT = 1509;

        public static int EDIT_EMAIL = 1583;

        public static int CLIENT_BUDDY_ADD = 1601;
        public static int CLIENT_BUDDY_REM = 1602;
        public static int CLIENT_BUDDY_STATE = 1603;
        public static int CLIENT_BUDDY_STATES = 1604;

        public static int GAME_PLAYER_JOINED = 1681;
        public static int GAME_PLAYER_LEFT = 1682;

        public static int CLIENT_SET_CLAN = 3301;
        public static int CLIENT_WC3_KEY = 1013;
    }

    public class Globals
    {
        private static IRgcInterface interf = null;

        private static string XMLFILE = "settings.xml";

        public static IRgcInterface GetInterface()
        {
            if (interf == null)
            {
                interf = new RgcInterface();
            }
            return interf;
        }

        public static void Debug(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            DateTime now = DateTime.Now;
            Console.WriteLine("[" + now.Hour + ":" + now.Minute + ":" + now.Second + "] " + message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static string GetSetting(string xpath)
        {
            XPathDocument xdoc = new XPathDocument(XMLFILE);
            XPathNavigator xnav = xdoc.CreateNavigator();

            XPathExpression xexpr = xnav.Compile(xpath);
            XPathNodeIterator iterator = xnav.Select(xexpr);
            try
            {
                iterator.MoveNext();
                return iterator.Current.Value;
            }
            catch (Exception e)
            {
                Globals.Debug(e.Message);
                return "";
            }
        }
    }

}
