using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Newtonsoft.Json;

namespace KcptunGUI.Class {
    class I18N {
        /// <summary>通过指定LCID来加载对应的语言文件</summary>
        /// <param name="_LCID">LCID</param>
        public static void LoadLanguageJsonFile(Int32 _LCID) {
            if( !File.Exists(AppAttributes.I18NPath + App.AppConfigObject.LCID + ".json") ) {//语言文件不存在
                MessageBox.Show("语言文件不存在,将使用简体中文\nNot found language file,default use chinese." ,AppAttributes.Name , MessageBoxButton.OK , MessageBoxImage.Information);
                App.AppLanguageJson = Properties.Resources.String_AppLanguageJsonDefault;
            } else {
                App.AppLanguageJson = File.ReadAllText(AppAttributes.I18NPath + App.AppConfigObject.LCID + ".json");
                App.AppLanguageObject = JsonConvert.DeserializeObject<LanguageJson>(App.AppLanguageJson);
            }
        }
        
        public static String GetString(Object _SenderTag) {
            Int32 _index;
            if( Int32.TryParse( ( (String)_SenderTag ).Replace( "I18N_" , "" ) ,out _index ) && _index<App.AppLanguageObject.Text.Count ) {
                return App.AppLanguageObject.Text[_index];
            } else {
                return App.AppLanguageObject.Language+" - "+App.AppLanguageObject.LangTag+" - "+App.AppLanguageObject.LCID;
            }
        }
    }
}
