using System;
using System.Diagnostics;
using System.Dynamic;
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
        private System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();
        private Process cmdp;
        private bool cmdp_isRun = false;
        private MainWindowViewModel view;
        public MainWindow()
        {
            this.view = new MainWindowViewModel();
            this.DataContext = this.view;
            InitializeComponent();
            this.StateChanged += MainWindow_StateChanged;
            this.Closed += MainWindow_Closed;
            nicon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            nicon.Text = "Kcptun GUI";
            nicon.Visible = true;
            nicon.MouseClick += Nicon_MouseClick;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (cmdp_isRun == true)
            {
                MainWindow_StopKcptun_Click(null, null);
            }
            nicon.Visible = false;
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState.Equals(WindowState.Minimized)
                && IsWindowVisible(new System.Windows.Interop.WindowInteropHelper(this).Handle))
            {
                this.Hide();
            }
        }

        private void Nicon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (IsWindowVisible(new System.Windows.Interop.WindowInteropHelper(this).Handle))
            {
                this.Hide();
            }
            else
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }
        }

        private void MainWindow_RunKcptun_Click(object sender, RoutedEventArgs e)
        {
            cmdp = new Process();
            cmdp.StartInfo.CreateNoWindow = true;
            //cmdp.StartInfo.FileName = Environment.CurrentDirectory + "\\" + (Properties.Settings.Default.setKcptunConfig_SystemBit.Equals(0) ? "client_windows_386.exe" : "client_windows_amd64.exe");
            //cmdp.StartInfo.Arguments = strKcptunArgvs;
            cmdp.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
            cmdp.StartInfo.UseShellExecute = false;
            cmdp.StartInfo.RedirectStandardInput = true;
            cmdp.StartInfo.RedirectStandardOutput = true;
            cmdp.StartInfo.RedirectStandardError = true;
            cmdp.ErrorDataReceived += Cmdp_ErrorDataReceived;
            cmdp.Start();
            cmdp.BeginErrorReadLine();
            cmdp_isRun = true;
            //this.MainWindow_LogsText.Text += "\nKcptun已后台运行,监听本地" + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口";
            //nicon.ShowBalloonTip(1500, "Kcptun GUI" + "  " + App.AppVersion, "Kcptun已后台运行,监听本地" + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口", System.Windows.Forms.ToolTipIcon.Info);
            //this.MainWindow_RunKcptun.IsEnabled = false; this.MainWindow_StopKcptun.IsEnabled = true;
        }

        private void Cmdp_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                //this.MainWindow_LogsText.Text += "\n" + e.Data;
            }));
        }

        private void MainWindow_StopKcptun_Click(object sender, RoutedEventArgs e)
        {
            cmdp.CancelErrorRead(); cmdp.Kill();
            cmdp_isRun = false;
            //this.MainWindow_LogsText.Text += "\nKcptun已停止运行," + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口已释放";
            //nicon.ShowBalloonTip(1500, App.AppName + "  " + App.AppVersion, "Kcptun已停止运行," + Properties.Settings.Default.setKcptunConfig_LocalPort + "端口已释放", System.Windows.Forms.ToolTipIcon.Info);
            //this.MainWindow_RunKcptun.IsEnabled = true; this.MainWindow_StopKcptun.IsEnabled = false;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Boolean IsWindowVisible(IntPtr hWnd);
    }
}
