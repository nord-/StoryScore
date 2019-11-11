using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryScore.Common
{
    public class MediaFileAction
    {
        public string              FileName        { get; set; }
        public MediaFileActionEnum Action          { get; set; }
        public DateTime            RequestMade     { get; set; }
        public DateTime?           OriginalRequest { get; set; }
    }

    public enum MediaFileActionEnum
    {
        Play,
        Stop,
        Noop
    }
}