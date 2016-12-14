using System;
using System.Windows;
using System.Collections.Generic;

namespace KcptunGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application{
        public static Dictionary<String , Object> AppSettings = new Dictionary<String , Object>();
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();
        protected override void OnStartup( StartupEventArgs e ) {
            App.AppSettings["AppName"] = "KcptunGUI";
            App.AppSettings["AppVersion"] = Application.ResourceAssembly.GetName().Version.Major.ToString() + "." + Application.ResourceAssembly.GetName().Version.Minor.ToString() + "." + Application.ResourceAssembly.GetName().Version.Build.ToString();
            App.AppSettings["AppVersionR"] = Application.ResourceAssembly.GetName().Version.Revision.ToString();
            App.AppSettings["AppPath"] = Environment.CurrentDirectory;
            App.AppSettings["AppExecFilePath"] = App.AppSettings["AppPath"]+ "\\"+App.AppSettings["AppName"]+".exe";
            App.AppSettings["AppConfig"] = FadeJson.JsonValue.FromFile( App.AppSettings["AppPath"]+"\\Resx\\config.json" );//return json object
            base.OnStartup( e );
        }

    }
}
