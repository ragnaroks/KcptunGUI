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

namespace KcptunGUI.SubFrame {
    /// <summary>
    /// Status.xaml 的交互逻辑
    /// </summary>
    public partial class Status : Page {
        public Status() {
            InitializeComponent();
            this.Loaded += Status_Loaded;
            this.PageStatus_Image_Icon.Source = Class.AppAttributes.BitmapSource_picture_status_png;
            //this.PageStatus_TextBlock_PageHeader.Text = Class.I18N.GetString( this.PageStatus_TextBlock_PageHeader );
            //MessageBox.Show("已初始化");
        }
        /// <summary>
        /// 注意,每次切换到都会触发,不能用做初始化操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Status_Loaded(object sender , RoutedEventArgs e) {
            //MessageBox.Show("a");
            //this.PageStatus_Icon.Source = FindResource("picture_status_png") as ImageSource;
        }
    }
}
