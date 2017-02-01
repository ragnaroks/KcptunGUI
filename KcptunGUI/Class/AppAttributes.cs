using System;
using System.Windows;
using System.IO;
using System.Windows.Interop;
using System.Windows.Controls;
using System.ComponentModel;
using System.Text;

namespace KcptunGUI.Class {
    class AppAttributes {
        public static String Name = "KcptunGUI";
        public const String FileName = "KcptunGUI.exe";
        public static String Version{get{return _Version.ToString();}}
        private static StringBuilder _Version = new StringBuilder().AppendFormat(
            "{0}.{1}.{2}({3})",
            Application.ResourceAssembly.GetName().Version.Major,
            Application.ResourceAssembly.GetName().Version.Minor,
            Application.ResourceAssembly.GetName().Version.Build,
            Application.ResourceAssembly.GetName().Version.Revision
        );
        public static readonly String RunPath = Environment.CurrentDirectory;//运行目录绝对路径,无路径分隔符
        public static readonly String FilePath = AppDomain.CurrentDomain.BaseDirectory;//程序集绝对路径
        public static readonly String ExecFilePath = Path.Combine(FilePath,FileName);//可执行文件绝对路径
        public static readonly String ResxPath = AppAttributes.RunPath + "Resx\\";
        public static readonly String I18NPath = AppAttributes.ResxPath + "I18N\\";
        public static readonly String ConfigFilePath = AppAttributes.ResxPath + "config.json";
        public static readonly System.Text.UTF8Encoding UTF8EncodingNoBom = new System.Text.UTF8Encoding(false , true);//无BOM编码
    }
}
