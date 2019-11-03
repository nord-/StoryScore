using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StoryScore.Client.Controls
{
    /// <summary>
    /// Interaction logic for FileTransferControl.xaml
    /// </summary>
    public partial class FileTransferControl : UserControl
    {
        public event Action<string> SendFile;

        public FileTransferControl()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendFile?.Invoke(FileNameTextBox.Text);
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files|*.*";
            if (openFileDialog.ShowDialog() == true)
                FileNameTextBox.Text = openFileDialog.FileName;
        }
    }
}
