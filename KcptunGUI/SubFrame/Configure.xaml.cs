using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using MaterialDesignThemes.Wpf;

namespace KcptunGUI.SubFrame {
    /// <summary>
    /// Configure.xaml 的交互逻辑
    /// </summary>
    public partial class Configure : Page {
        public MainWindow _MainWindow { get; set; }
        public Configure() {
            InitializeComponent(); //PageConfigure           
            this.Loaded += Configure_Loaded;
            this.PageConfigure_Image_Icon.Source = Imaging.CreateBitmapSourceFromHBitmap(KcptunGUI.Resource.图片.png_72x72_configure_1.GetHbitmap() , IntPtr.Zero , Int32Rect.Empty , BitmapSizeOptions.FromEmptyOptions());
            PageConfigure_Grid_Configure.DataContext = App.AppConfigObject;
        }

        private void Configure_Loaded(object sender , RoutedEventArgs e) {
            PageConfigure_I18N();
            foreach(ComboBoxItem item in PageConfigure_ComboBox_Setting_Language.Items ) {//显示当前使用的语言
                if( item.Tag.ToString() == App.AppConfigObject.LCID.ToString() ) {
                    item.IsSelected = true; return;
                }
            }
        }

        /// <summary>加载本地化文本</summary>
        private void PageConfigure_I18N() {
            PageConfigure_TextBlock_PageHeader.Text = Class.LocalFunction.GetString(PageConfigure_TextBlock_PageHeader.Uid);
            PageConfigure_TextBlock_ConfigureTip.Text = Class.LocalFunction.GetString(PageConfigure_TextBlock_ConfigureTip.Uid);
            PageConfigure_TextBlock_Setting_TabAutoHide.Text = Class.LocalFunction.GetString(PageConfigure_TextBlock_Setting_TabAutoHide.Uid);
            PageConfigure_TextBlock_Setting_RememberWinLocation.Text = Class.LocalFunction.GetString(PageConfigure_TextBlock_Setting_RememberWinLocation.Uid);
            PageConfigure_TextBlock_Setting_FetchSystemStatus.Text = Class.LocalFunction.GetString(PageConfigure_TextBlock_Setting_FetchSystemStatus.Uid);
            PageConfigure_TextBlock_Setting_LCID.Text = Class.LocalFunction.GetString(PageConfigure_TextBlock_Setting_LCID.Uid);
            PageConfigure_TextBlock_Setting_Language.Text = Class.LocalFunction.GetString(PageConfigure_TextBlock_Setting_Language.Uid);

        }

        /// <summary>System.Windows.Controls.Primitives.ToggleButton元素失去焦点的事件</summary>
        private void ToggleButton_LostFocus(object sender , RoutedEventArgs e) {
            //System.Windows.Controls.Primitives.ToggleButton thisToggleButton = (System.Windows.Controls.Primitives.ToggleButton)sender;
            Class.LocalFunction.SaveJsonToFile(App.AppConfigObject , Class.AppAttributes.ConfigFilePath);
        }

        private void ComboBox_SelectionChanged(object sender , SelectionChangedEventArgs e) {
            ComboBox thisComboBox =sender as ComboBox;
            switch( thisComboBox.Name ) {
                case "PageConfigure_ComboBox_Setting_Language":
                    PageConfigure_TextBox_Setting_LCID.Text = (thisComboBox.SelectedItem as ComboBoxItem).Tag.ToString();
                    Int32 a = 0;
                    Int32.TryParse(PageConfigure_TextBox_Setting_LCID.Text,out a);
                    App.AppConfigObject.LCID = ( a == 0 ) ? 2052 : a;
                    Class.LocalFunction.LoadLanguageObjectFromJSON(App.AppConfigObject.LCID);
                    PageConfigure_I18N();
                    _MainWindow.MainWindow_I18N();
                    break;
                default:break;
            }
            Class.LocalFunction.SaveJsonToFile(App.AppConfigObject , Class.AppAttributes.ConfigFilePath);
        }
    }
}
