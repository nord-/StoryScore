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
    }
}
