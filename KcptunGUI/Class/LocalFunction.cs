using System;
using System.Collections.Generic;

namespace KcptunGUI.Class {
    class LocalFunction {
        /// <summary>
        /// 验证JSON有效性 , http://lbv.github.io/litjson/docs/quickstart.html
        /// </summary>
        /// <param name="_JSON">待验证的JSON字符串</param>
        /// <param name="_Keys_ValueType">待验证的键-值类型</param>
        /// <returns></returns>
        public static Int32 ValidateJSON( String _JSON,Dictionary<String , LitJson.JsonType> _Keys_ValueType) {
            Int32 InvalidCount = 0;
            if( String.IsNullOrEmpty( _JSON ) ) { return Int32.MaxValue; }
            LitJson.JsonData jd = LitJson.JsonMapper.ToObject( new LitJson.JsonReader( _JSON ) { AllowComments=false,AllowSingleQuotedStrings=false,SkipNonMembers=false} );
            if( jd.Count < _Keys_ValueType.Count ) { return Int32.MaxValue; }
            foreach(KeyValuePair<String , LitJson.JsonType> _k_v in _Keys_ValueType ) {
                InvalidCount = ( jd[_k_v.Key] == null || jd[_k_v.Key].GetJsonType() != _k_v.Value ) ? +1 : +0;
            }
            jd = null;
            return InvalidCount;
        }
    }
}
