using System;
using System.Windows;
using System.IO;
using System.Windows.Interop;
using System.Windows.Controls;
using System.ComponentModel;

namespace KcptunGUI.Class {
    class AppAttributes: INotifyPropertyChanged {
        public static readonly String Name = "KcptunGUI";
        public static readonly String Version = Application.ResourceAssembly.GetName().Version.Major + "." + Application.ResourceAssembly.GetName().Version.Minor + "." + Application.ResourceAssembly.GetName().Version.Build;
        public static readonly String Build = Application.ResourceAssembly.GetName().Version.Build.ToString();
        public static readonly String RunPath = Environment.CurrentDirectory + "\\";//运行目录绝对路径
        public static readonly String ExecFilePath = AppAttributes.RunPath + AppAttributes.Name + ".exe";//可执行文件绝对路径
        public static readonly String ResxPath = AppAttributes.RunPath + "Resx\\";
        public static readonly String I18NPath = AppAttributes.ResxPath + "I18N\\";
        public static readonly String ConfigFilePath = AppAttributes.ResxPath + "config.json";
        public static readonly System.Text.UTF8Encoding UTF8EncodingNoBom = new System.Text.UTF8Encoding(false , true);//无BOM编码

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add {
                
            }

            remove {
                
            }
        }

    }
}
