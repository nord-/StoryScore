using StoryScore.Client.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for LineUp.xaml
    /// </summary>
    public partial class LineUp : Window
    {
        public LineupViewModel ViewModel { get; set; } = new LineupViewModel();


        public LineUp()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
