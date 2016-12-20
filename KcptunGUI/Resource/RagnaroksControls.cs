namespace RagnaroksWPF.Controls {
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:KcptunGUI.Resource"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:KcptunGUI.Resource;assembly=KcptunGUI.Resource"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:RagnaroksControls/>
    ///
    /// </summary>

    public class TitleBar: System.Windows.Controls.Control {
        static TitleBar() {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( TitleBar ) , new System.Windows.FrameworkPropertyMetadata( typeof( TitleBar ) ) );
        }
        /*
        public System.Windows.Media.ImageSource _IconSource {
            get { return base.GetValue( IconSource ) as System.Windows.Media.ImageSource; }
            set { base.SetValue( IconSource ,value); }
        }
        public static System.Windows.DependencyProperty IconSource =
            System.Windows.DependencyProperty.Register(
                "_IconSource",
                typeof( System.Windows.Media.ImageSource ),
                typeof(System.Windows.Controls.Image)
            );
            */
    }
}
