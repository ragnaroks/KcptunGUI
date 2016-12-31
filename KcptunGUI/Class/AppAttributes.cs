using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KcptunGUI.Class {
    class AppAttributes {
        public static readonly String Name = "KcptunGUI";
        public static readonly String Version = System.Windows.Application.ResourceAssembly.GetName().Version.Major + "." + System.Windows.Application.ResourceAssembly.GetName().Version.Minor;
        public static readonly String Build = System.Windows.Application.ResourceAssembly.GetName().Version.Build.ToString();
        public static readonly String RunPath = Environment.CurrentDirectory + "\\";//运行目录绝对路径
        public static readonly String ExecFilePath = AppAttributes.RunPath + AppAttributes.Name + ".exe";//可执行文件绝对路径
        public static readonly String ResxPath = AppAttributes.RunPath + "Resx\\";
        public static readonly String I18NPath = AppAttributes.ResxPath + "I18N\\";
        public static readonly String ConfigFilePath = AppAttributes.ResxPath + "config.json";
        public static readonly System.Text.UTF8Encoding UTF8EncodingNoBom = new System.Text.UTF8Encoding( false , true );//无BOM编码
    }
}
