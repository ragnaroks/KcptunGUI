using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace KcptunGUI.Class{
    /// <summary>废弃</summary>
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
                return new MemoryStream(_bytes);
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

        /// <summary>获取系统当前活动网卡的性能计数器</summary>
        /// <returns>性能计数器组,[0]上传情况,[1]下载情况</returns>
        public static PerformanceCounter[] GetSystemPerformanceCounter_Network() {
            NetworkInterface ni = null;
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            foreach( NetworkInterface item in nis ) {
                if( item.IsReceiveOnly != true
                    && item.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && item.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                    && item.NetworkInterfaceType != NetworkInterfaceType.Unknown
                    && item.OperationalStatus == OperationalStatus.Up
                    && item.GetPhysicalAddress() != PhysicalAddress.None
                ) {
                    ni = item;
                }
            }
            return new PerformanceCounter[] {
                new PerformanceCounter( "Network Interface" , "Bytes Sent/sec" , ni.Description ) ,
                new PerformanceCounter( "Network Interface" , "Bytes Received/sec" , ni.Description )
            };
        }
        /// <summary>根据性能计数器取当前网络速率</summary>
        /// <param name="_counter">性能计数器</param>
        /// <returns>已格式化的网络速率</returns>
        public static String[] GetSystemOccupying_Network( PerformanceCounter[] _counter) {
            Single up = _counter[0].NextValue();
            Single down = _counter[1].NextValue();
            String[] NetworkOccupyingString = { "" , "" };
            if( 1024 < up && up <= 1048576 ) {//kbyte
                NetworkOccupyingString[0] = String.Format( "{0:N2} kbyte/s" , up / 1024 );
            } else if( 1048576 < up && up <= 1073741824 ) {//mbyte
                NetworkOccupyingString[0] = String.Format( "{0:N2} mbyte/s" , up / 1024 / 1024 );
            } else {//default is byte or gbyte
                NetworkOccupyingString[0] = String.Format( "{0:N2} byte/s" , up );
            }
            if( 1024 < down && down <= 1048576 ) {//kbyte
                NetworkOccupyingString[1] = String.Format( "{0:N2} kbyte/s" , down / 1024 );
            } else if( 1048576 < down && down <= 1073741824 ) {//mbyte
                NetworkOccupyingString[1] = String.Format( "{0:N2} mbyte/s" , down / 1024 / 1024 );
            } else {//default is byte or gbyte
                NetworkOccupyingString[1] = String.Format( "{0:N2} byte/s" , down );
            }
            return NetworkOccupyingString;
        }

        /// <summary>获得系统CPU性能计数器</summary>
        /// <returns>系统CPU性能计数器</returns>
        public static PerformanceCounter GetSystemPerformanceCounter_CPU() {
            return new PerformanceCounter( "Processor" , "% Processor Time" , "_Total");
        }
        /// <summary>根据性能计数器获得CPU使用率</summary>
        /// <param name="_pc">性能计数器</param>
        /// <returns>已格式化的CPU使用率</returns>
        public static String GetSystemOccupying_CPU( PerformanceCounter _pc) {
            return String.Format("{0:N0}%", _pc.NextValue());
        }

        /// <summary>获得指定进程的CPU计数器,不写进程名则为本进程</summary>
        /// <param name="_pn">进程名称,不带后缀</param>
        /// <returns>进程CPU计数器</returns>
        public static PerformanceCounter GetApplicationPerformanceCounter_CPU( String _pn = null ) {
            return String.IsNullOrEmpty( _pn ) ? new PerformanceCounter( "Process" , "% Processor Time" , Process.GetCurrentProcess().ProcessName ) : new PerformanceCounter( "Process" , "% Processor Time" , _pn );
        }

        /// <summary>获取应用程序的CPU占用</summary>
        /// <param name="_pc">当前应用程序的计数器</param>
        /// <returns></returns>
        public static String GetApplicationOccupying_CPU( PerformanceCounter _pc) {
            return String.Format("{0:N2}%", _pc.NextValue() / Environment.ProcessorCount );
        }


    }
}
