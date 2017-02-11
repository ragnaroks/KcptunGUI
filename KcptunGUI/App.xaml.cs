using System;
using System.Windows;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Diagnostics;

namespace KcptunGUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application{
        public const String AppName = "KcptunGUI";
        public const String AppFileName = "KcptunGUI.exe";
        public static readonly String AppVersion = String.Format(
            "{0}.{1}.{2}({3})",
            Application.ResourceAssembly.GetName().Version.Major,
            Application.ResourceAssembly.GetName().Version.Minor,
            Application.ResourceAssembly.GetName().Version.Build,
            Application.ResourceAssembly.GetName().Version.Revision
        );
        public static readonly String AppRunPath = Environment.CurrentDirectory;//运行目录,无路径分隔符,默认是D:\My Projects\KcptunGUI\KcptunGUI\bin\Debug\x86
        public static readonly String AppFilePath = AppDomain.CurrentDomain.BaseDirectory;//程序集目录,有路径分隔符,D:\My Projects\KcptunGUI\KcptunGUI\bin\Debug\x86\
        public static readonly String AppExecFilePath = Path.Combine(AppFilePath,AppFileName);//可执行文件,D:\My Projects\KcptunGUI\KcptunGUI\bin\Debug\x86\KcptunGUI.exe
        public static readonly String AppResxPath = Path.Combine(AppFilePath,"Resx");//资源目录,D:\My Projects\KcptunGUI\KcptunGUI\bin\Debug\x86\Resx
        public static readonly String AppConfigFilePath = Path.Combine(AppFilePath,"config.json");//配置文件,D:\My Projects\KcptunGUI\KcptunGUI\bin\Debug\x86\config.json
        public static readonly System.Text.UTF8Encoding UTF8EncodingNoBom = new System.Text.UTF8Encoding(false,true);//无BOM编码
        public static Class.ConfigJson AppConfigObject;
        public static Class.LanguageJson AppLanguageObject;
        public static System.Windows.Forms.NotifyIcon nicon = new System.Windows.Forms.NotifyIcon();//托盘图标
        public static Cursor[] AppCursor = new Cursor[2];
        public static readonly Process AppProcess=Process.GetCurrentProcess();

        protected override void OnStartup( StartupEventArgs e ) {
            String AppConfigJson = String.Empty;
            //环境检查
            if(!File.Exists(AppConfigFilePath)){//若配置文件不存在
                MessageBox.Show("配置文件不存在(Not found config.json file)","错误(Error)",MessageBoxButton.OK,MessageBoxImage.Error);
                Environment.Exit(0);
            }else{
                AppConfigJson = File.ReadAllText( AppConfigFilePath,App.UTF8EncodingNoBom);//读取配置文件字符串
                if(!Class.LocalFunction.ValidateJSONBySchema(AppConfigJson,KcptunGUI.Properties.Resources.String_AppConfigJsonSchema)){//若配置文件无效
                    if(MessageBox.Show("配置文件无效,是否重建配置文件?(Configure file is invalid,set up?)",App.AppName,MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes) {
                        File.WriteAllText(AppConfigFilePath,KcptunGUI.Properties.Resources.String_AppConfigJsonDefault,UTF8EncodingNoBom);//写入默认配置
                        MessageBox.Show("配置文件重建完成(Configure file is set up)",AppName,MessageBoxButton.OK,MessageBoxImage.Information);
                        AppConfigJson = File.ReadAllText(AppConfigFilePath,UTF8EncodingNoBom);//重读配置文件字符串
                    } else { Environment.Exit(0); }
                }
                AppConfigObject = JsonConvert.DeserializeObject<Class.ConfigJson>(AppConfigJson);//加载配置文件
            }
            //if(!Directory.Exists(Class.AppAttributes.ResxPath)){ MessageBox.Show("Resx目录不存在(Not found Resx folder)","错误(Error)",MessageBoxButton.OK,MessageBoxImage.Error); Environment.Exit(0); }//若resx目录不存在
            AppLanguageObject = Class.LocalFunction.LoadLanguageObjectFromJSON(App.AppConfigObject.LCID);
            AppCursor[0]=new Cursor(Class.LocalFunction.BytesToStream(Resource.Byte.cur_normal));//默认鼠标样式
            //
            base.OnStartup( e );
        }
    }
}
