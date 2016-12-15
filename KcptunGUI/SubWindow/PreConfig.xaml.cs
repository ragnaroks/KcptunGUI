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
            this.Closing += PreConfig_Closing;
            this.Closed += PreConfig_Closed;
        }

        #region 窗口事件
        private void PreConfig_Closed( object sender , System.EventArgs e ) {
            this.DialogResult=initedConfig;
        }

        private void PreConfig_Closing( object sender , System.ComponentModel.CancelEventArgs e ) {
            this.ClosingDialog.IsOpen = true;

        }

        private void PreConfig_Loaded( object sender , System.Windows.RoutedEventArgs e ) {
            
        }
        #endregion
        private void Button_OnClick(object sender,System.Windows.RoutedEventArgs e) {
            System.Windows.Controls.Button thisButton = (System.Windows.Controls.Button)sender;
            switch( thisButton.Name ) {
                case "Button_SureDontSave":
                    initedConfig = false;
                    break;
                case "Button_ContinueSetting":
                    break;
                case "Button_UseDefaultConfig":
                    break;
                default:return;
            }
        }
    }
}
