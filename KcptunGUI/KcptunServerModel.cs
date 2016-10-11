using System.ComponentModel;

namespace KcptunGUI
{
    class KcptunServerModel
    {
        [DefaultValue(":29900")]
        public string listen { get; set; } = ":29900";
        [DefaultValue("127.0.0.1:12948")]
        public string target { get; set; } = "127.0.0.1:12948";
        [DefaultValue("it's a secrect")]
        public string key { get; set; } = "it's a secrect";
        [DefaultValue("aes")]
        public string crypt { get; set; } = "aes";
        [DefaultValue("fast")]
        public string mode { get; set; } = "fast";
        [DefaultValue("0")]
        public int autoexpire { get; set; } = 0;
        [DefaultValue(1350)]
        public int mtu { get; set; } = 1350;
        [DefaultValue(1024)]
        public int sndwnd { get; set; } = 1024;
        [DefaultValue(1024)]
        public int rcvwnd { get; set; } = 1024;
        [DefaultValue(10)]
        public int datashard { get; set; } = 10;
        [DefaultValue(3)]
        public int parityshard { get; set; } = 3;
        [DefaultValue(0)]
        public int dscp { get; set; } = 0;
        [DefaultValue(false)]
        public bool nocomp { get; set; } = false;
        [DefaultValue(true)]
        public bool acknodelay { get; set; } = true;
        [DefaultValue(0)]
        public int nodelay { get; set; } = 0;
        [DefaultValue(40)]
        public int interval { get; set; } = 40;
        [DefaultValue(0)]
        public int resend { get; set; } = 0;
        [DefaultValue(0)]
        public int nc { get; set; } = 0;
        [DefaultValue(4194304)]
        public int sockbuf { get; set; } = 4194304;
        [DefaultValue(10)]
        public int keepalive { get; set; } = 10;
    }
}
