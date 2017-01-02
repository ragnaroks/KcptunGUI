using System;
using System.Collections.Generic;

namespace KcptunGUI.Class {
    /// <summary>语言配置文件实体类</summary>
    public class LanguageJson {
        /// <summary>语言描述</summary>
        public String Language { get; set; }
        /// <summary>语言标签</summary>
        public String LangTag { get; set; }
        /// <summary>LCID</summary>
        public Int32 LCID { get; set; }
        /// <summary>
        /// 文本列表
        /// </summary>
        public List<String> Text { get; set; }
    }
}
