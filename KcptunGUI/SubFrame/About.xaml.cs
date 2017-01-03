using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace KcptunGUI.SubFrame {
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Page {
        public About() {
            InitializeComponent();
            this.Loaded += About_Loaded;
            this.PageAbout_Image_Icon.Source = Imaging.CreateBitmapSourceFromHBitmap(ResourceCSharp.Picture.png_72x72_user_1.GetHbitmap() , IntPtr.Zero , Int32Rect.Empty , BitmapSizeOptions.FromEmptyOptions());

        }
        
        private void About_Loaded(object sender , RoutedEventArgs e) {
            PageAbout_I18N();
        }

        /// <summary>加载本地化文本</summary>
        private void PageAbout_I18N() {
            this.PageAbout_TextBlock_PageHeader.Text = Class.I18N.GetString(this.PageAbout_TextBlock_PageHeader.Tag);
        }
    }
}
