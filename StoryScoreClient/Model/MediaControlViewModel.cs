using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client.Model
{
    public class MediaControlViewModel : INotifyPropertyChanged
    {
        public string FolderPath { get; set; } = @"C:\Users\ricka\Desktop\fct musik";

        public ObservableCollection<MediaFolder> MediaFolders { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
