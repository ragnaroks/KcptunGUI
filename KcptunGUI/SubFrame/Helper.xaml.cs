using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace KcptunGUI.SubFrame {
    /// <summary>
    /// Helper.xaml 的交互逻辑
    /// </summary>
    public partial class Helper : Page {
        public Helper() {
            InitializeComponent();
            this.Loaded += Helper_Loaded;
            this.PageHelper_Image_Icon.Source = Imaging.CreateBitmapSourceFromHBitmap( ResourceCSharp.Picture.png_72x72_text_1.GetHbitmap() , IntPtr.Zero , Int32Rect.Empty , BitmapSizeOptions.FromEmptyOptions());
        }

        private void Helper_Loaded(object sender , RoutedEventArgs e) {
            PageStatus_I18N();
        }

        /// <summary>加载本地化文本</summary>
        private void PageStatus_I18N() {
            this.PageHelper_TextBlock_PageHeader.Text = Class.I18N.GetString(this.PageHelper_TextBlock_PageHeader.Tag);
        }
    }
}
