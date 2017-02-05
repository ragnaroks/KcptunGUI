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
        public static String AppConfigJson=String.Empty;
        public static Class.ConfigJson AppConfigObject;
        public static String AppLanguageJson = String.Empty;
        public static Class.LanguageJson AppLanguageObject;
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();//托盘图标
        public static Cursor[] AppCursor = new Cursor[2];

        protected override void OnStartup( StartupEventArgs e ) {
            //检查环境
            if( !Directory.Exists( Class.AppAttributes.ResxPath ) ) { Directory.CreateDirectory( Class.AppAttributes.ResxPath ); }//若resx目录不存在则创建
            if( !Directory.Exists( Class.AppAttributes.I18NPath ) ) { Directory.CreateDirectory( Class.AppAttributes.I18NPath ); }//若i18n目录不存在则创建
            /*
            if(!File.Exists(Path.Combine(Class.AppAttributes.I18NPath,"2052.json")) || !File.Exists(Path.Combine(Class.AppAttributes.I18NPath ,"1033.json"))) {
                File.WriteAllText(Path.Combine(Class.AppAttributes.I18NPath , "2052.json") , KcptunGUI.Properties.Resources.String_AppLanguageJsonDefault,Class.AppAttributes.UTF8EncodingNoBom);//写入默认语言文件
            }
            */
            //初始化配置文件
            if(!File.Exists( Class.AppAttributes.ConfigFilePath ) ) {
                FileStream fs=File.Create( Class.AppAttributes.ConfigFilePath );//若配置文件不存在则创建
                fs.Close(); fs.Dispose(); //File.WriteAllText(Class.AppAttributes.ConfigFilePath , KcptunGUI.Properties.Resources.String_AppConfigJsonDefault , Class.AppAttributes.UTF8EncodingNoBom);//写入默认配置
            }
            AppConfigJson = File.ReadAllText(Class.AppAttributes.ConfigFilePath,Class.AppAttributes.UTF8EncodingNoBom);
            //配置文件有效性验证
            if( !Class.LocalFunction.ValidateConfigJSON() ) {//验证失败
                if(MessageBox.Show( "配置文件无效,是否重建配置文件?\nConfigure file is invalid,set up?" , Class.AppAttributes.Name, MessageBoxButton.YesNo , MessageBoxImage.Question ) == MessageBoxResult.Yes ) {
                    File.WriteAllText( Class.AppAttributes.ConfigFilePath , KcptunGUI.Properties.Resources.String_AppConfigJsonDefault , Class.AppAttributes.UTF8EncodingNoBom );//写入默认配置
                    MessageBox.Show("配置文件重建完成\nConfigure file is set up." , Class.AppAttributes.Name , MessageBoxButton.OK, MessageBoxImage.Information );
                    AppConfigJson = File.ReadAllText( Class.AppAttributes.ConfigFilePath , Class.AppAttributes.UTF8EncodingNoBom );
                    AppConfigObject = JsonConvert.DeserializeObject<Class.ConfigJson>( AppConfigJson );
                } else {Environment.Exit( 0 );}
            } else {//验证成功
                AppConfigObject = JsonConvert.DeserializeObject<Class.ConfigJson>(AppConfigJson);
            }
            //初始化语言文件
            if( !File.Exists(Path.Combine(Class.AppAttributes.I18NPath,App.AppConfigObject.LCID+".json")) ) {//语言文件不存在
                MessageBox.Show( "语言文件不存在,将使用简体中文\nNot found language file,default use schinese.", Class.AppAttributes.Name, MessageBoxButton.OK,MessageBoxImage.Information);
                AppLanguageJson = KcptunGUI.Properties.Resources.String_AppLanguageJsonDefault;
            }else {
                AppLanguageJson = File.ReadAllText(Path.Combine(Class.AppAttributes.I18NPath , App.AppConfigObject.LCID + ".json"));
            }
            AppLanguageObject = JsonConvert.DeserializeObject<Class.LanguageJson>(AppLanguageJson);
            //鼠标样式
            AppCursor[0]=new Cursor(Class.Functions.BytesToStream(Resource.Byte.cur_normal));
            //
            base.OnStartup( e );
        }
    }
}
