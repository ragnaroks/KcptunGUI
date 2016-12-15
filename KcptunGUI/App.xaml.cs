using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;

namespace KcptunGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application{
        public static Dictionary<String ,String> AppAttributes = new Dictionary<String ,String>();//应用属性
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();//托盘图标
        public static Object objConfig = new Object();
        protected override void OnStartup( StartupEventArgs e ) {
            App.AppAttributes["AppName"] = "KcptunGUI";
            App.AppAttributes["AppVersion"] = Application.ResourceAssembly.GetName().Version.Major.ToString() + "." + Application.ResourceAssembly.GetName().Version.Minor.ToString() + "." + Application.ResourceAssembly.GetName().Version.Build.ToString();
            App.AppAttributes["AppVersionR"] = Application.ResourceAssembly.GetName().Version.Revision.ToString();
            App.AppAttributes["AppPath"] = Environment.CurrentDirectory;
            App.AppAttributes["AppExecFilePath"] = App.AppAttributes["AppPath"] + "\\"+App.AppAttributes["AppName"]+".exe";
            App.AppAttributes["AppConfigFilePath"]= App.AppAttributes["AppPath"] + "\\Resx\\config.json";
            //初始化首选项
            if( File.Exists( App.AppAttributes["AppConfigFilePath"]) ) {//未曾初始化
                String strConfig="";
                var a = File.OpenText( App.AppAttributes["AppConfigFilePath"] ).ReadToEndAsync();
                strConfig = a.Result;
                MessageBox.Show(strConfig);
            } else {//已初始化过
                MessageBox.Show( "not found config.json", App.AppAttributes["AppName"] ); System.Environment.Exit( 0 );
            }
            base.OnStartup( e );
        }

    }
}
