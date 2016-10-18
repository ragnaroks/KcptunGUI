using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace KcptunGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notify = new System.Windows.Forms.NotifyIcon();
        private Process clientP, serverP, serverSocks5P;
        private MainWindowViewModel view;
        private static readonly string DefaultClientConfigFile = "_client.json";
        private static readonly string DefaultServerConfigFile = "_server.json";
        private KcptunUtils kcptun;
        private LogUtils clientLog;
        private LogUtils serverLog;
        private LogUtils log;

        public MainWindow()
        {
            this.view = new MainWindowViewModel();
            this.DataContext = this.view;
            InitializeComponent();
            this.clientLog = new LogUtils(this.RichTextBoxClientLog);
            this.serverLog = new LogUtils(this.RichTextBoxServerLog);
            this.log = new LogUtils(this.RichTextBoxLog);
            this.kcptun = new KcptunUtils(this.log);
            this.notify.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this.notify.Text = "Kcptun GUI";
            this.notify.Visible = true;
            this.notify.MouseClick += Notify_MouseClick;
        }

        private void Notify_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
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

        private bool ExportConfigFile(object target, string file)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(file, false, Encoding.Default))
                {
                    string config = JsonConvert.SerializeObject(target, Formatting.Indented,
                        new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
                    sw.Write(config);
                    sw.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                this.log.AppendLog(e.ToString(), Colors.Red);
                return false;
            }
        }

        private object ImportConfigFile(Type type, string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file, Encoding.Default))
                {
                    string json = sr.ReadToEnd();
                    sr.Close();
                    return JsonConvert.DeserializeObject(json, type);
                }
            }
            catch (Exception e)
            {
                this.log.AppendLog(e.ToString(), Colors.Red);
                return null;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        private void ButtonExportClientConfig_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
            fileDialog.Title = "Export Config File";
            fileDialog.Filter = "JSON File (*.json)|*.json";
            if (fileDialog.ShowDialog() == true)
            {
                ExportConfigFile(this.view.Client, fileDialog.FileName);
            }
        }

        private void ButtonImportClientConfig_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "Import Config File";
            fileDialog.Filter = "JSON File (*.json)|*.json";

            if (fileDialog.ShowDialog() == true)
            {
                object o = ImportConfigFile(typeof(KcptunClientModel), fileDialog.FileName);
                if (o != null)
                {
                    this.view.Client = o as KcptunClientModel;
                }
            }
        }

        private void ButtonExportServerConfig_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
            fileDialog.Title = "Export Config File";
            fileDialog.Filter = "JSON File (*.json)|*.json";
            if (fileDialog.ShowDialog() == true)
            {
                ExportConfigFile(this.view.Server, fileDialog.FileName);
            }
        }

        private void ButtonImportServerConfig_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "Import Config File";
            fileDialog.Filter = "JSON File (*.json)|*.json";

            if (fileDialog.ShowDialog() == true)
            {
                object o = ImportConfigFile(typeof(KcptunServerModel), fileDialog.FileName);
                if (o != null)
                {
                    this.view.Server = o as KcptunServerModel;
                }
            }
        }

        private void ButtonRunClient_Click(object sender, RoutedEventArgs e)
        {
            if (!this.view.IsClientRunning)
            {
                ExportConfigFile(this.view.Client, Path.Combine(KcptunUtils.KcptunPath, DefaultClientConfigFile));
                RunKcptunClient();
            }
            else
            {
                StopKcptunClient();
            }
        }

        private void RunKcptunClient()
        {
            try
            {
                this.clientP = new Process();
                this.clientP.StartInfo.CreateNoWindow = true;
                this.clientP.StartInfo.FileName = Path.Combine(KcptunUtils.KcptunPath, this.view.ClientType == "x86" ? KcptunUtils.KcptunClient32 : KcptunUtils.KcptunClient64);
                this.clientP.StartInfo.Arguments = "-c " + DefaultClientConfigFile;
                this.clientP.StartInfo.WorkingDirectory = KcptunUtils.KcptunPath;
                this.clientP.StartInfo.UseShellExecute = false;
                this.clientP.StartInfo.RedirectStandardOutput = true;
                this.clientP.StartInfo.RedirectStandardError = true;
                this.clientP.EnableRaisingEvents = true;
                this.clientP.OutputDataReceived +=
                    (_sender, _e) => this.clientLog.AppendLog(_e.Data, Colors.Black);
                this.clientP.ErrorDataReceived +=
                    (_sender, _e) => this.clientLog.AppendLog(_e.Data, Colors.Black);
                this.clientP.Exited += (_sender, _e) =>
                {
                    this.view.IsClientRunning = false;
                    this.clientLog.AppendLog("Kcptun client has exited abnormally.", Colors.Red, true);
                };
                this.clientP.Start();
                this.clientP.BeginOutputReadLine();
                this.clientP.BeginErrorReadLine();
                this.view.IsClientRunning = true;
            }
            catch (Exception e)
            {
                this.clientLog.AppendLog(e.ToString(), Colors.Red);
                this.clientP = null;
            }
        }

        private void StopKcptunClient()
        {
            this.clientP.CancelOutputRead();
            this.clientP.CancelErrorRead();
            this.clientP.EnableRaisingEvents = false;
            if (this.clientP != null && !this.clientP.HasExited)
            {
                this.clientP.Kill();
                this.clientP.WaitForExit(3000);
            }
            this.clientP.Dispose();
            this.view.IsClientRunning = false;
        }

        private void Label_MySRCPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/menghang/KcptunGUI");
        }

        private void Label_OrigAuthorPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("http://www.ragnaroks.org");
        }

        private void Label_OrigSRCPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/ragnaroks/KcptunGUI");
        }

        private void ButtonClearClientLog_Click(object sender, RoutedEventArgs e)
        {
            this.clientLog.ClearLog();
        }

        private void ButtonClearServerLog_Click(object sender, RoutedEventArgs e)
        {
            this.serverLog.ClearLog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.kcptun.CreateKcptunFolder())
            {
                MessageBox.Show("Can not create kcptun folder in temp folder. Kcptun GUI will close.", "Kcptun GUI", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            this.kcptun.ExtractEmbeddedKcptunBinary();
            this.kcptun.UpdateAvailable += Kcptun_UpdateAvailable;
            this.kcptun.CheckKcptunUpdate(5000);
        }

        private void Kcptun_UpdateAvailable(object sender, EventArgs e)
        {
            bool clientNeedRestore = false;
            bool serverNeedRestore = false;
            if (this.view.IsClientRunning || this.view.IsServerRunning)
            {
                if (MessageBox.Show("Kcptun binary needs to be updated. "
                    + "Do you want to stop running kcptun server / client and update them? "
                    + "Kcptun server / client will restart automaticly after updating.",
                    "Kcptun GUI", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (this.view.IsClientRunning)
                    {
                        StopKcptunClient();
                        clientNeedRestore = true;
                    }
                    if (this.view.IsServerRunning)
                    {
                        if (this.view.EnableSocks5Server)
                        {
                            StopSocks5Server();
                        }
                        StopKcptunServer();
                        serverNeedRestore = true;
                    }
                }
                else
                {
                    return;
                }
            }
            this.kcptun.UpdateKcptunBinary();
            if (clientNeedRestore)
            {
                RunKcptunClient();
            }
            if (serverNeedRestore)
            {
                if (this.view.EnableSocks5Server)
                {
                    RunSocks5Server();
                }
                RunKcptunServer();
            }
        }

        private void ButtonRunServer_Click(object sender, RoutedEventArgs e)
        {
            if (!this.view.IsServerRunning)
            {
                if (this.view.EnableSocks5Server)
                {
                    RunSocks5Server();
                }
                if (this.view.EnableSocks5Server && (this.serverSocks5P == null || this.serverSocks5P.HasExited))
                {
                    return;
                }
                ExportConfigFile(this.view.Server, Path.Combine(KcptunUtils.KcptunPath, DefaultServerConfigFile));
                RunKcptunServer();
            }
            else
            {
                if (this.view.EnableSocks5Server)
                {
                    StopSocks5Server();
                }
                StopKcptunServer();
            }
        }

        private void RunSocks5Server()
        {
            try
            {
                this.serverSocks5P = new Process();
                this.serverSocks5P.StartInfo.CreateNoWindow = true;
                this.serverSocks5P.StartInfo.FileName = Path.Combine(KcptunUtils.KcptunPath, this.view.ClientType == "x86" ? KcptunUtils.Socks5Server32 : KcptunUtils.Socks5Server32);
                this.serverSocks5P.StartInfo.Arguments = this.view.Server.target;
                this.serverSocks5P.StartInfo.WorkingDirectory = KcptunUtils.KcptunPath;
                this.serverSocks5P.StartInfo.UseShellExecute = false;
                this.serverSocks5P.StartInfo.RedirectStandardOutput = true;
                this.serverSocks5P.StartInfo.RedirectStandardError = true;
                this.serverSocks5P.EnableRaisingEvents = true;
                this.serverSocks5P.OutputDataReceived +=
                    (_sender, _e) => this.serverLog.AppendLog(_e.Data, Colors.Blue);
                this.serverSocks5P.ErrorDataReceived +=
                    (_sender, _e) => this.serverLog.AppendLog(_e.Data, Colors.Blue);
                this.serverSocks5P.Exited += (_sender, _e) =>
                {
                    this.serverLog.AppendLog("Socks5 server has exited abnormally.", Colors.Red, true);
                    StopKcptunServer();
                };
                this.serverSocks5P.Start();
                this.serverSocks5P.BeginOutputReadLine();
                this.serverSocks5P.BeginErrorReadLine();
            }
            catch (Exception e)
            {
                this.serverLog.AppendLog(e.ToString(), Colors.Red);
                this.serverSocks5P = null;
            }
        }

        private void StopSocks5Server()
        {
            this.serverSocks5P.CancelOutputRead();
            this.serverSocks5P.CancelErrorRead();
            this.serverSocks5P.EnableRaisingEvents = false;
            if (this.serverSocks5P != null && !this.serverSocks5P.HasExited)
            {
                this.serverSocks5P.Kill();
                this.serverSocks5P.WaitForExit(3000);
            }
            this.serverSocks5P.Dispose();
        }

        private void RunKcptunServer()
        {
            try
            {
                this.serverP = new Process();
                this.serverP.StartInfo.CreateNoWindow = true;
                this.serverP.StartInfo.FileName = Path.Combine(KcptunUtils.KcptunPath, this.view.ClientType == "x86" ? KcptunUtils.KcptunServer32 : KcptunUtils.KcptunServer32);
                this.serverP.StartInfo.Arguments = "-c " + DefaultServerConfigFile;
                this.serverP.StartInfo.WorkingDirectory = KcptunUtils.KcptunPath;
                this.serverP.StartInfo.UseShellExecute = false;
                this.serverP.StartInfo.RedirectStandardOutput = true;
                this.serverP.StartInfo.RedirectStandardError = true;
                this.serverP.EnableRaisingEvents = true;
                this.serverP.OutputDataReceived +=
                    (_sender, _e) => this.serverLog.AppendLog(_e.Data, Colors.Black);
                this.serverP.ErrorDataReceived +=
                    (_sender, _e) => this.serverLog.AppendLog(_e.Data, Colors.Black);
                this.serverP.Exited += (_sender, _e) =>
                {
                    this.view.IsServerRunning = false;
                    this.serverLog.AppendLog("Kcptun server has exited abnormally.", Colors.Red, true);
                    if (this.view.EnableSocks5Server)
                    {
                        StopSocks5Server();
                    }
                };
                this.serverP.Start();
                this.serverP.BeginOutputReadLine();
                this.serverP.BeginErrorReadLine();
                this.view.IsServerRunning = true;
            }
            catch (Exception e)
            {
                this.serverLog.AppendLog(e.ToString(), Colors.Red);
                this.serverP = null;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (this.view.IsClientRunning)
            {
                StopKcptunClient();
            }
            if (this.view.IsServerRunning)
            {
                StopKcptunServer();
            }
            this.notify.Visible = false;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState.Equals(WindowState.Minimized)
                && IsWindowVisible(new System.Windows.Interop.WindowInteropHelper(this).Handle))
            {
                this.Hide();
            }
        }

        private void StopKcptunServer()
        {
            this.serverP.CancelOutputRead();
            this.serverP.CancelErrorRead();
            this.serverP.EnableRaisingEvents = false;
            if (this.serverP != null && !this.serverP.HasExited)
            {
                this.serverP.Kill();
                this.serverP.WaitForExit(3000);
            }
            this.serverP.Dispose();
            this.view.IsServerRunning = false;
        }
    }
}
