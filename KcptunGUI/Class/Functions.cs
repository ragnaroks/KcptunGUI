using System;
using System.IO;
using System.Runtime.InteropServices;

namespace KcptunGUI.Class
{
    class Functions{
        // C函数声明
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Boolean IsWindowVisible(IntPtr hWnd);//当前窗体是否显示

        [DllImport( "Kernel32.dll" , CharSet = CharSet.Auto )]
        public static extern UInt32 GetThreadLocale();//获取LCID,System.Globalization.CultureInfo.CurrentCulture.LCID


        /// <summary>
        /// 清除空白字符
        /// </summary>
        /// <param name="_str">要处理的字符串</param>
        /// <param name="_all">false清理空格,true清理全部</param>
        /// <returns></returns>
        static public String TrimString(String _str,Boolean _all=false) {
            return _all ? System.Text.RegularExpressions.Regex.Replace( _str.Trim() , @"\s" , "" ) : System.Text.RegularExpressions.Regex.Replace( _str.Trim() , @" " , "" );
        }
        /// <summary>
        /// 字节组转内存流
        /// </summary>
        /// <param name="_bytes">字节组</param>
        /// <returns>内存流</returns>
        static public Stream BytesToStream(Byte[] _bytes) {
            return new MemoryStream( _bytes );
        }
        /// <summary>
        /// 内存流转字节组
        /// </summary>
        /// <param name="_stream">内存流</param>
        /// <returns>字节组</returns>
        static public Byte[] StreamToBytes(Stream _stream) {
            Byte[] bytes = new Byte[_stream.Length];
            _stream.Read( bytes , 0 , bytes.Length );
            _stream.Seek( 0 , SeekOrigin.Begin );
            return bytes;
        }
        
    }
}
