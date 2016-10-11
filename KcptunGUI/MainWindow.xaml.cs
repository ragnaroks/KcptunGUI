using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace KcptunGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon notify = new System.Windows.Forms.NotifyIcon();
        private Process clientP, serverP;
        private MainWindowViewModel view;
        private static readonly string DefaultClientConfigFile = "_client.json";
        private static readonly string DefaultServerConfigFile = "_server.json";
        private KcptunUtils kcptun;

        public MainWindow()
        {
            this.view = new MainWindowViewModel();
            this.DataContext = this.view;
            InitializeComponent();
            this.kcptun = new KcptunUtils();
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
            catch
            {
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
            catch
            {
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
                this.view.IsClientRunning = true;
            }
            else
            {
                StopKcptunClient();
                this.view.IsClientRunning = false;
            }
        }

        private void RunKcptunClient()
        {
            this.clientP = new Process();
            this.clientP.StartInfo.CreateNoWindow = true;
            this.clientP.StartInfo.FileName = Path.Combine(KcptunUtils.KcptunPath, this.view.ClientType == "x86" ? KcptunUtils.KcptunClient32 : KcptunUtils.KcptunClient64);
            this.clientP.StartInfo.Arguments = "-c " + DefaultClientConfigFile;
            this.clientP.StartInfo.WorkingDirectory = KcptunUtils.KcptunPath;
            this.clientP.StartInfo.UseShellExecute = false;
            this.clientP.StartInfo.RedirectStandardOutput = true;
            this.clientP.StartInfo.RedirectStandardError = true;
            this.clientP.OutputDataReceived +=
                (_sender, _e) => this.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        this.TextBoxClientLog.AppendText(_e.Data + Environment.NewLine);
                        this.TextBoxClientLog.ScrollToEnd();
                    }));
            this.clientP.ErrorDataReceived +=
                (_sender, _e) => this.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        this.TextBoxClientLog.AppendText(_e.Data + Environment.NewLine);
                        this.TextBoxClientLog.ScrollToEnd();
                    }));
            this.clientP.Start();
            this.clientP.BeginOutputReadLine();
            this.clientP.BeginErrorReadLine();
        }

        private void StopKcptunClient()
        {
            this.clientP.CancelOutputRead();
            this.clientP.CancelErrorRead();
            if (this.clientP != null && !this.clientP.HasExited)
            {
                this.clientP.Kill();
                this.clientP.WaitForExit(3000);
            }
            this.clientP.Dispose();
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
            Process.Start("http://git.oschina.net/ragnaroks/KcptunGUI");
        }

        private void ButtonClearClientLog_Click(object sender, RoutedEventArgs e)
        {
            this.TextBoxClientLog.Clear();
        }

        private void ButtonClearServerLog_Click(object sender, RoutedEventArgs e)
        {
            this.TextBoxServerLog.Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!this.kcptun.CreateKcptunFolder())
            {
                return;
            }
            this.kcptun.ExtractEmbeddedKcptunBinary();
            this.kcptun.UpdateAvailable += Kcptun_UpdateAvailable;
            this.kcptun.CheckKcptunUpdate(5000);
        }

        private void Kcptun_UpdateAvailable(object sender, EventArgs e)
        {
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
                    }
                    if (this.view.IsServerRunning)
                    {
                        StopKcptunServer();
                    }
                }
                else
                {
                    return;
                }
            }
            this.kcptun.UpdateKcptunBinary();
            if (this.view.IsClientRunning)
            {
                RunKcptunClient();
            }
            if (this.view.IsServerRunning)
            {
                RunKcptunServer();
            }
        }

        private void ButtonRunServer_Click(object sender, RoutedEventArgs e)
        {
            if (!this.view.IsServerRunning)
            {
                ExportConfigFile(this.view.Server, Path.Combine(KcptunUtils.KcptunPath, DefaultServerConfigFile));
                RunKcptunServer();
                this.view.IsServerRunning = true;
            }
            else
            {
                StopKcptunServer();
                this.view.IsServerRunning = false;
            }
        }

        private void RunKcptunServer()
        {
            this.serverP = new Process();
            this.serverP.StartInfo.CreateNoWindow = true;
            this.serverP.StartInfo.FileName = Path.Combine(KcptunUtils.KcptunPath, this.view.ClientType == "x86" ? KcptunUtils.KcptunServer32 : KcptunUtils.KcptunServer32);
            this.serverP.StartInfo.Arguments = "-c " + DefaultServerConfigFile;
            this.serverP.StartInfo.WorkingDirectory = KcptunUtils.KcptunPath;
            this.serverP.StartInfo.UseShellExecute = false;
            this.serverP.StartInfo.RedirectStandardOutput = true;
            this.serverP.StartInfo.RedirectStandardError = true;
            this.serverP.OutputDataReceived +=
                (_sender, _e) => this.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        this.TextBoxServerLog.AppendText(_e.Data + Environment.NewLine);
                        this.TextBoxServerLog.ScrollToEnd();
                    }));
            this.serverP.ErrorDataReceived +=
                (_sender, _e) => this.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        this.TextBoxServerLog.AppendText(_e.Data + Environment.NewLine);
                        this.TextBoxServerLog.ScrollToEnd();
                    }));
            this.serverP.Start();
            this.serverP.BeginOutputReadLine();
            this.serverP.BeginErrorReadLine();
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
            if (this.serverP != null && !this.serverP.HasExited)
            {
                this.serverP.Kill();
                this.serverP.WaitForExit(3000);
            }
            this.serverP.Dispose();
        }
    }
}
