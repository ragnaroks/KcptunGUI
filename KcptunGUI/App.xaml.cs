using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace KcptunGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application{
        public static Dictionary<String ,String> AppAttributes = new Dictionary<String ,String>();//应用属性
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();//托盘图标
        public static Object AppConfig = new Object();
        public static FileStream fsAppConfigFile = null;
        protected override void OnStartup( StartupEventArgs e ) {
            //应用属性
            App.AppAttributes["AppName"] = "KcptunGUI";
            App.AppAttributes["AppVersion"] = Application.ResourceAssembly.GetName().Version.Major.ToString() + "." + Application.ResourceAssembly.GetName().Version.Minor.ToString() + "." + Application.ResourceAssembly.GetName().Version.Build.ToString();
            App.AppAttributes["AppVersionR"] = Application.ResourceAssembly.GetName().Version.Revision.ToString();
            App.AppAttributes["AppPath"] = Environment.CurrentDirectory;
            App.AppAttributes["AppExecFilePath"] = App.AppAttributes["AppPath"] + "\\"+App.AppAttributes["AppName"]+".exe";
            App.AppAttributes["AppConfigFilePath"]= App.AppAttributes["AppPath"] + "\\Resx\\config.json";
            App.AppAttributes["AppConfigVaild"] = "false";
            //初始化首选项
            if( File.Exists( App.AppAttributes["AppConfigFilePath"]) ) {//文件存在
                String strConfig = Class.Functions.TrimString( File.OpenText( App.AppAttributes["AppConfigFilePath"] ).ReadToEndAsync().Result , true );
                

            } else {//文件不存在
                MessageBox.Show( "Not found \"config.josn\",will create and restore settings.", App.AppAttributes["AppName"] ,MessageBoxButton.OK,MessageBoxImage.Warning);
                fsAppConfigFile = new FileStream( App.AppAttributes["AppConfigFilePath"] ,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None);
                SubWindow.PreConfig pc = new SubWindow.PreConfig() {
                    ShowActivated = true , ShowInTaskbar = false , ShowSystemMenuOnRightClick = false , ShowIconOnTitleBar = true,
                    TitleCharacterCasing = System.Windows.Controls.CharacterCasing.Normal
                };
                pc.ShowDialog();
                if( pc.DialogResult == true ) {//成功配置

                }else {//配置失败或放弃配置
                    fsAppConfigFile.Dispose(); fsAppConfigFile.Close();//释放文件
                    MessageBox.Show( "Invaild \"config.json\",exit." , App.AppAttributes["AppName"] , MessageBoxButton.OK , MessageBoxImage.Warning );
                    if( File.Exists( App.AppAttributes["AppConfigFilePath"] ) ) { File.Delete( App.AppAttributes["AppConfigFilePath"] ); }
                }
            }
            base.OnStartup( e );
        }

    }
}
