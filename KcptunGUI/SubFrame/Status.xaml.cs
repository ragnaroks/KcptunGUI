using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace KcptunGUI.SubFrame {
    /// <summary>
    /// Status.xaml 的交互逻辑
    /// </summary>
    public partial class Status : Page {
        public Status() {
            InitializeComponent();
            this.Loaded += Status_Loaded;
            this.PageStatus_Image_Icon.Source = Imaging.CreateBitmapSourceFromHBitmap(ResourceCSharp.Picture.png_72x72_status_2.GetHbitmap() , IntPtr.Zero , Int32Rect.Empty , BitmapSizeOptions.FromEmptyOptions());
            GetOccupyingAsync();
        }
        #region 初始化
        /// <summary>注意,每次切换到都会触发,不能用做初始化操作</summary>
        private void Status_Loaded(object sender , RoutedEventArgs e) {
            PageStatus_I18N();
        }
        /// <summary>加载本地化文本</summary>
        private void PageStatus_I18N() {
            PageStatus_TextBlock_PageHeader.Text = Class.I18N.GetString(PageStatus_TextBlock_PageHeader.Tag);
            //CPU
            PageStatus_TextBlock_AppCpuCardHeader.Text = Class.I18N.GetString(PageStatus_TextBlock_AppCpuCardHeader.Tag);
            PageStatus_TextBlock_AppCPUCardSystemOccupyingPretext.Text = Class.I18N.GetString( PageStatus_TextBlock_AppCPUCardSystemOccupyingPretext.Tag);
            PageStatus_TextBlock_AppCPUCardApplicationOccupyingPretext.Text = Class.I18N.GetString( PageStatus_TextBlock_AppCPUCardApplicationOccupyingPretext.Tag );
            //Memory
            PageStatus_TextBlock_AppMemoryCardHeader.Text = Class.I18N.GetString(PageStatus_TextBlock_AppMemoryCardHeader.Tag);
            //Network
            PageStatus_TextBlock_AppNetworkCardHeader.Text = Class.I18N.GetString(PageStatus_TextBlock_AppNetworkCardHeader.Tag);
            PageStatus_TextBlock_AppNetworkCardSystemUploadPretext.Text = Class.I18N.GetString( PageStatus_TextBlock_AppNetworkCardSystemUploadPretext.Tag );
            PageStatus_TextBlock_AppNetworkCardSystemDownloadPretext.Text = Class.I18N.GetString( PageStatus_TextBlock_AppNetworkCardSystemDownloadPretext.Tag );
        }
        #endregion

        #region 读取系统状态        
        /// <summary>读取系统的3个主要状态</summary>
        private async void GetOccupyingAsync() {
            await Task.Run( () => GetOccupyingDataAsync() );
        }
        /// <summary>异步获取系统状态</summary>
        /// <returns>无</returns>
        private void GetOccupyingDataAsync() {
            PerformanceCounter[] sys_network = Class.Functions.GetSystemPerformanceCounter_Network();//系统网络计数器
            PerformanceCounter sys_cpu = Class.Functions.GetSystemPerformanceCounter_CPU();//系统CPU计数器
            PerformanceCounter app_cpu = Class.Functions.GetApplicationPerformanceCounter_CPU();//本程序CPU计数器
            while( App.AppConfigObject.FetchSystemStatus ) {
                String[] value_SystemNetworkOccupying = Class.Functions.GetSystemOccupying_Network( sys_network);//系统网络数据
                String value_SystemCPUOccupying = Class.Functions.GetSystemOccupying_CPU(sys_cpu);//系统CPU数据
                String value_ApplicationCPUOccupying = Class.Functions.GetApplicationOccupying_CPU(app_cpu);//本程序CPU数据
                this.Dispatcher.Invoke(
                    () => {
                        this.PageStatus_TextBlock_AppCPUCardSystemOccupyingText.Text = value_SystemCPUOccupying;
                        this.PageStatus_TextBlock_AppCPUCardApplicationOccupyingText.Text = value_ApplicationCPUOccupying;
                        this.PageStatus_TextBlock_AppNetworkCardSystemUploadText.Text = value_SystemNetworkOccupying[0];
                        this.PageStatus_TextBlock_AppNetworkCardSystemDownloadText.Text = value_SystemNetworkOccupying[1];
                    }
                );
                Thread.Sleep( 1000 );
            }
        }
        #endregion
        //
    }
}
