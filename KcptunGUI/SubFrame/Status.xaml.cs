using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Interop;

namespace KcptunGUI.SubFrame {
    /// <summary>
    /// Status.xaml 的交互逻辑
    /// </summary>
    public partial class Status : Page {
        public Status() {
            InitializeComponent();
            this.Loaded += Status_Loaded;
            this.PageStatus_Image_Icon.Source = Imaging.CreateBitmapSourceFromHBitmap(ResourceCSharp.Picture.png_72x72_status_2.GetHbitmap() , IntPtr.Zero , Int32Rect.Empty , BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>注意,每次切换到都会触发,不能用做初始化操作</summary>
        private void Status_Loaded(object sender , RoutedEventArgs e) {
            PageStatus_I18N();//加载本地化文本,写在这里可以不用手动刷新
        }
        /// <summary>加载本地化文本</summary>
        private void PageStatus_I18N() {
            this.PageStatus_TextBlock_PageHeader.Text = Class.I18N.GetString(this.PageStatus_TextBlock_PageHeader.Tag);
        }
    }
}
