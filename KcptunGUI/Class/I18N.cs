using System;

namespace KcptunGUI.Class {
    class I18N {
        /// <summary>通过指定LCID来加载对应的语言文件</summary>
        /// <param name="_LCID">LCID</param>
        /// <returns>是否加载成功</returns>
        public static Boolean LoadLanguageJsonFile(Int32 _LCID) {
            //todo
            return false;
        }
        /// <summary>获取全球化语言文本</summary>
        /// <param name="_index">索引</param>
        /// <returns>对应LCID的文本</returns>
        public static String GetString(Int32 _index) {
            return App.AppLanguageObject.Text[_index];
        }
    }
}
