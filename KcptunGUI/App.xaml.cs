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
        public static Dictionary<String , Object> AppSettings = new Dictionary<String , Object>();
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();
        protected override void OnStartup( StartupEventArgs e ) {
            App.AppSettings["AppName"] = "KcptunGUI";
            App.AppSettings["AppVersion"] = Application.ResourceAssembly.GetName().Version.Major.ToString() + "." + Application.ResourceAssembly.GetName().Version.Minor.ToString() + "." + Application.ResourceAssembly.GetName().Version.Build.ToString();
            App.AppSettings["AppVersionR"] = Application.ResourceAssembly.GetName().Version.Revision.ToString();
            App.AppSettings["AppPath"] = Environment.CurrentDirectory;
            App.AppSettings["AppExecFilePath"] = App.AppSettings["AppPath"]+ "\\"+App.AppSettings["AppName"]+".exe";
            App.AppSettings["AppConfigFilePath"]= App.AppSettings["AppPath"] + "\\Resx\\config.json";
            
            App.AppSettings["AppConfig"] = new FadeJSON.JsonObject(true);
            if( File.Exists( App.AppSettings["AppConfigFilePath"].ToString() ) ) {
                App.AppSettings["AppConfig"] = new FadeJSON.StreamParser( App.AppSettings["AppPath"] + "\\Resx\\config.json" );//return json object
            }else {
                MessageBox.Show( "not found config.json", App.AppSettings["AppName"].ToString() ); System.Environment.Exit( 0 );
            }

            App.AppSettings["AppI18N"] = new Object();
            base.OnStartup( e );
        }

    }
}
