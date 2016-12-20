using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace KcptunGUI.SubWindow {
    /// <summary>
    /// PreConfig.xaml 的交互逻辑
    /// </summary>
    public partial class PreConfig :  MetroWindow{
        Boolean initedConfig = false;
        public PreConfig() {
            InitializeComponent();
            this.Loaded += PreConfig_Loaded;
            this.Closed += PreConfig_Closed;
        }

        #region 窗口事件
        private void PreConfig_Loaded( object sender , System.Windows.RoutedEventArgs e ) {
            this.Cursor = App.AppCursor[0];
            
        }
        private void PreConfig_Closed( object sender , System.EventArgs e ) {
            this.DialogResult=initedConfig;
        }
        #endregion
        private void Button_OnClick(object sender,System.Windows.RoutedEventArgs e) {
            System.Windows.Controls.Button thisButton = (System.Windows.Controls.Button)sender;
            switch( thisButton.Name ) {
                default:
                case "Button_CancelConfig":
                    if( MessageBoxResult.Yes== MessageBox.Show( "确定不进行配置并退出?" , App.AppAttributes["Name"] , MessageBoxButton.YesNo , MessageBoxImage.Question ) ) {
                        initedConfig = false;
                        this.Close();
                    }
                    break;
                case "Button_SaveConfig":
                    
                    break;
            }
        }
    }
}
