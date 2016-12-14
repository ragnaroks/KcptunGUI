using System;
using System.Windows;
using System.Collections.Generic;

namespace KcptunGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application{
        public static Dictionary<String , String> AppSettings = new Dictionary<String , String>();
        protected override void OnStartup( StartupEventArgs e ) {
            App.AppSettings["AppName"] = "KcptunGUI";
            App.AppSettings["AppVersion"] = Application.ResourceAssembly.GetName().Version.Major.ToString() + "." + Application.ResourceAssembly.GetName().Version.Minor.ToString() + "." + Application.ResourceAssembly.GetName().Version.Build.ToString();
            App.AppSettings["AppVersionR"] = Application.ResourceAssembly.GetName().Version.Revision.ToString();
            App.AppSettings["AppPath"] = Environment.CurrentDirectory;
            App.AppSettings["AppExecFilePath"] = App.AppSettings["AppPath"]+ "\\"+App.AppSettings["AppName"]+".exe";
            base.OnStartup( e );
        }

    }
}
