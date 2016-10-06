using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KcptunGUI
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private KcpClientModel client = new KcpClientModel();
        private KcpServerModel server = new KcpServerModel();
        public KcpClientModel Client
        {
            get { return this.client; }
            set { this.client = value; RaisePropertyChanged(nameof(Client)); }
        }
        public KcpServerModel Server
        {
            get { return this.server; }
            set { this.server = value; RaisePropertyChanged(nameof(Server)); }
        }
        public Collection<KcpMode> KcpModeList { get; private set; }
        public Collection<KcpCrypt> KcpCryptList { get; private set; }
        public Collection<KcpType> KcpTypeList { get; private set; }
        public string ClientType { get; set; } = "x86";
        public string ServerType { get; set; } = "x86";
        private bool isClientRunning = false;
        public bool IsClientRunning
        {
            get { return this.isClientRunning; }
            set { this.isClientRunning = value; RaisePropertyChanged(nameof(ButtonClientRun)); }
        }
        public string ButtonClientRun
        {
            get { return this.isClientRunning ? "Stop Kcptun Client" : "Run Kcptun Client"; }
        }
        private bool isServerRunning = false;
        public bool IsServerRunning
        {
            get { return this.isServerRunning; }
            set { this.isServerRunning = value; RaisePropertyChanged(nameof(ButtonServerRun)); }
        }
        public string ButtonServerRun
        {
            get { return this.isServerRunning ? "Stop Kcptun Server" : "Run Kcptun Server"; }
        }
        public string Version { get; private set; }

        public MainWindowViewModel()
        {
            this.KcpCryptList = new Collection<KcpCrypt>() {
                new KcpCrypt("aes"), new KcpCrypt("aes-128"), new KcpCrypt("aes-192"),
                new KcpCrypt("salsa20"), new KcpCrypt("blowfish"), new KcpCrypt("twofish"),
                new KcpCrypt("cast5"), new KcpCrypt("3des"), new KcpCrypt("tea"),
                new KcpCrypt("xtea"), new KcpCrypt("xor"), new KcpCrypt("none") };
            this.KcpModeList = new Collection<KcpMode>() {
                new KcpMode("fast3"), new KcpMode("fast2"), new KcpMode("fast"),
                new KcpMode("normal"), new KcpMode("manual") };
            this.KcpTypeList = new Collection<KcpType>() {
                new KcpType("x86"), new KcpType("x64") };
            this.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class KcpMode
        {
            public string Mode { get; }
            public KcpMode(string _mode)
            {
                this.Mode = _mode;
            }
        }

        public class KcpCrypt
        {
            public string Crypt { get; }
            public KcpCrypt(string _crypt)
            {
                this.Crypt = _crypt;
            }
        }

        public class KcpType
        {
            public string Type { get; }
            public KcpType(string _type)
            {
                this.Type = _type;
            }
        }
    }
}
