using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace KcptunGUI {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public static SubFrame.ClientMode pageClientMode;
        public static SubFrame.ServerMode pageServerMode;
        public static SubFrame.Configure pageConfigure;
        public static SubFrame.Status pageStatus;
        public static SubFrame.About pageAbout;
        //public static SubFrame.Helper pageHelper;
        public MainWindow(){
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;//窗体加载完成
            this.Closing += MainWindow_Closing;//窗口即将关闭,可取消
            this.Closed += MainWindow_Closed;//窗口已确定将关闭
        }

        #region 初始化
        private void MainWindow_Loaded( object sender , RoutedEventArgs e ) {//窗体加载完成
            if( App.AppConfigObject.RememberWinLocation && App.AppConfigObject.WinLocation[0] != 0 && App.AppConfigObject.WinLocation[1] != 0 ) {
                this.Top = Convert.ToDouble(App.AppConfigObject.WinLocation[0]);
                this.Left = Convert.ToDouble(App.AppConfigObject.WinLocation[1]);
            } else {
                this.Top = ( SystemParameters.PrimaryScreenHeight / 2 ) - ( this.ActualHeight/2 );
                this.Left = ( SystemParameters.PrimaryScreenWidth / 2 ) - ( this.ActualWidth/2 );
            }
            //托盘
            App.nicon.Icon = KcptunGUI.Resource.图标.ico_64x64_appicon;
            App.nicon.Text = Class.AppAttributes.Name + "  " + Class.AppAttributes.Version;
            App.nicon.Visible = true;
            App.nicon.MouseClick += Nicon_MouseClick;
            App.nicon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            System.Windows.Forms.ToolStripMenuItem[] tsmi = new System.Windows.Forms.ToolStripMenuItem[1];
            tsmi[0] = new System.Windows.Forms.ToolStripMenuItem() { Text = "I18N_ExitApp" , Image = KcptunGUI.Resource.图片.png_none};
            tsmi[0].Click += ( (Object _sender,EventArgs _e) => { this.Close(); } );
            for(Byte i=0;i<tsmi.Length ;i++ ) {
                App.nicon.ContextMenuStrip.Items.Insert( i , tsmi[i] );
            }
            //加载文本
            MainWindow_I18N();
            //实例化页面
            pageClientMode = new SubFrame.ClientMode();
            pageServerMode = new SubFrame.ServerMode();
            pageConfigure = new SubFrame.Configure(); pageConfigure._MainWindow = this;
            pageAbout = new SubFrame.About();
            pageStatus = new SubFrame.Status(); pageStatus._MainWindow = this;
            MainWindow_Frame_ViewArea.Content = pageAbout;
            //
            this.Cursor=App.AppCursor[0];
        }

        /// <summary>窗口即将关闭,可取消</summary>
        private void MainWindow_Closing( object sender , System.ComponentModel.CancelEventArgs e ) {}

        /// <summary>窗口已确定将关闭</summary>
        private void MainWindow_Closed( object sender , EventArgs e ) {
            //if (cmdp_isRun == true) {MainWindow_StopKcptun_Click(null, null);}
            App.AppConfigObject.WinLocation[0] = Convert.ToInt32(this.Top);
            App.AppConfigObject.WinLocation[1] = Convert.ToInt32(this.Left);
            Class.LocalFunction.SaveAppConfig();
            App.nicon.Visible = false;
        }

        /// <summary>加载全球化文本</summary>
        public void MainWindow_I18N() {
            this.MainWindow_TextBlock_AppVersion.Text = "Version: " + Class.AppAttributes.Version;
            this.MainWindow_ListBoxItem_ClientMode.Content = Class.I18N.GetString(this.MainWindow_ListBoxItem_ClientMode.Tag);//I18N[0]
            this.MainWindow_ListBoxItem_ServerMode.Content = Class.I18N.GetString(this.MainWindow_ListBoxItem_ServerMode.Tag );//I18N[1]
            this.MainWindow_ListBoxItem_Configure.Content = Class.I18N.GetString(this.MainWindow_ListBoxItem_Configure.Tag);//I18N[2]
            this.MainWindow_ListBoxItem_Status.Content = Class.I18N.GetString(this.MainWindow_ListBoxItem_Status.Tag);//I18N[4]
            this.MainWindow_ListBoxItem_About.Content = Class.I18N.GetString(this.MainWindow_ListBoxItem_About.Tag);//I18N[3]
        }
        #endregion

        #region Button控件响应
        /// <summary>
        /// 根据x:Name响应按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(Object sender,RoutedEventArgs e) {
            Button thisButton = sender as Button;
            switch(thisButton.Name){
                case "MainWindow_Button_ViewSrcOnGitHub":
                    Process.Start("https://github.com/ragnaroks/KcptunGUI");
                    break;
                case "MainWindow_Button_ViewSrcOnOSchina":
                    Process.Start("https://git.oschina.net/ragnaroks/KcptunGUI");
                    break;
                case "MainWindow_Button_GetHelp":
                    Process.Start("https://github.com/ragnaroks/KcptunGUI/wiki");
                    break;
                case "MainWindow_Button_GetHelp2":
                    Process.Start("https://git.oschina.net/ragnaroks/KcptunGUI/wikis");
                    break;
                default:break;
            }
        }
        #endregion
        private void CheckBox_Checked(object sender, RoutedEventArgs e){//单选框选择事件
            CheckBox thisCheckBox = sender as CheckBox;
            if( false == thisCheckBox.IsMouseOver ) { return; }
            switch (thisCheckBox.Name) {
                case "KcptunConfig_Compress":
                     break;
                default:break;
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e){//单选框取消选择事件
            CheckBox thisCheckBox = sender as CheckBox;
            if (false == thisCheckBox.IsMouseOver) { return; }
            switch (thisCheckBox.Name){
                case "KcptunConfig_Compress":
                    break;
                default:
                    break;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e){//输入框变动事件
            TextBox thisTextBox = sender as TextBox;
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
            ComboBox thisComboBox = sender as ComboBox;
            switch (thisComboBox.Name) {
                case "KcptunConfig_SystemBit":
                    break;
                default:
                    break;
            }
        }
        private void TextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e){//输入框丢失键盘焦点
            TextBox thisTextBox = sender as TextBox;
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
                    if( Class.Functions.IsWindowVisible(new WindowInteropHelper(this).Handle) ) {
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
        /// <summary>
        /// 响应Canvas控件的"鼠标左键松开"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e){
            Canvas thisCanvas = sender as Canvas;
            if (!thisCanvas.IsMouseOver) { return; }
            switch (thisCanvas.Name) {
                default:break;
            }
        }
        /// <summary>
        /// 响应ListBoxItem的"鼠标左键松开"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_MouseLeftButtonUp(object sender , System.Windows.Input.MouseButtonEventArgs e) {
            ListBoxItem thisListBoxItem = sender as ListBoxItem;
            if(!thisListBoxItem.IsMouseOver) { return; }
            switch( thisListBoxItem.Name ) {
                case "MainWindow_ListBoxItem_ClientMode":
                    this.MainWindow_Frame_ViewArea.Content = pageClientMode;
                    break;
                case "MainWindow_ListBoxItem_ServerMode":
                    this.MainWindow_Frame_ViewArea.Content = pageServerMode;
                    break;
                case "MainWindow_ListBoxItem_Configure":
                    this.MainWindow_Frame_ViewArea.Content = pageConfigure;
                    break;
                case "MainWindow_ListBoxItem_About":
                    this.MainWindow_Frame_ViewArea.Content = pageAbout;
                    break;
                case "MainWindow_ListBoxItem_Status":
                    this.MainWindow_Frame_ViewArea.Content = pageStatus;
                    break;
                default:break;
            }
            if( App.AppConfigObject.TabAutoHide ) { this.MainWindow_ToggleButton_ToggleMenu.IsChecked = false; }
        }
    }
}
