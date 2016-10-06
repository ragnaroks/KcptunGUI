using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
        private Process clientP, serverP;
        private MainWindowViewModel view;
        private static readonly string DefaultClientConfigFile = "_client.json";
        private static readonly string DefaultServerConfigFile = "_server.json";
        private static readonly string KcptunFolder = "kcptun";
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

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (this.view.IsClientRunning)
            {
                ButtonRunClient_Click(null, null);
            }
            if (this.view.IsServerRunning)
            {
                ButtonRunServer_Click(null, null);
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
                object o = ImportConfigFile(typeof(KcpClientModel), fileDialog.FileName);
                if (o != null)
                {
                    this.view.Client = o as KcpClientModel;
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
                object o = ImportConfigFile(typeof(KcpServerModel), fileDialog.FileName);
                if (o != null)
                {
                    this.view.Server = o as KcpServerModel;
                }
            }
        }

        private void ButtonRunClient_Click(object sender, RoutedEventArgs e)
        {
            if (!this.view.IsClientRunning)
            {
                ExportConfigFile(this.view.Client, Path.Combine(KcptunFolder, DefaultClientConfigFile));
                this.clientP = new Process();
                this.clientP.StartInfo.CreateNoWindow = true;
                this.clientP.StartInfo.FileName = Path.Combine(KcptunFolder, this.view.ClientType == "x86" ? "client_windows_386.exe" : "client_windows_amd64.exe");
                this.clientP.StartInfo.Arguments = "-c " + DefaultClientConfigFile;
                this.clientP.StartInfo.WorkingDirectory = Path.Combine(Environment.CurrentDirectory, KcptunFolder);
                this.clientP.StartInfo.UseShellExecute = false;
                this.clientP.StartInfo.RedirectStandardOutput = true;
                this.clientP.StartInfo.RedirectStandardError = true;
                this.clientP.OutputDataReceived +=
                    async (_sender, _e) => await this.Dispatcher.BeginInvoke(
                        new Action(() =>
                        {
                            this.TextBoxClientLog.AppendText(_e.Data + Environment.NewLine);
                            this.TextBoxClientLog.ScrollToEnd();
                        }));
                this.clientP.ErrorDataReceived +=
                    async (_sender, _e) => await this.Dispatcher.BeginInvoke(
                        new Action(() =>
                        {
                            this.TextBoxClientLog.AppendText(_e.Data + Environment.NewLine);
                            this.TextBoxClientLog.ScrollToEnd();
                        }));
                this.clientP.Start();
                this.clientP.BeginOutputReadLine();
                this.clientP.BeginErrorReadLine();
                this.view.IsClientRunning = true;
            }
            else
            {
                this.clientP.CancelOutputRead();
                this.clientP.CancelErrorRead();
                if (this.clientP != null && !this.clientP.HasExited)
                {
                    this.clientP.Kill();
                }
                this.clientP.Dispose();
                this.view.IsClientRunning = false;
            }
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

        private void ButtonRunServer_Click(object sender, RoutedEventArgs e)
        {
            if (!this.view.IsServerRunning)
            {
                ExportConfigFile(this.view.Server, Path.Combine(KcptunFolder, DefaultServerConfigFile));
                this.serverP = new Process();
                this.serverP.StartInfo.CreateNoWindow = true;
                this.serverP.StartInfo.FileName = Path.Combine(KcptunFolder, this.view.ClientType == "x86" ? "client_windows_386.exe" : "client_windows_amd64.exe");
                this.serverP.StartInfo.Arguments = "-c " + DefaultServerConfigFile;
                this.serverP.StartInfo.WorkingDirectory = Path.Combine(Environment.CurrentDirectory, KcptunFolder);
                this.serverP.StartInfo.UseShellExecute = false;
                this.serverP.StartInfo.RedirectStandardOutput = true;
                this.serverP.StartInfo.RedirectStandardError = true;
                this.serverP.OutputDataReceived +=
                    async (_sender, _e) => await this.Dispatcher.BeginInvoke(
                        new Action(() =>
                        {
                            this.TextBoxServerLog.AppendText(_e.Data + Environment.NewLine);
                            this.TextBoxServerLog.ScrollToEnd();
                        }));
                this.serverP.ErrorDataReceived +=
                    async (_sender, _e) => await this.Dispatcher.BeginInvoke(
                        new Action(() =>
                        {
                            this.TextBoxServerLog.AppendText(_e.Data + Environment.NewLine);
                            this.TextBoxServerLog.ScrollToEnd();
                        }));
                this.serverP.Start();
                this.serverP.BeginOutputReadLine();
                this.serverP.BeginErrorReadLine();
                this.view.IsServerRunning = true;
            }
            else
            {
                this.serverP.CancelOutputRead();
                this.serverP.CancelErrorRead();
                if (this.serverP != null && !this.serverP.HasExited)
                {
                    this.serverP.Kill();
                }
                this.serverP.Dispose();
                this.view.IsServerRunning = false;
            }
        }
    }
}
