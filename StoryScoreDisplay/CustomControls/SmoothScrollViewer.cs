using System.Windows;
using System.Windows.Controls;

namespace StoryScore.Display.CustomControls
{
    public class SmoothScrollViewer : ScrollViewer
    {
        public static readonly DependencyProperty MyOffsetProperty = DependencyProperty.Register(
            "MyOffset", typeof(double), typeof(SmoothScrollViewer),
            new PropertyMetadata(new PropertyChangedCallback(onChanged)));

        public double MyOffset
        {
            get { return (double)this.GetValue(ScrollViewer.VerticalOffsetProperty); }
            set { this.ScrollToVerticalOffset(value); }
        }
        private static void onChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SmoothScrollViewer)d).MyOffset = (double)e.NewValue;
        }
    }
}
