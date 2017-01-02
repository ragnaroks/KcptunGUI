using System;
using System.Windows;
using System.Windows.Controls;

namespace KcptunGUI.Class {
    class I18N {
        /// <summary>通过指定LCID来加载对应的语言文件</summary>
        /// <param name="_LCID">LCID</param>
        /// <returns>是否加载成功</returns>
        public static Boolean LoadLanguageJsonFile(Int32 _LCID) {
            //todo
            return false;
        }
        
        public static String GetString(Object _SenderTag) {
            Int32 _index;
            if( Int32.TryParse( ( (String)_SenderTag ).Replace( "I18N_" , "" ) ,out _index ) ) {
                return App.AppLanguageObject.Text[_index];
            } else {
                return App.AppLanguageObject.Language;
            }
        }
    }
}
