using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace StoryScore.Client
{
    /// <summary>
    /// Interaction logic for InputTimeWindow.xaml
    /// </summary>
    public partial class InputTimeWindow : Window
    {
        public TimeSpan OffsetTime { get; set; }

        public InputTimeWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) {
            var fixedTime = TimeTextBox.Text.Trim('_');
            var timeParts = fixedTime.Split(':');
            OffsetTime = new TimeSpan(0, int.Parse(timeParts[0].Trim('_')), int.Parse(timeParts[1].Trim('_')));
            DialogResult = true;
            this.Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
