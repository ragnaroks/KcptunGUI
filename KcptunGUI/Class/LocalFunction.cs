using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.NetworkInformation;

namespace KcptunGUI.Class {
    class LocalFunction {
        #region 非托管方法
        [DllImport("user32.dll",CharSet = CharSet.Auto)]
        public static extern Boolean IsWindowVisible(IntPtr hWnd);//当前窗体是否显示
        #endregion

        #region 托管方法
        /// <summary>清除空白字符</summary>
        /// <param name="_str">要处理的字符串</param>
        /// <param name="_all">false清理空格,true清理全部</param>
        /// <returns>已处理的字符串</returns>
        static public String TrimJSON(String _JSON,Boolean _All=false) {
            return _All ? System.Text.RegularExpressions.Regex.Replace(_JSON.Trim(),@"\s","") : _JSON.Trim();
        }

        /// <summary>字节组转内存流</summary>
        /// <param name="_bytes">字节组</param>
        /// <returns>内存流</returns>
        public static Stream BytesToStream(Byte[] _bytes) {
            return new MemoryStream(_bytes);
        }

        /// <summary>内存流转字节组</summary>
        /// <param name="_stream">内存流</param>
        /// <returns>字节组</returns>
        public static Byte[] StreamToBytes(Stream _stream) {
            Byte[] bytes = new Byte[_stream.Length];
            _stream.Read(bytes,0,bytes.Length);
            _stream.Seek(0,SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>验证指定JSON合法性</summary>
        /// <param name="_JSON">待验证的JSON</param>
        /// <param name="_SchemaString">验证器字符串</param>
        /// <returns>是否合法</returns>
        public static Boolean ValidateJSONBySchema( String _JSON , String _SchemaString ) {
            if( String.IsNullOrWhiteSpace( _JSON ) || String.IsNullOrWhiteSpace( _SchemaString ) || _JSON.IndexOf( "}{" , 0 ) > -1 || _SchemaString.IndexOf( "}{" , 0 ) > -1 ) { return false; }
            JSchema schema = JSchema.Parse( _SchemaString );
            JObject jo = JObject.Parse( _JSON );
            IList<string> errors;
            Boolean v = jo.IsValid( schema , out errors );
            if( errors.Count == 0 && v == true ) {
                return true;
            } else {
                foreach( var value in errors ) { Console.WriteLine( "验证失败: " + value ); }
                return false;
            }
        }

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

        /// <summary>将当前对象写入文件</summary>
        /// <param name="_JSON">JSON对象</param>
        /// <param name="_Path">文件完整路径</param>
        public static void SaveJsonToFile(Object _JSON,String _Path) {
            System.IO.File.WriteAllText( App.AppConfigFilePath , JsonConvert.SerializeObject( _JSON ) , App.UTF8EncodingNoBom );
        }

        /// <summary>保存配置文件</summary>
        public static void SaveAppConfig(){
            Class.LocalFunction.SaveJsonToFile(App.AppConfigObject , App.AppConfigFilePath);
        }

        /// <summary>获取活动网卡实例的设备描述</summary>
        /// <returns>设备描述</returns>
        public static String GetNetworkInterfaceInstance() {
            NetworkInterface[] ni = NetworkInterface.GetAllNetworkInterfaces();
            String InstanceName = "";
            foreach(NetworkInterface item in ni) {
                if(item.IsReceiveOnly != true
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

        /// <summary>字节单位换算</summary>
        /// <param name="_BytesCount">字节计数</param>
        /// <param name="_Value">out 换算后的数据</param>
        /// <param name="_Unit">out 换算后的数据的单位</param>
        public static void BytesCountUnitScaler_Single(Single _BytesCount,out Single _Value,out String _Unit) {
            if(1024<=_BytesCount && _BytesCount<1048576) {//1024²
                _Value=_BytesCount/1024;//kbyte
                _Unit="KB";
            } else if(1048576<=_BytesCount && _BytesCount<1073741824) {//1024³
                _Value=_BytesCount/1048576;//mbyte
                _Unit="MB";
            } else if(1073741824<=_BytesCount && _BytesCount<1099511627776) {//1024⁴
                _Value=_BytesCount/1073741824;//gbyte
                _Unit="GB";
            }else {
                _Value=_BytesCount;//byte
                _Unit="B";
            }
        }
        #endregion

        #region 处理器状态
        /// <summary>获得系统处理器性能计数器</summary>
        /// <param name="_pc1">处理器总负载</param>
        /// <param name="_pc2">KcptunGUI的处理器占用</param>
        public static void GetPerformanceCounter_Processor(out PerformanceCounter _pc1,out PerformanceCounter _pc2) {
            _pc1=new PerformanceCounter("Processor","% Processor Time","_Total");
            _pc2=new PerformanceCounter("Process","% Processor Time",App.AppProcess.ProcessName);
        }

        /// <summary>获取处理器状态</summary>
        /// <param name="_pcs">计数器</param>
        /// <returns>已处理的数据</returns>
        public static String[] GetStatus_Processor(PerformanceCounter[] _pcs) {
            String[] Status = {
                String.Format("{0:N0}%",_pcs[0].NextValue()),
                String.Format("{0:N2}%",_pcs[1].NextValue())
            };
            return Status;
        }
        #endregion

        #region 内存状态
        /// <summary>获取系统内存计数器</summary>
        /// <param name="_pc1">out 系统内存总量计数器</param>
        /// <param name="_pc2">out 系统可用内存计数器</param>
        public static void GetSystemMemoryPerformanceCounter(out PerformanceCounter _pc1,out PerformanceCounter _pc2) {
            _pc1=new PerformanceCounter(readOnly:true,instanceName:String.Empty,categoryName:"Memory",counterName: "Commit Limit");//计数器_系统内存总字节
            _pc2=new PerformanceCounter(readOnly:true,instanceName:String.Empty,categoryName:"Memory",counterName:"Available Bytes");//计数器_系统当前可用内存字节数
            //pcs[2]=new PerformanceCounter(readOnly:true,instanceName:String.Empty,categoryName:"Memory",counterName:"Modified Page List Bytes");//计数器_系统当前"已修改"内存字节数
        }

        /// <summary>根据进程名获取内存占用计数器</summary>
        /// <param name="_ProcessName">进程名</param>
        /// <param name="_pc">进程专用工作集计数器</param>
        /// <returns>专用工作集计数器</returns>
        public static void GetApplicationWorkingSetPrivateMemoryPerformanceCounter(String _ProcessName,out PerformanceCounter _pc) {
            _pc=new PerformanceCounter(readOnly:true,instanceName:_ProcessName,categoryName:"Process",counterName: "Working Set - Private");
        }

        /// <summary>获取内存状态</summary>
        /// <param name="pcs">计数器</param>
        /// <returns>已转化的数据</returns>
        public static String[] GetStatus_Memory(PerformanceCounter[] pcs){
            //当前可用内存
            Single AvailableMemory_Bytes = pcs[1].NextValue();
            Single AvailableMemory_HumanBytes;
            String AvailableMemory_Unit;
            BytesCountUnitScaler_Single(AvailableMemory_Bytes,out AvailableMemory_HumanBytes,out AvailableMemory_Unit);
            //系统总内存
            Single TotalMemory_Bytes = pcs[0].NextValue();
            Single TotalMemory_HumanBytes;
            String TotalMemory_Unit;
            BytesCountUnitScaler_Single(TotalMemory_Bytes,out TotalMemory_HumanBytes,out TotalMemory_Unit);
            //当前占用内存
            Single UsedMemory_Bytes = TotalMemory_Bytes-AvailableMemory_Bytes;
            Single UsedMemory_HumanBytes;
            String UsedMemory_Unit;
            BytesCountUnitScaler_Single(UsedMemory_Bytes,out UsedMemory_HumanBytes,out UsedMemory_Unit);
            //内存负载
            Single SystemOccupied = UsedMemory_Bytes/TotalMemory_Bytes;//0.05=5%
            //本应用程序虚拟内存
            Single AppVirtualMemory_Bytes = Process.GetCurrentProcess().VirtualMemorySize64;
            Single AppVirtualMemory_HumanBytes;
            String AppVirtualMemory_Unit;
            BytesCountUnitScaler_Single(AppVirtualMemory_Bytes,out AppVirtualMemory_HumanBytes,out AppVirtualMemory_Unit);
            //本应用程序独享内存
            Single AppPrivateMemory_Bytes = Process.GetCurrentProcess().PrivateMemorySize64;
            Single AppPrivateMemory_HumanBytes;
            String AppPrivateMemory_Unit;
            BytesCountUnitScaler_Single(AppPrivateMemory_Bytes,out AppPrivateMemory_HumanBytes,out AppPrivateMemory_Unit);
            //本应用程序实际内存
            Single AppRealMemory_Bytes = pcs[2].NextValue();
            Single AppRealMemory_HumanBytes;
            String AppRealMemory_Unit;
            BytesCountUnitScaler_Single(AppRealMemory_Bytes,out AppRealMemory_HumanBytes,out AppRealMemory_Unit);
            //内存状态
            String[] Status = {
                String.Format("{0:N1}{1} / {2:N1}{3}  {4:N1}%",UsedMemory_HumanBytes,UsedMemory_Unit,TotalMemory_HumanBytes,TotalMemory_Unit,SystemOccupied*100),//内存负载: 3.2GB/31.9GB  10.1%
                String.Format("{0:N1}{1}",AppVirtualMemory_HumanBytes,AppVirtualMemory_Unit),//应用程序分配的虚拟内存
                String.Format("{0:N1}{1}",AppPrivateMemory_HumanBytes,AppPrivateMemory_Unit),//应用程序分配的物理内存
                String.Format("{0:N1}{1}",AppRealMemory_HumanBytes,AppRealMemory_Unit)//应用程序分配的专用工作集
            };
            return Status;
        }
        #endregion

        /// <summary>获取网络计数器</summary>
        /// <param name="_pc1">出网字节计数器</param>
        /// <param name="_pc2">入网字节计数器</param>
        public static void GetPerformanceCounter_Network(out PerformanceCounter _pc1,out PerformanceCounter _pc2) {
            NetworkInterface ni = null;
            NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
            foreach(NetworkInterface item in nis) {//遍历出活动网卡,一般来说单网卡不会有问题
                if(item.IsReceiveOnly != true
                    && item.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && item.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                    && item.NetworkInterfaceType != NetworkInterfaceType.Unknown
                    && item.OperationalStatus == OperationalStatus.Up
                    && item.GetPhysicalAddress() != PhysicalAddress.None
                ) {ni = item;}
            }
            _pc1=new PerformanceCounter("Network Interface","Bytes Sent/sec",ni.Description);
            _pc2=new PerformanceCounter("Network Interface","Bytes Received/sec",ni.Description);
        }

        /// <summary>获取网络状态</summary>
        /// <param name="_pcs">计数器</param>
        /// <returns>已处理的数据</returns>
        public static String[] GetStatus_Network(PerformanceCounter[] _pcs){
            //1s内出网字节
            Single Sent_Bytes = _pcs[0].NextValue();
            Single Sent_HumanBytes;
            String Sent_Unit;
            BytesCountUnitScaler_Single(Sent_Bytes,out Sent_HumanBytes,out Sent_Unit);
            //1s内入网字节
            Single Received_Bytes = _pcs[1].NextValue();
            Single Received_HumanBytes;
            String Received_Unit;
            BytesCountUnitScaler_Single(Received_Bytes,out Received_HumanBytes,out Received_Unit);
            String[] Status = {
                String.Format("{0:N2} {1}/s",Sent_HumanBytes,Sent_Unit),
                String.Format("{0:N2} {1}/s",Received_HumanBytes,Received_Unit)
            };
            return Status;
        }
        //
    }
}