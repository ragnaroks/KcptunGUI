using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KcptunGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application{
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();//托盘图标
        public static FileStream ConfigStream = null;
        public static StreamWriter ConfigWriter = null;//配置文件_写入
        public static StreamReader ConfigReader = null;//配置文件_读取
        public static Cursor[] AppCursor = new Cursor[2];
        public static String AppConfigJson;
        public static Class.ConfigJson AppConfigObject;
        public static String AppLanguageJson;
        public static Class.LanguageJson AppLanguageObject;

        protected override void OnStartup( StartupEventArgs e ) {
            //检查环境
            if( !Directory.Exists( Class.AppAttributes.ResxPath ) ) { Directory.CreateDirectory( Class.AppAttributes.ResxPath ); }//若resx目录不存在则创建
            if( !Directory.Exists( Class.AppAttributes.I18NPath ) ) { Directory.CreateDirectory( Class.AppAttributes.I18NPath ); }//若i18n目录不存在则创建
            if(!File.Exists( Class.AppAttributes.I18NPath+"2052.json") || !File.Exists( Class.AppAttributes.I18NPath+"1033.json") ) { Environment.Exit( 0 ); }//没有默认语言文件直接退出
            //初始化鼠标样式
            AppCursor[0] = new Cursor( Class.Functions.BytesToStream( KcptunGUI.Properties.Resources.cursor_Arrow ) , false);//箭头
            //初始化配置文件
            if(!File.Exists( Class.AppAttributes.ConfigFilePath ) ) {
                File.Create( Class.AppAttributes.ConfigFilePath );//若配置文件不存在则创建
                File.WriteAllText(Class.AppAttributes.ConfigFilePath , KcptunGUI.Properties.Resources.String_AppConfigJsonDefault , Class.AppAttributes.UTF8EncodingNoBom);//写入默认配置
            }
            AppConfigJson = File.ReadAllText(Class.AppAttributes.ConfigFilePath,Class.AppAttributes.UTF8EncodingNoBom);
            Console.WriteLine(KcptunGUI.Properties.Resources.String_AppConfigJsonDefault);// System.Threading.Thread.Sleep(99999);
            //配置文件有效性验证
            if( !Class.LocalFunction.ValidateConfigJSON() ) {//验证失败
                if(MessageBox.Show( "配置文件无效,是否重建配置文件?\nConfigure file is invalid,set up?" , Class.AppAttributes.Name, MessageBoxButton.YesNo , MessageBoxImage.Question ) == MessageBoxResult.Yes ) {
                    File.WriteAllText( Class.AppAttributes.ConfigFilePath , KcptunGUI.Properties.Resources.String_AppConfigJsonDefault , Class.AppAttributes.UTF8EncodingNoBom );//写入默认配置
                    MessageBox.Show("配置文件重建完成\nConfigure file is set up." , Class.AppAttributes.Name , MessageBoxButton.OK, MessageBoxImage.Information );
                    AppConfigJson = File.ReadAllText( Class.AppAttributes.ConfigFilePath , Class.AppAttributes.UTF8EncodingNoBom );
                    AppConfigObject = JsonConvert.DeserializeObject<Class.ConfigJson>( AppConfigJson );
                } else {Environment.Exit( 0 );}
            } else {//验证成功
                AppConfigObject = JsonConvert.DeserializeObject<Class.ConfigJson>(AppConfigJson); //Console.WriteLine( "AppConfigObject_String: "+JsonConvert.SerializeObject( AppConfigObject ) ); System.Threading.Thread.Sleep( 99999 );
            }
            //初始化语言文件
            if( !File.Exists( Class.AppAttributes.I18NPath + App.AppConfigObject.LCID + ".json" ) ) {//语言文件不存在
                MessageBox.Show( "语言文件不存在,将使用简体中文\nNot found language file,default use chinese.", Class.AppAttributes.Name, MessageBoxButton.OK,MessageBoxImage.Information);
                AppLanguageJson = KcptunGUI.Properties.Resources.String_AppLanguageJsonDefault;
            }else {
                AppLanguageJson = File.ReadAllText( Class.AppAttributes.I18NPath + App.AppConfigObject.LCID + ".json");
                AppLanguageObject = JsonConvert.DeserializeObject<Class.LanguageJson>( AppLanguageJson );
            }
            //终于可以启动了
            base.OnStartup( e );
        }
    }
}
