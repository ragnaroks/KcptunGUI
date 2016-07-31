using System;
using System.Windows;
using System.Windows.Controls;

namespace KcptunGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        String strKcptunCommand;
        public MainWindow()
        {
            InitializeComponent();
            this.MainWindow_LogsText.Text = "KcptunGUI  Version: " + App.AppVersion + "(" + App.AppVersionR+")";
            this.KcptunConfig_SystemBit.SelectedIndex = Properties.Settings.Default.setKcptunConfig_SystemBit;
            this.KcptunConfig_Compress.IsChecked = (true == Properties.Settings.Default.setKcptunConfig_Compress ? true : false);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e){
            CheckBox thisCheckBox = (CheckBox)sender; if (false==thisCheckBox.IsMouseOver) { return; }
            switch (thisCheckBox.Name) {
                case "KcptunConfig_Compress":
                    Properties.Settings.Default.setKcptunConfig_Compress = true; Properties.Settings.Default.Save(); break;
                default:
                    break;
            }
            ShowCommandInTitle();
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox thisCheckBox = (CheckBox)sender; if (false == thisCheckBox.IsMouseOver) { return; }
            switch (thisCheckBox.Name)
            {
                case "KcptunConfig_Compress":
                    Properties.Settings.Default.setKcptunConfig_Compress = false; Properties.Settings.Default.Save(); break;
                default:
                    break;
            }
            ShowCommandInTitle();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e){//输入框变动事件
            TextBox thisTextBox = (TextBox)sender; //if (false == thisTextBox.IsKeyboardFocused) { return; }
            
        }
        private void KcptunConfig_SystemBit_SelectionChanged(object sender, SelectionChangedEventArgs e){//更改了系统版本
            ComboBox thisComboBox = (ComboBox)sender;
            Properties.Settings.Default.setKcptunConfig_SystemBit = thisComboBox.SelectedIndex; Properties.Settings.Default.Save();
            this.MainWindow_LogsText.Text += "\n已将系统版本更改为" + (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "x86" : "x86_64");
            ShowCommandInTitle();
        }
        private void ShowCommandInTitle(){
            strKcptunCommand = (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "client_windows_386.exe" : "client_windows_amd64.exe")
                                        + " -r \"" + Properties.Settings.Default.setKcptunConfig_Server + "\""
                                        + " -l \"0.0.0.0:" + Properties.Settings.Default.setKcptunConfig_LocalPort + "\""
                                        + " -mode " + Properties.Settings.Default.setKcptunConfig_Mode
                                        + (Properties.Settings.Default.setKcptunConfig_Compress ? "" : " -nocomp")
                                        ;
            this.Title = Properties.Resources.AppName + "  -  Command: [ " + strKcptunCommand + " ]";
        }
    }
}
