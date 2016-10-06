﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KcptunGUI
{
    class KcpClientModel
    {
        [DefaultValue(":12948")]
        public string localaddr { get; set; } = ":12948";
        [DefaultValue("vps:29900")]
        public string remoteaddr { get; set; } = "vps:29900";
        [DefaultValue("1")]
        public string conn { get; set; } = "1";
        [DefaultValue("it's a secrect")]
        public string key { get; set; } = "it's a secrect";
        [DefaultValue("aes")]
        public string crypt { get; set; } = "aes";
        [DefaultValue("fast")]
        public string mode { get; set; } = "fast";
        [DefaultValue("0")]
        public string autoexpire { get; set; } = "0";
        [DefaultValue("1350")]
        public string mtu { get; set; } = "1350";
        [DefaultValue("128")]
        public string sndwnd { get; set; } = "128";
        [DefaultValue("1024")]
        public string rcvwnd { get; set; } = "1024";
        [DefaultValue("10")]
        public string datashard { get; set; } = "10";
        [DefaultValue("3")]
        public string parityshard { get; set; } = "3";
        [DefaultValue("0")]
        public string dscp { get; set; } = "0";
        [DefaultValue(false)]
        public bool nocomp { get; set; } = false;
        [DefaultValue(true)]
        public bool acknodelay { get; set; } = true;
        [DefaultValue("0")]
        public string nodelay { get; set; } = "0";
        [DefaultValue("40")]
        public string interval { get; set; } = "40";
        [DefaultValue("0")]
        public string resend { get; set; } = "0";
        [DefaultValue("0")]
        public string nc { get; set; } = "0";
        [DefaultValue("4194304")]
        public string sockbuf { get; set; } = "4194304";
        [DefaultValue("10")]
        public string keepalive { get; set; } = "10";
    }
}
