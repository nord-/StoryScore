using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        //RelayCommand _saveCommand; public ICommand SaveCommand
        //{
        //    get
        //    {
        //        if (_saveCommand == null)
        //        {
        //            _saveCommand = new RelayCommand<MediaFile>(param => this.Sync(param),
        //                param => true);
        //        }
        //        return _saveCommand;
        //    }
        //}

        public void Sync(MediaFile file)
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
