using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Diagnostics;

namespace KcptunGUI.Class {
    class LocalFunction {
        /// <summary>验证配置文件合法性</summary>
        /// <returns>是否合法</returns>
        public static Boolean ValidateConfigJSON() {
            if( String.IsNullOrWhiteSpace( App.AppConfigJson ) || App.AppConfigJson.IndexOf( "}{" , 0 ) > -1 ) { return false; }
            JSchema schema = JSchema.Parse( KcptunGUI.Properties.Resources.String_AppConfigJsonSchema );
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

        /// <summary>将当前对象写入文件</summary>
        /// <param name="_JSON">JSON对象</param>
        /// <param name="_Path">文件完整路径</param>
        public static void SaveJsonToFile(Object _JSON,String _Path) {
            System.IO.File.WriteAllText( Class.AppAttributes.ConfigFilePath , JsonConvert.SerializeObject( _JSON ) , Class.AppAttributes.UTF8EncodingNoBom );
        }

        /// <summary>保存配置文件</summary>
        public static void SaveAppConfig(){
            Class.LocalFunction.SaveJsonToFile(App.AppConfigObject , Class.AppAttributes.ConfigFilePath);
        }
    }
}