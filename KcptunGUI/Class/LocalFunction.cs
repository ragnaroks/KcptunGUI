using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace KcptunGUI.Class {
    class LocalFunction {
        /// <summary>
        /// 生成默认配置文件
        /// </summary>
        /// <returns>默认配置文件JSON</returns>
        public static Object GenDefaultConfigObject() {
            Class.ConfigJson cjo = new ConfigJson();
            cjo.AutoConnect = false;
            cjo.LCID = System.Globalization.CultureInfo.CurrentCulture.LCID;
            cjo.Nodes = new List<ConfigJson.NodesItems>();

            Class.ConfigJson.NodesItems[] node = new ConfigJson.NodesItems[2];
            for( Int16 i = 0 ; i < node.Length ; i++ ) {
                node[i] = new ConfigJson.NodesItems();
                node[i].description = "图样主机 - " + i;
                node[i].hostname = "server" + i + ".tooyoung.simple";
                node[i].ip = "127.0.0." + i;
                node[i].localport = 1080 + i;
                node[i].remoteport = 8080 + i;
                cjo.Nodes.Add( node[i] );
            }
            return cjo;
        }
        /// <summary>
        /// 验证配置文件合法性
        /// </summary>
        /// <returns>是否合法</returns>
        public static Boolean ValidateConfigJSON() {
            if( String.IsNullOrWhiteSpace( App.AppConfigJson ) || App.AppConfigJson.IndexOf( "}{" , 0 ) > -1 ) { return false; }
            JSchema schema = JSchema.Parse( Properties.Resources.String_AppConfigJsonSchema );
            JObject jo = JObject.Parse( App.AppConfigJson );
            IList<string> errors;
            Boolean v = jo.IsValid( schema , out errors );
            if( errors.Count == 0 && v == true ) {
                return true;
            } else {
                foreach( var value in errors ) { Console.WriteLine( "配置文件验证失败: " + value ); }
                return false;
            }
        }
        /// <summary>
        /// 验证语言文件合法性
        /// </summary>
        /// <returns>是否合法</returns>
        public static Boolean ValidateLanguageJSON() {
            if( String.IsNullOrWhiteSpace( App.AppLanguageJson ) || App.AppLanguageJson.IndexOf( "}{" , 0 ) > -1 ) { return false; }
            JSchema schema = JSchema.Parse( Properties.Resources.String_AppLanguageJsonSchema );
            JObject jo = JObject.Parse( App.AppLanguageJson );
            IList<string> errors;
            Boolean v = jo.IsValid( schema , out errors );
            if( errors.Count == 0 && v == true ) {
                return true;
            } else {
                foreach( var value in errors ) { Console.WriteLine( "语言文件验证失败: " + value ); }
                return false;
            }
        }
        /// <summary>
        /// 验证指定JSON合法性
        /// </summary>
        /// <param name="_JSON">待验证的JSON</param>
        /// <param name="_SchemaString">验证器字符串</param>
        /// <returns>是否合法</returns>
        public static Boolean ValidateJSON( String _JSON , String _SchemaString ) {
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
        private static IPInterfaceStatistics 网络接口数据;
        public static Int64[] 获取活动网卡的使用情况() {
            NetworkInterface[] 所有网络接口 = NetworkInterface.GetAllNetworkInterfaces();
            foreach( NetworkInterface 当前遍历网络接口 in 所有网络接口 ) {
                if( 当前遍历网络接口.IsReceiveOnly != true
                    && 当前遍历网络接口.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && 当前遍历网络接口.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                    && 当前遍历网络接口.NetworkInterfaceType != NetworkInterfaceType.Unknown
                    && 当前遍历网络接口.OperationalStatus == OperationalStatus.Up
                    && 当前遍历网络接口.GetPhysicalAddress() != PhysicalAddress.None
                ) {
                    网络接口数据 = 当前遍历网络接口.GetIPStatistics();
                }
            }
            Int64[] 网卡速率 = { 网络接口数据.BytesSent , 网络接口数据.BytesReceived };
            Console.WriteLine(网卡速率[0]+"/"+网卡速率[1]);
            return 网卡速率;
        }
        /// <summary>
        /// 获取活动网卡实例的设备描述
        /// </summary>
        /// <returns>设备描述</returns>
        public static String GetNetworkInterfaceInstance() {
            NetworkInterface[] ni = NetworkInterface.GetAllNetworkInterfaces();
            String InstanceName = "";
            foreach( NetworkInterface item in ni ) {
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
        public static void 获取当前网络速率() {

            PerformanceCounter[] pc = new PerformanceCounter[2];
            //pc[0]=new PerformanceCounter("Network Interface", "Bytes Sent/sec");
            pc[0].CategoryName = "Network Interface";
            pc[0].CounterName = "Bytes Sent/sec";
            pc[0].InstanceName = "";
        }
    }
}
