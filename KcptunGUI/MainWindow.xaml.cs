using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
        String strKcptunArgvs;
        Regex KcptunConfig_LocalPort_Regex = new Regex(@"\D");
        System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();
        Process cmdp;
        public MainWindow(){
            InitializeComponent();
            this.StateChanged += MainWindow_StateChanged;
            this.Closed += MainWindow_Closed;
            nicon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            nicon.Text = App.AppName; nicon.Visible = true; nicon.MouseClick += Nicon_MouseClick;
            this.MainWindow_LogsText.Text = "KcptunGUI  Version: " + App.AppVersion + "(" + App.AppVersionR+")";
            this.KcptunConfig_SystemBit.SelectedIndex = Properties.Settings.Default.setKcptunConfig_SystemBit;
            this.KcptunConfig_Mode.SelectedIndex = 3;
            this.KcptunConfig_Compress.IsChecked = (true == Properties.Settings.Default.setKcptunConfig_Compress ? true : false);
        }

        private void MainWindow_Closed(object sender, EventArgs e){
            nicon.Visible = false;
        }

        // C函数声明
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hWnd);


        //
        private void GenCommandLine(){//生成命令行
            strKcptunCommand = (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "client_windows_386.exe" : "client_windows_amd64.exe")
                                        + " -r \"" + Properties.Settings.Default.setKcptunConfig_Server + "\""
                                        + " -l \"0.0.0.0:" + Properties.Settings.Default.setKcptunConfig_LocalPort.ToString() + "\""
                                        + " -mode " + Properties.Settings.Default.setKcptunConfig_Mode
                                        + (Properties.Settings.Default.setKcptunConfig_Compress ? "" : " -nocomp")
                                        + (Properties.Settings.Default.setKcptunConfig_Connects.Equals(0) ? "" : " -conn "+ Properties.Settings.Default.setKcptunConfig_Connects)
                                        ;
            strKcptunArgvs= " -r \"" + Properties.Settings.Default.setKcptunConfig_Server + "\""
                                        + " -l \"0.0.0.0:" + Properties.Settings.Default.setKcptunConfig_LocalPort.ToString() + "\""
                                        + " -mode " + Properties.Settings.Default.setKcptunConfig_Mode
                                        + (Properties.Settings.Default.setKcptunConfig_Compress ? "" : " -nocomp")
                                        + (Properties.Settings.Default.setKcptunConfig_Connects.Equals(0) ? "" : " -conn " + Properties.Settings.Default.setKcptunConfig_Connects)
                                        ;
            this.MainWindow_KcptunCommandLine.Text = strKcptunCommand;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e){//选择框选择事件
            CheckBox thisCheckBox = (CheckBox)sender; if (false==thisCheckBox.IsMouseOver) { return; }
            switch (thisCheckBox.Name) {
                case "KcptunConfig_Compress":
                    Properties.Settings.Default.setKcptunConfig_Compress = true; this.MainWindow_LogsText.Text += "\n已启用数据压缩"; break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); GenCommandLine();
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e){//选择框取消选择事件
            CheckBox thisCheckBox = (CheckBox)sender; if (false == thisCheckBox.IsMouseOver) { return; }
            switch (thisCheckBox.Name){
                case "KcptunConfig_Compress":
                    Properties.Settings.Default.setKcptunConfig_Compress = false; this.MainWindow_LogsText.Text += "\n已停用数据压缩"; break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); GenCommandLine();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e){//输入框变动事件
            TextBox thisTextBox = (TextBox)sender; //if (false == thisTextBox.IsKeyboardFocused) { return; }
            switch (thisTextBox.Name) {
                case "MainWindow_LogsText":
                    this.MainWindow_LogsView.ScrollToBottom(); break;
                case "KcptunConfig_Server":
                    Properties.Settings.Default.setKcptunConfig_Server = thisTextBox.Text; break;
                case "KcptunConfig_LocalPort":
                    thisTextBox.Text=KcptunConfig_LocalPort_Regex.Replace(thisTextBox.Text,""); if (thisTextBox.Text.Length >= 5) { thisTextBox.Text=thisTextBox.Text.Substring(0, 5); }
                    thisTextBox.SelectionStart = thisTextBox.Text.Length; if (thisTextBox.Text.Length > 1) { Properties.Settings.Default.setKcptunConfig_LocalPort = UInt32.Parse(thisTextBox.Text); } break;
                case "KcptunConfig_Connects":
                    thisTextBox.Text = KcptunConfig_LocalPort_Regex.Replace(thisTextBox.Text, ""); if (thisTextBox.Text.Length >= 3) { thisTextBox.Text = thisTextBox.Text.Substring(0, 3); }
                    thisTextBox.SelectionStart = thisTextBox.Text.Length;
                    if (thisTextBox.Text.Length > 0){
                        Properties.Settings.Default.setKcptunConfig_Connects = UInt32.Parse(thisTextBox.Text);
                        if (thisTextBox.Text.Equals("0")) { this.MainWindow_LogsText.Text += "\n使用默认链接线程设定"; }
                        else{ this.MainWindow_LogsText.Text += "\n已设置为" + Properties.Settings.Default.setKcptunConfig_Connects + "个链接线程"; }
                    }break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); GenCommandLine();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e){//下拉选单变动事件
            ComboBox thisComboBox = (ComboBox)sender;
            switch (thisComboBox.Name) {
                case "KcptunConfig_SystemBit":
                    Properties.Settings.Default.setKcptunConfig_SystemBit = thisComboBox.SelectedIndex;
                    this.MainWindow_LogsText.Text += "\n将使用" + (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "x86" : "x86_64")+"版本"; break;
                case "KcptunConfig_Mode":
                    ComboBoxItem _cbi = (ComboBoxItem)thisComboBox.SelectedItem; Properties.Settings.Default.setKcptunConfig_Mode = _cbi.Content.ToString();
                    this.MainWindow_LogsText.Text += "\n已选择" + (Properties.Settings.Default.setKcptunConfig_Mode)+"模式"; break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); GenCommandLine();
        }
        private void MainWindow_StateChanged(object sender, EventArgs e){
            if (this.WindowState.Equals(WindowState.Minimized) && MainWindow.IsWindowVisible(new System.Windows.Interop.WindowInteropHelper(this).Handle)) { this.Hide();}
        }
        
        private void Nicon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e){
            if (MainWindow.IsWindowVisible(new System.Windows.Interop.WindowInteropHelper(this).Handle)) { this.Hide(); }
            else { this.Show(); this.WindowState = WindowState.Normal; }
        }

        private void MainWindow_RunKcptun_Click(object sender, RoutedEventArgs e){
            cmdp = new Process();
            cmdp.StartInfo.CreateNoWindow = true;
            cmdp.StartInfo.FileName = Environment.CurrentDirectory +"\\"+ (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "client_windows_386.exe" : "client_windows_amd64.exe");
            cmdp.StartInfo.Arguments = strKcptunArgvs;
            cmdp.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            cmdp.StartInfo.UseShellExecute = false;
            cmdp.StartInfo.RedirectStandardInput = true;
            cmdp.StartInfo.RedirectStandardOutput = true;
            cmdp.StartInfo.RedirectStandardError = true;
            cmdp.ErrorDataReceived += Cmdp_ErrorDataReceived;
            cmdp.Start();
            cmdp.BeginErrorReadLine();

            this.MainWindow_LogsText.Text += "\nKcptun已后台运行,监听本地" + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口";
            nicon.ShowBalloonTip(1500, App.AppName + "  " + App.AppVersion, "Kcptun已后台运行,监听本地" + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口", System.Windows.Forms.ToolTipIcon.Info);
            this.MainWindow_RunKcptun.IsEnabled = false; this.MainWindow_StopKcptun.IsEnabled = true;
        }

        private void Cmdp_ErrorDataReceived(object sender, DataReceivedEventArgs e){
            this.Dispatcher.Invoke(new Action(delegate {
                this.MainWindow_LogsText.Text += "\n" + e.Data;
            }));
        }

        private void MainWindow_StopKcptun_Click(object sender, RoutedEventArgs e){
            cmdp.CancelErrorRead();//cmdp.CancelOutputRead();
            cmdp.Kill();
            this.MainWindow_LogsText.Text += "\nKcptun已停止运行," + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口已释放";
            nicon.ShowBalloonTip(1500, App.AppName + "  " + App.AppVersion, "Kcptun已停止运行," + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口已释放", System.Windows.Forms.ToolTipIcon.Info);
            this.MainWindow_RunKcptun.IsEnabled = true; this.MainWindow_StopKcptun.IsEnabled = false;
        }
    }
}
