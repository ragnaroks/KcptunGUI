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
        public MainWindow _MainWindow { get; set; }
        public Status() {
            InitializeComponent();
            this.Loaded += Status_Loaded;
            this.PageStatus_Image_Icon.Source = Imaging.CreateBitmapSourceFromHBitmap(KcptunGUI.Resource.图片.png_72x72_status_2.GetHbitmap() , IntPtr.Zero , Int32Rect.Empty , BitmapSizeOptions.FromEmptyOptions());
            GetOccupyingAsync();
        }

        #region 初始化
        /// <summary>注意,每次切换到都会触发,不能用做初始化操作</summary>
        private void Status_Loaded(object sender , RoutedEventArgs e) {
            PageStatus_I18N();
        }

        /// <summary>加载本地化文本</summary>
        private void PageStatus_I18N() {
            PageStatus_TextBlock_PageHeader.Text = Class.LocalFunction.GetString(PageStatus_TextBlock_PageHeader.Uid);
            //CPU
            PageStatus_TextBlock_ProcessorCardHeader.Text = Class.LocalFunction.GetString(PageStatus_TextBlock_ProcessorCardHeader.Uid);
            PageStatus_TextBlock_ProcessorCardSystemOccupyingPretext.Text = Class.LocalFunction.GetString(PageStatus_TextBlock_ProcessorCardSystemOccupyingPretext.Uid);
            PageStatus_TextBlock_ProcessorCardApplicationOccupyingPretext.Text = Class.LocalFunction.GetString(PageStatus_TextBlock_ProcessorCardApplicationOccupyingPretext.Uid);
            //Memory
            PageStatus_TextBlock_MemoryCardHeader.Text = Class.LocalFunction.GetString(PageStatus_TextBlock_MemoryCardHeader.Uid);
            PageStatus_TextBlock_MemoryCardSystemOccupyiedPretext.Text = Class.LocalFunction.GetString(PageStatus_TextBlock_MemoryCardSystemOccupyiedPretext.Uid);
            PageStatus_TextBlock_MemoryCardApplocationOccupyiedPretext.Text = Class.LocalFunction.GetString(PageStatus_TextBlock_MemoryCardApplocationOccupyiedPretext.Uid);
            //Network
            PageStatus_TextBlock_NetworkCardHeader.Text = Class.LocalFunction.GetString(PageStatus_TextBlock_NetworkCardHeader.Uid);
            PageStatus_TextBlock_NetworkCardSystemUploadPretext.Text = Class.LocalFunction.GetString( PageStatus_TextBlock_NetworkCardSystemUploadPretext.Uid);
            PageStatus_TextBlock_NetworkCardSystemDownloadPretext.Text = Class.LocalFunction.GetString( PageStatus_TextBlock_NetworkCardSystemDownloadPretext.Uid);
        }
        #endregion

        #region 读取系统状态        
        /// <summary>读取系统状态</summary>
        private async void GetOccupyingAsync() {
            await Task.Run( () => GetOccupyingDataAsync() );
        }
        /// <summary>异步获取系统状态</summary>
        /// <returns>无</returns>
        private void GetOccupyingDataAsync() {
            //处理器
            PerformanceCounter[] counter_Processor = new PerformanceCounter[2];//处理器计数器
            Class.LocalFunction.GetPerformanceCounter_Processor(out counter_Processor[0],out counter_Processor[1]);//系统负载,KcptunGUI的占用
            //内存
            PerformanceCounter[] counter_Memory = new PerformanceCounter[3];//内存计数器
            Class.LocalFunction.GetSystemMemoryPerformanceCounter(out counter_Memory[0],out counter_Memory[1]);//系统总内存,系统可用总内存
            Class.LocalFunction.GetApplicationWorkingSetPrivateMemoryPerformanceCounter(App.AppProcess.ProcessName,out counter_Memory[2]);//本程序专用工作集
            //网络
            PerformanceCounter[] counter_Network = new PerformanceCounter[2];//网络计数器
            Class.LocalFunction.GetPerformanceCounter_Network(out counter_Network[0],out counter_Network[1]);//出网字节,入网字节
            //数据处理
            while( true ) {
                if( App.AppConfigObject.FetchSystemStatus ) {
                    String[] value_Processor = Class.LocalFunction.GetStatus_Processor(counter_Processor);//处理器状态数据
                    String[] value_Memory = Class.LocalFunction.GetStatus_Memory(counter_Memory);//内存状态数据
                    String[] value_Network = Class.LocalFunction.GetStatus_Network(counter_Network);//网络状态数据
                    this.Dispatcher.Invoke(
                        () => {
                            //处理器
                            this.PageStatus_TextBlock_ProcessorCardSystemOccupyingText.Text =value_Processor[0];
                            this.PageStatus_TextBlock_ProcessorCardApplicationOccupyingText.Text = value_Processor[1];
                            //网络
                            this.PageStatus_TextBlock_NetworkCardSystemUploadText.Text = value_Network[0];
                            this.PageStatus_TextBlock_NetworkCardSystemDownloadText.Text = value_Network[1];
                            //内存
                            this.PageStatus_TextBlock_MemoryCardSystemOccupyiedText.Text=value_Memory[0];
                            this.PageStatus_TextBlock_MemoryCardApplocationOccupyiedText.Text=value_Memory[3];
                        }
                    );
                }
                Thread.Sleep( 1000 );
            }
        }
        #endregion
        //
    }
}
