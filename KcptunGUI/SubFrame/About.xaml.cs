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

namespace KcptunGUI.SubFrame {
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Page {
        public About() {
            InitializeComponent();
            //MemoryStream ms = new MemoryStream();
            //AppResource.picture_status_png.Save(ms , ImageFormat.Bmp);
            //BitmapSource bs=Imaging.CreateBitmapSourceFromHBitmap(AppResource.picture_status_png.GetHbitmap(),IntPtr.Zero,Int32Rect.Empty,BitmapSizeOptions.FromEmptyOptions());

            //BitmapImage a = new BitmapImage();
            this.PageAbout_Image.Source = Class.AppAttributes.BitmapSource_picture_status_png;
            this.PageAbout_Image.Width = 32;
            this.PageAbout_Image.Height = 32;
        }
    }
}
