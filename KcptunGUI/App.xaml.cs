using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using LitJson;

namespace KcptunGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application{
        public static Dictionary<String ,String> AppAttributes = new Dictionary<String ,String>();//应用属性
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();//托盘图标
        public static FileStream ConfigStream = null;
        public static StreamWriter ConfigWriter = null;//配置文件_写入
        public static StreamReader ConfigReader = null;//配置文件_读取
        public static Cursor[] AppCursor = new Cursor[2];
        public static System.Text.UTF8Encoding UTF8EncodingNoBom = new System.Text.UTF8Encoding( false,true);//无BOM编码
        public const String DefaultConfig = "{\"AutoConnect\":false,\"LCID\":2052,\"ClientNodes\":{}}";
        public static readonly Dictionary<String,JsonType> ConfigFormat = new Dictionary<String,JsonType>(){
            { "AutoConnect" , JsonType.Boolean },
            { "LCID" , JsonType.Int },
            { "ClientNodes" , JsonType.Object },//{ "ServerNodes" , JsonType.None }
        };
        public static JsonData AppConfig;

        protected override void OnStartup( StartupEventArgs e ) {
            //应用属性
            App.AppAttributes["Name"] = "KcptunGUI";
            App.AppAttributes["Version"] = Application.ResourceAssembly.GetName().Version.Major.ToString() + "." + Application.ResourceAssembly.GetName().Version.Minor.ToString() + "." + Application.ResourceAssembly.GetName().Version.Build.ToString();
            App.AppAttributes["VersionR"] = Application.ResourceAssembly.GetName().Version.Revision.ToString();
            App.AppAttributes["Path"] = Environment.CurrentDirectory;
            App.AppAttributes["ExecFilePath"] = App.AppAttributes["Path"] + "\\"+App.AppAttributes["Name"]+".exe";
            App.AppAttributes["ResxPath"] = App.AppAttributes["Path"] + "\\Resx";
            App.AppAttributes["I18NPath"] = App.AppAttributes["Path"] + "\\I18N";
            App.AppAttributes["ConfigFilePath"]= App.AppAttributes["ResxPath"] + "\\config.json";
            //必备环境
            if( !Directory.Exists( App.AppAttributes["ResxPath"] ) ) { Directory.CreateDirectory( App.AppAttributes["ResxPath"] ); }//若resx目录不存在则创建
            if( !Directory.Exists( App.AppAttributes["I18NPath"] ) ) { Directory.CreateDirectory( App.AppAttributes["I18NPath"] ); }//若resx目录不存在则创建
            //初始化鼠标样式
            App.AppCursor[0] = new Cursor( Class.Functions.BytesToStream( KcptunGUI.Properties.Resources.cursor_Arrow ) , false);//箭头
            //初始化首选项
            App.ConfigStream = new FileStream( App.AppAttributes["ConfigFilePath"] , FileMode.OpenOrCreate , FileAccess.ReadWrite , FileShare.None );//若配置文件不存在则创建并锁定
            if( !App.ConfigStream.CanRead || !App.ConfigStream.CanWrite || !App.ConfigStream.CanSeek ) { throw new Exception( "无法操作配置文件" ); }
            App.ConfigWriter = new StreamWriter( App.ConfigStream , UTF8EncodingNoBom ) { AutoFlush = true };
            App.ConfigReader = new StreamReader( App.ConfigStream , UTF8EncodingNoBom , false );//强制UTF8读取
            Int32 a = Class.LocalFunction.ValidateJSON( App.ConfigReader.ReadToEnd() , App.ConfigFormat );
            switch( a ) {
                default:
                case Int32.MaxValue:
                    if( MessageBox.Show( "配置文件无效,是否重建配置文件?" , App.AppAttributes["Name"] , MessageBoxButton.YesNo , MessageBoxImage.Question ) == MessageBoxResult.Yes ) {
                        App.ConfigWriter.Write(App.DefaultConfig); App.ConfigWriter.Flush();
                    } else {
                        Environment.Exit( 0 );
                    }
                    break;
                case 0:
                    //none
                    break;
            }
            App.AppConfig = JsonMapper.ToObject(new JsonReader( App.ConfigReader.ReadToEnd() ) );
            //初始化语言文件
            if( File.Exists( App.AppAttributes["I18NPath"] + App.AppConfig["LCID"].ToString() + ".json" ) ) {//语言文件存在
                MessageBox.Show( "语言文件存在" );
            }else {
                MessageBox.Show( "语言文件不存在" );
            }
            base.OnStartup( e );
        }

    }
}
