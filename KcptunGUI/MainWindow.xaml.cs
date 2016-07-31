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
        public MainWindow(){
            InitializeComponent();
            this.MainWindow_LogsText.Text = "KcptunGUI  Version: " + App.AppVersion + "(" + App.AppVersionR+")";
            this.KcptunConfig_SystemBit.SelectedIndex = Properties.Settings.Default.setKcptunConfig_SystemBit;
            this.KcptunConfig_Compress.IsChecked = (true == Properties.Settings.Default.setKcptunConfig_Compress ? true : false);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e){//选择框选择事件
            CheckBox thisCheckBox = (CheckBox)sender; if (false==thisCheckBox.IsMouseOver) { return; }
            switch (thisCheckBox.Name) {
                case "KcptunConfig_Compress":
                    Properties.Settings.Default.setKcptunConfig_Compress = true; break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); ShowCommand();
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e){//选择框取消选择事件
            CheckBox thisCheckBox = (CheckBox)sender; if (false == thisCheckBox.IsMouseOver) { return; }
            switch (thisCheckBox.Name){
                case "KcptunConfig_Compress":
                    Properties.Settings.Default.setKcptunConfig_Compress = false; break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); ShowCommand();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e){//输入框变动事件
            TextBox thisTextBox = (TextBox)sender; //if (false == thisTextBox.IsKeyboardFocused) { return; }
            switch (thisTextBox.Name) {
                case "MainWindow_LogsText":
                    this.MainWindow_LogsView.ScrollToBottom();
                    break;
                case "KcptunConfig_Server":
                    Properties.Settings.Default.setKcptunConfig_Server = thisTextBox.Text;
                    break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); ShowCommand();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e){//下拉选单变动事件
            ComboBox thisComboBox = (ComboBox)sender;
            switch (thisComboBox.Name) {
                case "KcptunConfig_SystemBit":
                    Properties.Settings.Default.setKcptunConfig_SystemBit = thisComboBox.SelectedIndex; this.MainWindow_LogsText.Text += "\n已将系统位元更改为" + (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "x86" : "x86_64");
                    break;
                case "KcptunConfig_Mode":
                default:
                    break;
            }
            Properties.Settings.Default.Save(); ShowCommand();
        }
        private void ShowCommand(){//生成命令行
            strKcptunCommand = (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "client_windows_386.exe" : "client_windows_amd64.exe")
                                        + " -r \"" + Properties.Settings.Default.setKcptunConfig_Server + "\""
                                        + " -l \"0.0.0.0:" + Properties.Settings.Default.setKcptunConfig_LocalPort + "\""
                                        + " -mode " + Properties.Settings.Default.setKcptunConfig_Mode
                                        + (Properties.Settings.Default.setKcptunConfig_Compress ? "" : " -nocomp")
                                        ;
            this.MainWindow_KcptunCommand.Text = strKcptunCommand;
        }
    }
}
