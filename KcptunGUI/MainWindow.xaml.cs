using System;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace KcptunGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Int32 intKcptunConfig_SystemBit=32;
        Boolean boolKcptunConfig_Compress=false;
        String strKcptunConfig_Server, strKcptunConfig_LocalPort, strKcptunConfig_Mode;//基础参数
        String strKcptunCommand = "";
        Configuration AppConfig;
        public MainWindow()
        {
            InitializeComponent();
            //AppConfig =System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private void ShowCommandInTitle(){
            strKcptunCommand = (intKcptunConfig_SystemBit == 32 ? "client_windows_386.exe" : "client_windows_amd64.exe")
                                        + " -r \""+strKcptunConfig_Server+"\""
                                        + " -l \"0.0.0.0:"+strKcptunConfig_LocalPort+"\""
                                        + " -mode "+strKcptunConfig_Mode
                                        ;
            this.Title = "    " + "Command: [ " + strKcptunCommand + " ]";
        }

        private void KcptunConfig_SystemBit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox thisComboBox = (ComboBox)sender;
            switch (thisComboBox.SelectedIndex) {
                case 0:
                    intKcptunConfig_SystemBit = 32;break;
                case 1:
                    intKcptunConfig_SystemBit = 64;break;
                default:
                    intKcptunConfig_SystemBit = 32;break;
            }
            //AppConfig.AppSettings.Settings["setintKcptunConfig_SystemBit"].Value = intKcptunConfig_SystemBit.ToString();
            ShowCommandInTitle();
        }
    }
}
