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
            if( !File.Exists(Path.Combine(AppAttributes.I18NPath , App.AppConfigObject.LCID + ".json"))) {//语言文件不存在
                MessageBox.Show("语言文件不存在,将使用简体中文\nNot found language file,default use chinese." ,AppAttributes.Name , MessageBoxButton.OK , MessageBoxImage.Information);
                App.AppLanguageJson = Properties.Resources.String_AppLanguageJsonDefault;
            } else {
                App.AppLanguageJson = File.ReadAllText(Path.Combine(AppAttributes.I18NPath , App.AppConfigObject.LCID + ".json"));
            }
            App.AppLanguageObject = JsonConvert.DeserializeObject<LanguageJson>(App.AppLanguageJson);
        }
        
        /// <summary>根据控件tag返回对应的全球化文本</summary>
        /// <param name="_SenderTag">控件的Tag属性</param>
        /// <returns>全球化文本</returns>
        public static String GetString(Object _SenderTag) {
            Int32 index;
            if( Int32.TryParse( ( (String)_SenderTag ).Replace( "I18N_" , "" ) ,out index ) && index<App.AppLanguageObject.Text.Count ) {
                return App.AppLanguageObject.Text[index];
            } else {
                return App.AppLanguageObject.Language+" - "+App.AppLanguageObject.LangTag+" - "+App.AppLanguageObject.LCID;
            }
        }

        /// <summary>根据索引返回全球化文本</summary>
        /// <param name="_Index">索引</param>
        /// <returns>全球化文本</returns>
        public static String GetString(Int32 _Index) {
            if(_Index<App.AppLanguageObject.Text.Count) {
                return App.AppLanguageObject.Text[_Index];
            }else {
                return App.AppLanguageObject.Language+" - "+App.AppLanguageObject.LangTag+" - "+App.AppLanguageObject.LCID;
            }
        }

        //
    }
}
