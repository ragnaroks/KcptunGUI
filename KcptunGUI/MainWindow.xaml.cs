using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Text;

namespace KcptunGUI {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow {
        //
        
        public MainWindow(){
            InitializeComponent();
            //this.StateChanged += MainWindow_StateChanged;
            this.Loaded += MainWindow_Loaded;//窗体加载完成
            this.Closing += MainWindow_Closing;//窗口即将关闭,可取消
            this.Closed += MainWindow_Closed;//窗口已确定将关闭
            this.ShowSystemMenuOnRightClick = false;//不响应标题栏右键菜单
        }

        private void MainWindow_Loaded( object sender , RoutedEventArgs e ) {//窗体加载完成
            this.Cursor = App.AppCursor[0];
            //NotifyIcon
            App.nicon.Icon = System.Drawing.Icon.ExtractAssociatedIcon( System.Windows.Forms.Application.ExecutablePath );
            App.nicon.Text = App.AppAttributes["AppName"].ToString() + App.AppAttributes["AppVersion"].ToString();
            App.nicon.Visible = true;
            App.nicon.MouseClick += Nicon_MouseClick;
            App.nicon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            System.Windows.Forms.ToolStripMenuItem[] tsmi = new System.Windows.Forms.ToolStripMenuItem[1];
            tsmi[0] = new System.Windows.Forms.ToolStripMenuItem() {Text = "I18N_ExitApp" , Image = Properties.Resources.icons_niconMenuClose};
            tsmi[0].Click += ( (Object _sender,EventArgs _e) => { this.Close(); } );
            for(Byte i=0;i<tsmi.Length ;i++ ) {
                App.nicon.ContextMenuStrip.Items.Insert( i , tsmi[i] );
            }
            //Debug Logs
            StringBuilder sb = new StringBuilder();
            sb.Append( "AppName : " + App.AppAttributes["AppName"].ToString()+"\n" );
            sb.Append( "AppVersion : " + App.AppAttributes["AppVersion"].ToString()+ "\n" );
            sb.Append( "AppVersionR : " + App.AppAttributes["AppVersionR"].ToString() + "\n" );
            sb.Append( "AppPath : " + App.AppAttributes["AppPath"].ToString() + "\n" );
            sb.Append( "AppExecFilePath : " + App.AppAttributes["AppExecFilePath"].ToString() + "\n" );
            sb.Append( "AppConfigFilePath : " + App.AppAttributes["AppConfigFilePath"].ToString() + "\n" );
            this.logs.Text += sb;
            //
        }

        private void MainWindow_Closing( object sender , System.ComponentModel.CancelEventArgs e ) {//窗口即将关闭,可取消

        }
        private void MainWindow_Closed( object sender , EventArgs e ) {//窗口已确定将关闭
            App.nicon.Visible = false;
            //if (cmdp_isRun == true) {MainWindow_StopKcptun_Click(null, null);}
        }
        //
        private void CheckBox_Checked(object sender, RoutedEventArgs e){//单选框选择事件
            CheckBox thisCheckBox = (CheckBox)sender;
            switch (thisCheckBox.Name) {
                case "KcptunConfig_Compress":
                     break;
                default:
                    break;
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e){//单选框取消选择事件
            CheckBox thisCheckBox = (CheckBox)sender; if (false == thisCheckBox.IsMouseOver) { return; }
            switch (thisCheckBox.Name){
                case "KcptunConfig_Compress":
                    break;
                default:
                    break;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e){//输入框变动事件
            TextBox thisTextBox = (TextBox)sender;
            switch (thisTextBox.Name) {
                case "MainWindow_LogsText":
                    //this.MainWindow_LogsView.ScrollToBottom(); 
                    break;
                case "KcptunConfig_Server":
                    break;
                default:
                    break;
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e){//下拉选单变动事件
            ComboBox thisComboBox = (ComboBox)sender;
            switch (thisComboBox.Name) {
                case "KcptunConfig_SystemBit":
                    break;
                default:
                    break;
            }
        }
        private void TextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e){//输入框丢失键盘焦点
            TextBox thisTextBox = (TextBox)sender;
            switch (thisTextBox.Name) {
                case "KcptunConfig_MTU":
                    break;
                default:break;
            }
        }
        
        private void Nicon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e){
            switch( e.Button ) {
                default:
                case System.Windows.Forms.MouseButtons.Left: //按下鼠标左键,显示/隐藏窗口
                    App.nicon.ContextMenuStrip.Hide();
                    if( Class.Functions.IsWindowVisible( new System.Windows.Interop.WindowInteropHelper( this ).Handle ) ) {
                        this.Hide();
                    } else {
                        this.Show();
                        this.WindowState = WindowState.Normal;
                    }
                    break;
                case System.Windows.Forms.MouseButtons.Right: //按下鼠标右键,显示菜单
                    App.nicon.ContextMenuStrip.Show();
                    break;
            }
            
            
        }

        private void MainWindow_RunKcptun_Click(object sender, RoutedEventArgs e){
            var cmdp= new Process();
            cmdp.StartInfo.CreateNoWindow = true;
            //cmdp.StartInfo.FileName = Environment.CurrentDirectory +"\\"+ (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "client_windows_386.exe" : "client_windows_amd64.exe");
            //cmdp.StartInfo.Arguments = strKcptunArgvs;
            cmdp.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            cmdp.StartInfo.UseShellExecute = false;
            cmdp.StartInfo.RedirectStandardInput = true;
            cmdp.StartInfo.RedirectStandardOutput = true;
            cmdp.StartInfo.RedirectStandardError = true;
            cmdp.ErrorDataReceived += Cmdp_ErrorDataReceived;
            cmdp.Start();
            cmdp.BeginErrorReadLine();
            //cmdp_isRun = true;
            //this.MainWindow_LogsText.Text += "\nKcptun已后台运行,监听本地" + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口";
            //nicon.ShowBalloonTip(1500, App.AppName + "  " + App.AppVersion, "Kcptun已后台运行,监听本地" + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口", System.Windows.Forms.ToolTipIcon.Info);
            //this.MainWindow_RunKcptun.IsEnabled = false; this.MainWindow_StopKcptun.IsEnabled = true;
        }

        private void Cmdp_ErrorDataReceived(object sender, DataReceivedEventArgs e){
            this.Dispatcher.Invoke(new Action(delegate {
                //this.MainWindow_LogsText.Text += "\n" + e.Data;
            }));
        }

        private void MainWindow_StopKcptun_Click(object sender, RoutedEventArgs e){
            /*
            cmdp.CancelErrorRead(); cmdp.Kill(); cmdp_isRun = false;
            this.MainWindow_LogsText.Text += "\nKcptun已停止运行," + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口已释放";
            nicon.ShowBalloonTip(1500, App.AppName + "  " + App.AppVersion, "Kcptun已停止运行," + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口已释放", System.Windows.Forms.ToolTipIcon.Info);
            this.MainWindow_RunKcptun.IsEnabled = true; this.MainWindow_StopKcptun.IsEnabled = false;
            */
        }

        private void Canvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e){//鼠标离开Canvas
            Canvas thisCanvas = (Canvas)sender;
            if (!thisCanvas.IsMouseOver) { return; }
            switch (thisCanvas.Name) {
                case "Canvas_VRW":
                    System.Diagnostics.Process.Start("http://www.ragnaroks.org/");
                    break;
                default:break;
            }
        }

        private void AppExit() {
            System.Environment.Exit( 0 );
        }

    }
}
