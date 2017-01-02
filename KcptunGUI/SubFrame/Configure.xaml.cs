using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace KcptunGUI.SubFrame {
    /// <summary>
    /// Configure.xaml 的交互逻辑
    /// </summary>
    public partial class Configure : Page {
        public Configure() {
            InitializeComponent(); //PageConfigure
            this.Loaded += Configure_Loaded;
            this.PageConfigure_Image_Icon.Source = Imaging.CreateBitmapSourceFromHBitmap(ResourceCSharp.Picture.png_72x72_configure_1.GetHbitmap() , IntPtr.Zero , Int32Rect.Empty , BitmapSizeOptions.FromEmptyOptions());
        }

        private void Configure_Loaded(object sender , RoutedEventArgs e) {
            PageConfigure_I18N();
        }

        /// <summary>加载本地化文本</summary>
        private void PageConfigure_I18N() {
            this.PageConfigure_TextBlock_PageHeader.Text = Class.I18N.GetString(this.PageConfigure_TextBlock_PageHeader.Tag);
        }
    }
}
