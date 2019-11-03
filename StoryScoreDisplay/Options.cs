using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Display
{
    public class Options
    {
        private int? _port = null;
        private string _fileStoreFolderPath;

        public int Port
        {
            get
            {
                if (_port == null)
                {
                    var port = ConfigurationManager.AppSettings["port"];

                    if (int.TryParse(port, out int portInt))
                    {
                        _port = portInt;
                        return _port.Value;
                    }
                    return 1884;    // default value
                }

                return _port.Value;
            }
        }

        public string ClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["clientid"];
            }
        }

        public string FileStorePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_fileStoreFolderPath))
                    _fileStoreFolderPath = ConfigurationManager.AppSettings["filestorepath"];

                if (string.IsNullOrEmpty(_fileStoreFolderPath))
                {
                    _fileStoreFolderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

                    if (!System.IO.Directory.Exists(_fileStoreFolderPath))
                        System.IO.Directory.CreateDirectory(_fileStoreFolderPath);
                }

                return _fileStoreFolderPath;
            }
        }
    }
}
