using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace KcptunGUI
{
    public class LogUtils
    {
        private RichTextBox logControl;

        public LogUtils(RichTextBox _control)
        {
            this.logControl = _control;
        }

        public void AppendLog(string log, Color color)
        {
            this.logControl.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    Run r = new Run(log + Environment.NewLine);
                    r.Foreground = new SolidColorBrush(color);
                    (this.logControl.Document.Blocks.LastBlock as Paragraph).Inlines.Add(r);
                    this.logControl.ScrollToEnd();
                }));
        }

        public void AppendLog(string log, Color color, bool addTime)
        {
            string tmp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + log;
            AppendLog(tmp, color);
        }

        public void ClearLog()
        {
            this.logControl.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    this.logControl.Document.Blocks.Clear();
                    this.logControl.Document.Blocks.Add(new Paragraph());
                }
                ));
        }
    }
}
