using System;
using System.Runtime.InteropServices;

namespace KcptunGUI.Class
{
    class Functions
    {
        static public String GetModeStringFromSelectedIndex(UInt16 _SelectedIndex){
            switch (_SelectedIndex){
                case 0: return "normal";
                case 1: return "fast";
                default:
                case 2: return "fast2";
                case 3: return "fast3";
                case 4: return "manual";
            }
        }
        static public String GetDscpStringFromString(String _strDscp){//DSCP的值只能是整数 0~63
            if (_strDscp.Length == 0) { return ""; }
            else { Byte a; if (Byte.TryParse(_strDscp, out a) && a < 64) { return " -dscp " + a; } else { return ""; } }
        }
        static public String GetCryptStringFromSelectedIndex(UInt16 _SelectedIndex){
            switch (_SelectedIndex){
                default:
                case 0:return "默认";
                case 1: return "aes-256";
                case 2: return "aes-192";
                case 3: return "aes-128";
                case 4: return "salsa20";
                case 5: return "blowfish";
                case 6: return "twofish";
                case 7: return "cast5";
                case 8: return "3des";
                case 9: return "xtea";
                case 10: return "tea";
                case 11: return "xor";
                case 12: return "none";
            }
        }

        // C函数声明
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Boolean IsWindowVisible(IntPtr hWnd);//当前窗体是否显示
    }
}
