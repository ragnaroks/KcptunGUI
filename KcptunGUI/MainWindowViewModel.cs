﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace KcptunGUI
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public KcpClientModel Client { get; } = new KcpClientModel();
        public KcpServerModel Server { get; } = new KcpServerModel();
        public Collection<KcpMode> KcpModeList { get; private set; }
        public Collection<KcpCrypt> KcpCryptList { get; private set; }
        public Collection<KcpType> KcpTypeList { get; private set; }
        public string ClientType { get; set; } = "x86";
        public string ServerType { get; set; } = "x86";

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
