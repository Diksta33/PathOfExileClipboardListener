using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ExileClipboardListener.JSON
{
    public static class Extensions
    {
        public static byte[] ReadAllBytes(this StreamReader reader)
        {
            var bytes = new List<byte>();
            var buffer = new byte[1024];

            int readBytes = -1;
            while (readBytes != 0)
            {
                readBytes = reader.BaseStream.Read(buffer, 0, buffer.Length);
                bytes.AddRange(buffer.Take(readBytes));
            }
            return bytes.ToArray();
        }
    }
}
