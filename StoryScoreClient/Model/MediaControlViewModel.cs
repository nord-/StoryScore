using GalaSoft.MvvmLight.Command;
using StoryScore.Client.Services;
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
        public MediaFile SelectedMediaFile { get => _file; set => _file = value; }
        //public string MediaFilePath => SelectedMediaFile?.Path ?? "";
        public System.Windows.Controls.MediaElement Player { get; set; }

        public ObservableCollection<MediaFolder> MediaFolders { get; set; }

        RelayCommand<MediaFile> _syncCommand;
        RelayCommand<MediaFile> _playCommand;
        private FileTransferService _fileTransferService;
        private MediaFile _file;

        public FileTransferService FileTransferService
        {
            get
            {
                return _fileTransferService;
            }
            set
            {
                _fileTransferService = value;
                _fileTransferService.TransferStatus += FileTransferService_TransferStatus;
            }
        }

        private void FileTransferService_TransferStatus(Common.FileTransferStatus obj)
        {
            if (SelectedMediaFile == null) return;

            SelectedMediaFile.TransferProgress = obj.TransferredBytes / (decimal)obj.FileSize;
            SelectedMediaFile.Synced = obj.TransferComplete;
        }

        public RelayCommand<MediaFile> SyncCommand
        {
            get
            {
                if (_syncCommand == null)
                {
                    _syncCommand = new RelayCommand<MediaFile>(param => this.Sync(param), param => !(param?.Synced ?? false));
                }
                return _syncCommand;
            }
        }

        public RelayCommand<MediaFile> PlayCommand
        {
            get
            {
                if (_playCommand == null)
                    _playCommand = new RelayCommand<MediaFile>(p => Play(p), p => true);

                return _playCommand;
            }
        }

        private void Play(MediaFile p)
        {
            SelectedMediaFile = p;
        }

        private async void Sync(MediaFile file)
        {
            SelectedMediaFile = file;
            Debug.Print(file?.Name ?? "nothing");
            await _fileTransferService.SendFileAsync(file.File.FullName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
