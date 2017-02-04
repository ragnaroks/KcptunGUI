using System;
using System.Windows;
using System.IO;

namespace KcptunGUI.Class {
    class AppAttributes {
        public const String Name = "KcptunGUI";
        public const String FileName = "KcptunGUI.exe";
        public static readonly String Version = String.Format(
            "{0}.{1}.{2}({3})" ,
            Application.ResourceAssembly.GetName().Version.Major ,
            Application.ResourceAssembly.GetName().Version.Minor ,
            Application.ResourceAssembly.GetName().Version.Build ,
            Application.ResourceAssembly.GetName().Version.Revision
        );
        public static readonly String RunPath = Environment.CurrentDirectory;//运行目录,无路径分隔符,默认是D:\My Projects\KcptunGUI\KcptunGUI\bin\Debug\x86\
        public static readonly String FilePath = AppDomain.CurrentDomain.BaseDirectory;//程序集目录,有路径分隔符,D:\My Projects\KcptunGUI\KcptunGUI\bin\Debug\x86\
        public static readonly String ExecFilePath = Path.Combine(FilePath,FileName);//可执行文件,D:\My Projects\KcptunGUI\KcptunGUI\bin\Debug\x86\KcptunGUI.exe
        public static readonly String ResxPath = Path.Combine(FilePath , "Resx");//资源目录
        public static readonly String I18NPath = Path.Combine(ResxPath , "I18N");//语言文件
        public static readonly String ConfigFilePath = Path.Combine(ResxPath ,"config.json");//配置文件
        public static readonly System.Text.UTF8Encoding UTF8EncodingNoBom = new System.Text.UTF8Encoding(false , true);//无BOM编码
    }
}
