using System;
using System.Windows;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;

namespace KcptunGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application{
        public static String AppConfigJson=String.Empty;
        public static Class.ConfigJson AppConfigObject;
        public static Class.LanguageJson AppLanguageObject;
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();//托盘图标
        public static Cursor[] AppCursor = new Cursor[2];

        protected override void OnStartup( StartupEventArgs e ) {
            //环境检查
            if(!File.Exists(Class.AppAttributes.ConfigFilePath)){//若配置文件不存在
                MessageBox.Show("配置文件不存在(Not found config.json file)","错误(Error)",MessageBoxButton.OK,MessageBoxImage.Error);
                Environment.Exit(0);
            }else{
                AppConfigJson = File.ReadAllText(Class.AppAttributes.ConfigFilePath,Class.AppAttributes.UTF8EncodingNoBom);//读取配置文件字符串
                if(!Class.LocalFunction.ValidateConfigJSON()){//若配置文件无效
                    if(MessageBox.Show("配置文件无效,是否重建配置文件?(Configure file is invalid,set up?)",Class.AppAttributes.Name,MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes) {
                        File.WriteAllText(Class.AppAttributes.ConfigFilePath,KcptunGUI.Properties.Resources.String_AppConfigJsonDefault,Class.AppAttributes.UTF8EncodingNoBom);//写入默认配置
                        MessageBox.Show("配置文件重建完成(Configure file is set up)",Class.AppAttributes.Name,MessageBoxButton.OK,MessageBoxImage.Information);
                        AppConfigJson = File.ReadAllText(Class.AppAttributes.ConfigFilePath,Class.AppAttributes.UTF8EncodingNoBom);//重读配置文件字符串
                    } else { Environment.Exit(0); }
                }
                AppConfigObject = JsonConvert.DeserializeObject<Class.ConfigJson>(AppConfigJson);//加载配置文件
            }
            //if(!Directory.Exists(Class.AppAttributes.ResxPath)){ MessageBox.Show("Resx目录不存在(Not found Resx folder)","错误(Error)",MessageBoxButton.OK,MessageBoxImage.Error); Environment.Exit(0); }//若resx目录不存在
            AppLanguageObject = Class.I18N.LoadLanguageObjectFromJSON(App.AppConfigObject.LCID);
            AppCursor[0]=new Cursor(Class.Functions.BytesToStream(Resource.Byte.cur_normal));//默认鼠标样式
            //
            base.OnStartup( e );
        }
    }
}
