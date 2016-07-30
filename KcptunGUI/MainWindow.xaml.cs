using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KcptunGUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        String strKcptunConfig_Server, strKcptunConfig_LocalPort, strKcptunConfig_Mode, strKcptunConfig_Compress = "";//基础参数
        String strKcptunCommand = "";
        public MainWindow()
        {
            InitializeComponent();
            this.MainWindow_LogsText.Text = "";
            strKcptunCommand="client_darwin_amd64.exe -r \"" + strKcptunConfig_Server +"\" -l \"0.0.0.0:"+strKcptunConfig_LocalPort+"\"";
        }

        private void ShowCommandInTitle(){
            this.Title = "    " + "Command: [ " + strKcptunCommand + " ]";
        }
    }
}
