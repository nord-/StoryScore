using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace StoryScore.Client.Model
{
    //[AddINotifyPropertyChangedInterface]
    public class MediaFile : INotifyPropertyChanged
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                var splitted = value.Split('.');
                _name = splitted[0];
            }
        }
        public string Path { get; set; }
        public string Extension { get; set; }
        public FileInfo File { get; set; }
        public bool SyncToDisplay { get; set; }
        public bool Synced { get; set; }

        public decimal TransferProgress { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class MediaFolder
    {
        private string _settingsFile;

        private string _name;
        private SolidColorBrush _backgroundColor = new SolidColorBrush(Colors.Black);
        private MediaFile[] _files;

        public MediaFolder(string path, MediaFile[] mediaFiles)
        {
            _settingsFile = System.IO.Path.Combine(path, "storyscore.json");
            Path = path;
            Files = mediaFiles;
        }

        public string Name
        {
            get => _name;
            set
            {
                var splittedName = value.Split('.');
                _name = splittedName[0];
                if (splittedName.Length == 2 && splittedName[1].IndexOf('#') >= 0)
                {
                    var c = (Color)ColorConverter.ConvertFromString(splittedName[1]);
                    _backgroundColor = new SolidColorBrush(c);
                }
            }
        }

        public SolidColorBrush BackgroundColor => _backgroundColor;

        public string Path { get; set; }
        public MediaFile[] Files
        {
            get => _files;
            set {
                _files = value;

                var o2 = GetFileSyncStatus();
                foreach (var f in _files)
                {
                    if (o2.TryGetValue(f.Name, out var tokenValue))
                        f.Synced = f.SyncToDisplay = (bool)tokenValue;

                    f.PropertyChanged += MediaFile_PropertyChanged;
                }
            }
        }

        public DirectoryInfo Folder { get; set; }

        public SolidColorBrush ForegroundColor
        {
            get
            {
                var c = _backgroundColor.Color;
                double Y = 0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B;
                var color = (Y / 255.0) > 0.5 ? Colors.Black : Colors.White;

                return new SolidColorBrush(color);
            }
        }

        private void MediaFile_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MediaFile.Synced))
            {
                var file = sender as MediaFile;
                UpdateFileSyncedStatus(file.Name);
            }
        }

        private void UpdateFileSyncedStatus(string filename)
        {
            var o2 = GetFileSyncStatus();

            if (o2.TryGetValue(filename, out var value))
            {
                Debug.Print(((bool)value).ToString());
            }
            else
            {
                o2.Add(filename, true);
            }

            using (StreamWriter file = File.CreateText(_settingsFile))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                o2.WriteTo(writer);
            }

            var fi = new FileInfo(_settingsFile);
            fi.Attributes |= FileAttributes.Hidden;
        }

        private JObject GetFileSyncStatus()
        {
            JObject o2 = new JObject();
            if (File.Exists(_settingsFile))
            {
                using (StreamReader file = File.OpenText(_settingsFile))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    o2 = (JObject)JToken.ReadFrom(reader);
                }
            }

            return o2;
        }
    }
}
