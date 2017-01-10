using System;
using System.Collections.Generic;

namespace KcptunGUI.Class {
    /// <summary>
    /// 配置文件实体类
    /// </summary>
    public class ConfigJson {
        /// <summary>区域ID</summary>
        public Int32 LCID { get; set; }

        /// <summary>是否自动关闭菜单</summary>
        public Boolean TabAutoHide { get; set; }
        
        /// <summary>是否记住窗口位置</summary>
        public Boolean RememberWinLocation { get; set; }

        /// <summary>窗口显示位置</summary>
        public Int32[] WinLocation { get; set; }

        /// <summary>是否获取系统状态</summary>
        public Boolean FetchSystemStatus { get; set; }
        
        /// <summary>服务器节点子类</summary>
        public class NodesItems {
            /// <summary>主机名</summary>
            public String hostname { get; set; }
            /// <summary>IP</summary>
            public String ip { get; set; }
            /// <summary>描述</summary>
            public String description { get; set; }
            /// <summary>本地端口</summary>
            public Int32 localport { get; set; }
            /// <summary>远程端口</summary>
            public Int32 remoteport { get; set; }
            /// <summary>是否自动链接</summary>
            public Boolean autoconnect { get; set; }
        }
        /// <summary>服务器节点</summary>
        public List<NodesItems> Nodes { get; set; }
    }
}
