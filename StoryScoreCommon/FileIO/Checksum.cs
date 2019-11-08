using System;
using System.IO;
using System.Security.Cryptography;

namespace StoryScore.Common
{
    public class FileIO
    {
        public static string GetChecksum(string file)
        {
            using (var stream = new BufferedStream(File.OpenRead(file), 512000))
            {
            //    using (FileStream stream = File.OpenRead(file))
            //{
                SHA256Managed sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }
    }
}
