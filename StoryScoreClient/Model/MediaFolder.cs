using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StoryScore.Client.Model
{
    public class MediaFile
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
    }

    public class MediaFolder
    {
        private string _name;
        private SolidColorBrush _backgroundColor = new SolidColorBrush(Colors.White);

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

        public string        Path { get; set; }
        public MediaFile[]   Files { get; set; }
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
    }
}
