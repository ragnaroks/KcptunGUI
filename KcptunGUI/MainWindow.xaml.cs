using System;
using System.Diagnostics;
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
        String strKcptunArgvs;//,strKcptunArgvs2;
        Regex KcptunConfig_LocalPort_Regex = new Regex(@"\D");
        System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();
        Process cmdp; Boolean cmdp_isRun = false;
        public MainWindow(){
            InitializeComponent();
            this.StateChanged += MainWindow_StateChanged;
            this.Closed += MainWindow_Closed;
            nicon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            nicon.Text = App.AppName; nicon.Visible = true; nicon.MouseClick += Nicon_MouseClick;
            this.MainWindow_LogsText.Text = "KcptunGUI  Version: " + App.AppVersion + "(" + App.AppVersionR+")";
            this.KcptunConfig_SystemBit.SelectedIndex = (Int32)Properties.Settings.Default.setKcptunConfig_SystemBit;
            this.KcptunConfig_Compress.IsChecked = (true == Properties.Settings.Default.setKcptunConfig_Compress ? true : false);
            //
            this.tabitemserver.IsEnabled = false;
        }
        //
        private void MainWindow_Closed(object sender, EventArgs e){//窗口已确定将关闭
            if (cmdp_isRun == true) {MainWindow_StopKcptun_Click(null, null);} nicon.Visible = false;
        }
        private void GenCommandLine(){//生成命令行
            String _a = Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "client_windows_386.exe" : "client_windows_amd64.exe";//客户端版本
            String _b = " -r \"" + Properties.Settings.Default.setKcptunConfig_Server + "\"";//远端地址
            String _c = " -l \":" + Properties.Settings.Default.setKcptunConfig_LocalPort + "\"";//本地监听端口
            String _d = " -mode " +Class.Functions.GetModeStringFromSelectedIndex(Properties.Settings.Default.setKcptunConfig_Mode);//传输模式
            String _e = Properties.Settings.Default.setKcptunConfig_Compress ? "" : " -nocomp";//是否启用压缩
            String _f = Properties.Settings.Default.setKcptunConfig_Connects.Equals(0) ? "" : " -conn " + Properties.Settings.Default.setKcptunConfig_Connects;//多链接
            String _g = Class.Functions.GetDscpStringFromString(Properties.Settings.Default.setKcptunConfig_DSCP);//DSCP
            String _h = Properties.Settings.Default.setKcptunConfig_Key.Equals("") ? "" : " -key " + Properties.Settings.Default.setKcptunConfig_Key;//密钥
            String _i = Properties.Settings.Default.setKcptunConfig_Crypt.Equals(0) ? "" : " -crypt " + Class.Functions.GetCryptStringFromSelectedIndex(Properties.Settings.Default.setKcptunConfig_Crypt);
            String _j = Properties.Settings.Default.setKcptunConfig_MTU.Equals(0) ? "" : " -mtu " + Properties.Settings.Default.setKcptunConfig_MTU;
            strKcptunArgvs = _b + _c + _d + _e + _f+_g+_h+_i+_j;
            this.MainWindow_KcptunCommandLine.Text = _a + strKcptunArgvs;//客户端手动启动命令行

            //String _a2= Properties.Settings.Default.setKcptunConfig_SystemBit2.Equals(0) ? "server_windows_386.exe" : "server_windows_amd64.exe";//服务端版本
            //String _b2=//转发目标端口
            //strKcptunArgvs2 = "";
            //this.MainWindow_KcptunCommandLine2.Text = _a2 + strKcptunArgvs2;//对应的服务端命令行
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e){//单选框选择事件
            CheckBox thisCheckBox = (CheckBox)sender; if (false==thisCheckBox.IsMouseOver) { return; }
            switch (thisCheckBox.Name) {
                case "KcptunConfig_Compress":
                    Properties.Settings.Default.setKcptunConfig_Compress = true; this.MainWindow_LogsText.Text += "\n已启用数据压缩"; break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); GenCommandLine();
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e){//单选框取消选择事件
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
            TextBox thisTextBox = (TextBox)sender;
            switch (thisTextBox.Name) {
                case "MainWindow_LogsText":
                    this.MainWindow_LogsView.ScrollToBottom(); break;
                case "KcptunConfig_Server":
                    Properties.Settings.Default.setKcptunConfig_Server = thisTextBox.Text; break;
                case "KcptunConfig_LocalPort":
                    thisTextBox.Text=KcptunConfig_LocalPort_Regex.Replace(thisTextBox.Text,"");
                    if (thisTextBox.Text.Length >=5 ) { thisTextBox.Text=thisTextBox.Text.Substring(0, 5); } thisTextBox.SelectionStart = thisTextBox.Text.Length;
                    if (thisTextBox.Text.Length > 1) { Properties.Settings.Default.setKcptunConfig_LocalPort = UInt16.Parse(thisTextBox.Text); }
                    break;
                case "KcptunConfig_Connects":
                    thisTextBox.Text = KcptunConfig_LocalPort_Regex.Replace(thisTextBox.Text, "");
                    if (thisTextBox.Text.Length >= 3) { thisTextBox.Text = thisTextBox.Text.Substring(0, 3); } thisTextBox.SelectionStart = thisTextBox.Text.Length;
                    if (thisTextBox.Text.Length > 0){
                        Properties.Settings.Default.setKcptunConfig_Connects = Byte.Parse(thisTextBox.Text);
                        if (thisTextBox.Text.Equals("0")) { this.MainWindow_LogsText.Text += "\n使用默认链接线程设定"; }
                        else{ this.MainWindow_LogsText.Text += "\n已设置为" + Properties.Settings.Default.setKcptunConfig_Connects + "个链接线程"; }
                    }break;
                case "KcptunConfig_DSCP":
                    thisTextBox.Text = KcptunConfig_LocalPort_Regex.Replace(thisTextBox.Text, "");
                    if (thisTextBox.Text.Length >2){ thisTextBox.Text = thisTextBox.Text.Substring(0, 2);} thisTextBox.SelectionStart = thisTextBox.Text.Length;
                    Properties.Settings.Default.setKcptunConfig_DSCP = thisTextBox.Text;
                    if (thisTextBox.Text.Length > 0){this.MainWindow_LogsText.Text += "\n已设置DSCP " + Properties.Settings.Default.setKcptunConfig_DSCP;}
                    else {this.MainWindow_LogsText.Text += "\n已设置DSCP为默认";}
                    break;
                case "KcptunConfig_Key":
                    Properties.Settings.Default.setKcptunConfig_Key = thisTextBox.Text;
                    if (Properties.Settings.Default.setKcptunConfig_Key.Length.Equals(0)) { this.MainWindow_LogsText.Text += "\n已取消Key设置"; }
                    else{ this.MainWindow_LogsText.Text += "\n已设置Key: " + Properties.Settings.Default.setKcptunConfig_Key; }
                    break;
                case "KcptunConfig_MTU":
                    thisTextBox.Text = KcptunConfig_LocalPort_Regex.Replace(thisTextBox.Text,"");
                    if (thisTextBox.Text.Length >= 4) { thisTextBox.Text = thisTextBox.Text.Substring(0,4); } thisTextBox.SelectionStart = thisTextBox.Text.Length;
                    if (thisTextBox.Text.Length > 0) {
                        Properties.Settings.Default.setKcptunConfig_MTU = UInt16.Parse(thisTextBox.Text);
                        if (Properties.Settings.Default.setKcptunConfig_MTU.Equals(0)) { this.MainWindow_LogsText.Text += "\n已设置MTU为默认值"; }
                        else { this.MainWindow_LogsText.Text += "\n已设置MTU: " + Properties.Settings.Default.setKcptunConfig_MTU; }
                    }else{
                        Properties.Settings.Default.setKcptunConfig_MTU = 0; this.MainWindow_LogsText.Text += "\n已设置MTU为默认值";
                    }
                    break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); GenCommandLine();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e){//下拉选单变动事件
            ComboBox thisComboBox = (ComboBox)sender;
            switch (thisComboBox.Name) {
                case "KcptunConfig_SystemBit":
                    Properties.Settings.Default.setKcptunConfig_SystemBit = (Byte)thisComboBox.SelectedIndex;
                    this.MainWindow_LogsText.Text += "\n将使用" + (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "x86" : "x86_64")+"版本"; break;
                case "KcptunConfig_Mode":
                    Properties.Settings.Default.setKcptunConfig_Mode = (Byte)thisComboBox.SelectedIndex;
                    this.MainWindow_LogsText.Text += "\n已选择" + Class.Functions.GetModeStringFromSelectedIndex(Properties.Settings.Default.setKcptunConfig_Mode) +"传输模式"; break;
                case "KcptunConfig_Crypt":
                    Properties.Settings.Default.setKcptunConfig_Crypt=(Byte)thisComboBox.SelectedIndex;
                    this.MainWindow_LogsText.Text += "\n已选择" + Class.Functions.GetCryptStringFromSelectedIndex(Properties.Settings.Default.setKcptunConfig_Crypt) + "加密"; break;
                default:
                    break;
            }
            Properties.Settings.Default.Save(); GenCommandLine();
        }
        private void TextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e){//输入框丢失键盘焦点
            TextBox thisTextBox = (TextBox)sender;
            switch (thisTextBox.Name) {
                case "KcptunConfig_MTU":
                    if (thisTextBox.Text.Length.Equals(0)) { thisTextBox.Text = "0"; }
                    break;
                default:break;
            }
        }
        private void MainWindow_StateChanged(object sender, EventArgs e){
            if (this.WindowState.Equals(WindowState.Minimized) && Class.Functions.IsWindowVisible(new System.Windows.Interop.WindowInteropHelper(this).Handle)) { this.Hide();}
        }
        
        private void Nicon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e){
            if(Class.Functions.IsWindowVisible(new System.Windows.Interop.WindowInteropHelper(this).Handle)) { this.Hide(); }
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
            cmdp_isRun = true;
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
            cmdp_isRun = false;
            this.MainWindow_LogsText.Text += "\nKcptun已停止运行," + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口已释放";
            nicon.ShowBalloonTip(1500, App.AppName + "  " + App.AppVersion, "Kcptun已停止运行," + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口已释放", System.Windows.Forms.ToolTipIcon.Info);
            this.MainWindow_RunKcptun.IsEnabled = true; this.MainWindow_StopKcptun.IsEnabled = false;
        }
    }
}
