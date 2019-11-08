using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Common
{
    public class RemoteFileInfo
    {
        public string   Name         { get; set; }
        public long     FileSize     { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string   AbsolutePath { get; set; }
        public string   Checksum     { get; set; }
    }
}
