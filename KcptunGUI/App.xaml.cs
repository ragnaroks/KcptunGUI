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


        protected override void OnStartup( StartupEventArgs e ) {
            //检查环境
            if( !Directory.Exists( Class.AppAttributes.ResxPath ) ) { Directory.CreateDirectory( Class.AppAttributes.ResxPath ); }//若resx目录不存在则创建
            if( !Directory.Exists( Class.AppAttributes.I18NPath ) ) { Directory.CreateDirectory( Class.AppAttributes.I18NPath ); }//若i18n目录不存在则创建
            //初始化鼠标样式
            AppCursor[0] = new Cursor( Class.Functions.BytesToStream( KcptunGUI.Properties.Resources.cursor_Arrow ) , false);//箭头
            //初始化首选项
            if(!File.Exists( Class.AppAttributes.ConfigFilePath ) ) {
                File.Create( Class.AppAttributes.ConfigFilePath );//若配置文件不存在则创建
                File.WriteAllText( Class.AppAttributes.ConfigFilePath , JsonConvert.SerializeObject( Class.LocalFunction.GenDefaultConfigObject() ) , Class.AppAttributes.UTF8EncodingNoBom );//写入默认配置
            }
            AppConfigJson = File.ReadAllText(Class.AppAttributes.ConfigFilePath,Class.AppAttributes.UTF8EncodingNoBom);
            //配置文件有效性验证
            if( !Class.LocalFunction.ValidateJSON() ) {//验证失败
                if(MessageBox.Show( "配置文件无效,是否重建配置文件?\nConfigure file is invalid,resetup?" , Class.AppAttributes.Name, MessageBoxButton.YesNo , MessageBoxImage.Question ) == MessageBoxResult.Yes ) {
                    File.WriteAllText( Class.AppAttributes.ConfigFilePath , JsonConvert.SerializeObject( Class.LocalFunction.GenDefaultConfigObject() ) , Class.AppAttributes.UTF8EncodingNoBom );//写入默认配置
                    MessageBox.Show("配置文件重建完成\nConfigure file setup." , Class.AppAttributes.Name , MessageBoxButton.OK, MessageBoxImage.Information );
                } else {Environment.Exit( 0 );}
            } else {//验证成功
                AppConfigObject = JsonConvert.DeserializeObject<Class.ConfigJson>(AppConfigJson); //Console.WriteLine( "AppConfigObject_String: "+JsonConvert.SerializeObject( AppConfigObject ) ); System.Threading.Thread.Sleep( 99999 );
            }
            //初始化语言文件
            if( File.Exists( Class.AppAttributes.I18NPath + App.AppConfigObject.LCID + ".json" ) ) {//语言文件存在
                //MessageBox.Show( "语言文件存在" );

            }else {
                MessageBox.Show( "语言文件不存在,将使用简体中文\nNot found language file,default use chinese.", Class.AppAttributes.Name, MessageBoxButton.OK,MessageBoxImage.Information);
            }
            base.OnStartup( e );
        }
    }
}
