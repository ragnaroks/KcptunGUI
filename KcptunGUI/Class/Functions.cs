using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace KcptunGUI.Class
{
    class Functions{
        // C函数声明
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Boolean IsWindowVisible(IntPtr hWnd);//当前窗体是否显示

        /// <summary>
        /// 清除空白字符
        /// </summary>
        /// <param name="_str">要处理的字符串</param>
        /// <param name="_all">false清理空格,true清理全部</param>
        /// <returns></returns>
        static public String TrimString(String _str,Boolean _all=false) {
            return _all ? Regex.Replace( _str.Trim() , @"\s" , "" ) : Regex.Replace( _str.Trim() , @" " , "" );
        }
    }
}
