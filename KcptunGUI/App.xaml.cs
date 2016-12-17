using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace KcptunGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application{
        public static Dictionary<String ,String> AppAttributes = new Dictionary<String ,String>();//应用属性
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();//托盘图标
        public static Object AppConfig = new Object();
        public static FileStream fsAppConfigFile = null;//配置文件
        public static Cursor[] AppCursor = new Cursor[2];
        protected override void OnStartup( StartupEventArgs e ) {
            //应用属性
            App.AppAttributes["Name"] = "KcptunGUI";
            App.AppAttributes["Version"] = Application.ResourceAssembly.GetName().Version.Major.ToString() + "." + Application.ResourceAssembly.GetName().Version.Minor.ToString() + "." + Application.ResourceAssembly.GetName().Version.Build.ToString();
            App.AppAttributes["VersionR"] = Application.ResourceAssembly.GetName().Version.Revision.ToString();
            App.AppAttributes["Path"] = Environment.CurrentDirectory;
            App.AppAttributes["ExecFilePath"] = App.AppAttributes["Path"] + "\\"+App.AppAttributes["Name"]+".exe";
            App.AppAttributes["ResxPath"] = App.AppAttributes["Path"] + "\\Resx";
            App.AppAttributes["ConfigFilePath"]= App.AppAttributes["ResxPath"] + "\\config.json";
            App.AppAttributes["ConfigVaild"] = "false";
            //必备环境
            if( !Directory.Exists( App.AppAttributes["ResxPath"] ) ) { Directory.CreateDirectory( App.AppAttributes["ResxPath"] ); }//若resx目录不存在则创建
            //初始化鼠标样式
            App.AppCursor[0] = new Cursor( Class.Functions.BytesToStream( KcptunGUI.Properties.Resources.cursor_Arrow ) , false);//箭头
            //初始化首选项
            if( File.Exists( App.AppAttributes["ConfigFilePath"]) ) {//配置文件存在
                String strConfig = Class.Functions.TrimString( File.OpenText( App.AppAttributes["ConfigFilePath"] ).ReadToEndAsync().Result , true );
                MessageBox.Show(strConfig);
            } else {//配置文件不存在
                MessageBox.Show( "配置文件不存在,将重新创建", App.AppAttributes["Name"] ,MessageBoxButton.OK,MessageBoxImage.Warning);
                fsAppConfigFile = new FileStream( App.AppAttributes["ConfigFilePath"] ,FileMode.OpenOrCreate,FileAccess.ReadWrite,FileShare.None);
                SubWindow.PreConfig pc = new SubWindow.PreConfig() {
                    ShowActivated = true , ShowInTaskbar = false , ShowSystemMenuOnRightClick = false , ShowIconOnTitleBar = true ,
                    TitleCharacterCasing = System.Windows.Controls.CharacterCasing.Normal
                };
                pc.ShowDialog();
                if( pc.DialogResult == true ) {//成功配置
                    MessageBox.Show("配置成功");
                }else {//配置失败或放弃配置
                    fsAppConfigFile.Dispose(); fsAppConfigFile.Close();//释放文件
                    if( File.Exists( App.AppAttributes["ConfigFilePath"] ) ) { File.Delete( App.AppAttributes["ConfigFilePath"] ); }
                    MessageBox.Show( "无有效配置文件,强制退出" , App.AppAttributes["Name"] , MessageBoxButton.OK , MessageBoxImage.Warning );
                    Environment.Exit( 0 );
                }
            }
            base.OnStartup( e );
        }

    }
}
