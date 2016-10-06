using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace KcptunGUI
{
    class KcpServerModel
    {
        public string listen { get; set; } = ":29900";
        public string target { get; set; } = "127.0.0.1:12948";
        public string key { get; set; } = "it's a secrect";
        public string crypt { get; set; } = "aes";
        public string mode { get; set; } = "fast";
        public string autoexpire { get; set; } = "0";
        public string mtu { get; set; } = "1350";
        public string sndwnd { get; set; } = "1024";
        public string rcvwnd { get; set; } = "1024";
        public string datashard { get; set; } = "10";
        public string parityshard { get; set; } = "3";
        public string dscp { get; set; } = "0";
        public bool nocomp { get; set; } = false;
        public bool acknodelay { get; set; } = true;
        public string nodelay { get; set; } = "0";
        public string interval { get; set; } = "40";
        public string resend { get; set; } = "0";
        public string nc { get; set; } = "0";
        public string sockbuf { get; set; } = "4194304";
        public string keepalive { get; set; } = "10";
    }
}
