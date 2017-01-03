using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace KcptunGUI.Class
{
    class Functions{
        // C函数声明
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Boolean IsWindowVisible(IntPtr hWnd);//当前窗体是否显示

        //[DllImport("gdi32.dll")]
        //public static extern Boolean DeleteObject(IntPtr hObject);

        /// <summary>清除空白字符</summary>
        /// <param name="_str">要处理的字符串</param>
        /// <param name="_all">false清理空格,true清理全部</param>
        /// <returns>已处理的字符串</returns>
        static public String TrimString(String _str,Boolean _all=false) {
            return _all ? System.Text.RegularExpressions.Regex.Replace( _str.Trim() , @"\s" , "" ) : System.Text.RegularExpressions.Regex.Replace( _str.Trim() , @" " , "" );
        }

        /// <summary>字节组转内存流</summary>
        /// <param name="_bytes">字节组</param>
        /// <returns>内存流</returns>
        static public Stream BytesToStream(Byte[] _bytes) {
            return new MemoryStream( _bytes );
        }

        /// <summary>内存流转字节组</summary>
        /// <param name="_stream">内存流</param>
        /// <returns>字节组</returns>
        static public Byte[] StreamToBytes(Stream _stream) {
            Byte[] bytes = new Byte[_stream.Length];
            _stream.Read( bytes , 0 , bytes.Length );
            _stream.Seek( 0 , SeekOrigin.Begin );
            return bytes;
        }

        /// <summary>获取活动网卡实例的设备描述</summary>
        /// <returns>设备描述</returns>
        public static String GetNetworkInterfaceInstance() {
            NetworkInterface[] ni = NetworkInterface.GetAllNetworkInterfaces();
            String InstanceName = "";
            foreach( NetworkInterface item in ni ) {
                if( item.IsReceiveOnly != true
                    && item.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && item.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                    && item.NetworkInterfaceType != NetworkInterfaceType.Unknown
                    && item.OperationalStatus == OperationalStatus.Up
                    && item.GetPhysicalAddress() != PhysicalAddress.None
                ) {
                    InstanceName = item.Description;
                }
            }
            return InstanceName;
        }

        /// <summary>获取当前网络速率,若要监控则不应使用本方法</summary>
        /// <returns>单精度数据组[0]上传,[1]下载</returns>
        public static Single[] 获取当前活动网卡的网络速率() {
            PerformanceCounter[] 性能计数器组 = 获取当前活动网卡的性能计数器();
            return new Single[] { 性能计数器组[0].NextValue() , 性能计数器组[1].NextValue() };
        }

        /// <summary>获取当前活动网卡的性能计数器</summary>
        /// <returns>性能计数器组,[0]上传情况,[1]下载情况</returns>
        public static PerformanceCounter[] 获取当前活动网卡的性能计数器() {
            NetworkInterface 活动网卡 = null;
            NetworkInterface[] 所有网络接口 = NetworkInterface.GetAllNetworkInterfaces();
            foreach( NetworkInterface 当前遍历网络接口 in 所有网络接口 ) {
                if( 当前遍历网络接口.IsReceiveOnly != true
                    && 当前遍历网络接口.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && 当前遍历网络接口.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                    && 当前遍历网络接口.NetworkInterfaceType != NetworkInterfaceType.Unknown
                    && 当前遍历网络接口.OperationalStatus == OperationalStatus.Up
                    && 当前遍历网络接口.GetPhysicalAddress() != PhysicalAddress.None
                ) {
                    活动网卡 = 当前遍历网络接口;
                }
            }
            return new PerformanceCounter[] { new PerformanceCounter( "Network Interface" , "Bytes Sent/sec" , 活动网卡.Description ) , new PerformanceCounter( "Network Interface" , "Bytes Received/sec" , 活动网卡.Description ) };
        }
        
        /// <summary>暂停运行</summary>
        public static void HoldOn() {
            System.Threading.Thread.Sleep(99999);
        }
    }
}
