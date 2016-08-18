using System;
using System.Runtime.InteropServices;

namespace KcptunGUI.Class
{
    class Functions
    {
        static public String GetModeStringFromSelectedIndex(UInt16 _SelectedIndex){
            switch (_SelectedIndex){
                case 0: return "default"; case 1: return "normal"; case 2: return "fast"; case 3: return "fast2"; case 4: return "fast3"; default: return "fast2";
            }
        }
        static public String GetDscpStringFromString(String _strDscp){//DSCP的值只能是整数 0~63
            if (_strDscp.Length == 0) { return ""; }
            else { Byte a; if (Byte.TryParse(_strDscp, out a) && a < 64) { return " -dscp " + a; } else { return ""; } }
        }
        static public String GetCryptStringFromSelectedIndex(UInt16 _SelectedIndex){
            switch (_SelectedIndex){
                case 0:return "nonuse"; case 1:return "none"; case 2:return "xor"; case 3:return "tea"; case 4:return "aes"; case 5:return "aes-128"; case 6:return "aes-192"; default:return "nonuse";
            }
        }

        // C函数声明
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Boolean IsWindowVisible(IntPtr hWnd);//当前窗体是否显示
    }
}
