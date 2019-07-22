using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Client
{
    public class Options
    {
        private int? _port = null;
        private string _displayEndpoint = "";

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

        public string ReceiverClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["receiverClientId"];
            }
        }

        public string DisplayEndpoint
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_displayEndpoint))
                {
                    var endpoint = ConfigurationManager.AppSettings["displayEndpoint"];
                    if (string.IsNullOrWhiteSpace(endpoint))
                    {
                        _displayEndpoint = "127.0.0.1"; // default localhost
                    }
                    else
                    {
                        _displayEndpoint = endpoint;
                    }
                }
                return _displayEndpoint;
            }
        }
    }
}
