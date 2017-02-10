using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Newtonsoft.Json;

namespace KcptunGUI.Class {
    class I18N {
        /// <summary>通过LCID来加载对应的语言文件</summary>
        /// <param name="_LCID">LCID</param>
        /// <returns>已经反序列化的语言对象</returns>
        public static Class.LanguageJson LoadLanguageObjectFromJSON(Int32 _LCID) {
            switch(_LCID) {
                case 1033:
                    return JsonConvert.DeserializeObject<Class.LanguageJson>(KcptunGUI.Resource.字符串.String_Language_ENG);//英文
                case 2052:
                default:
                    return JsonConvert.DeserializeObject<Class.LanguageJson>(KcptunGUI.Resource.字符串.String_Language_CHS);//简体中文
            }
        }
        
        /// <summary>根据控件Uid返回对应的全球化文本</summary>
        /// <param name="_SenderUid">控件的Uid属性</param>
        /// <returns>全球化文本</returns>
        public static String GetString(String _SenderUid) {
            UInt16 index = 0;
            if(String.IsNullOrWhiteSpace(_SenderUid)) {
                return App.AppLanguageObject.Text[0];
            } else {
                UInt16.TryParse(_SenderUid,out index);
                return App.AppLanguageObject.Text[index];
            }
        }

        /// <summary>根据控件索引返回对应的全球化文本</summary>
        /// <param name="_Index">手动指定的索引</param>
        /// <returns>全球化文本</returns>
        public static String GetString(UInt16 _Index) {
            return App.AppLanguageObject.Text[_Index];
        }
        //
    }
}
