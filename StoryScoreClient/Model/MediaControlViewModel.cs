using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StoryScore.Client.Model
{
    public class MediaControlViewModel : INotifyPropertyChanged
    {

        public string FolderPath { get; set; } = @"C:\Users\ricka\Desktop\fct musik";

        public ObservableCollection<MediaFolder> MediaFolders { get; set; }

        RelayCommand<MediaFile> _syncCommand; public RelayCommand<MediaFile> SyncCommand
        {
            get
            {
                if (_syncCommand == null)
                {
                    _syncCommand = new RelayCommand<MediaFile>(param => this.Sync(param), param => true);
                }
                return _syncCommand;
            }
        }

        private void Sync(MediaFile file)
        {
            Debug.Print(file?.Name ?? "nothing");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
