using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Common
{
    public class FileTransferStatus
    {
        public string Name                { get; set; }
        public long   FileSize            { get; set; }
        public long   TransferredBytes    { get; set; }
        public bool   TransferComplete    { get; set; }
        public long   ElapsedMilliseconds { get; set; }
    }
}
