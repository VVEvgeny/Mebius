using System;
using System.IO;

namespace ExtensionMethods
{
    public static class StringExtension
    {
        public static string ToUnixPath(this string strIn)
        {
            var index = strIn.IndexOf(@"homec", StringComparison.Ordinal);
            if (index == -1) index = strIn.IndexOf(@"basec", StringComparison.Ordinal);
            if (index == -1) index = 14; //s:\np\2016.04\basec
            index -= 9;
            if (index == -3) index = 5;
            return strIn.Replace(strIn.Substring(0, index), ".").Replace(@"\", "/");
            //return strIn.Replace(@"s:\np", ".").Replace(@"\", "/");
        }
    }
    public static class FileInfoExtension
    {
        public static uint CalculateCrc(this FileInfo fileInfo)
        {
            var stream = File.OpenRead(fileInfo.FullName);
            const int bufferSize = 1024;
            const uint polynomial = 0xEDB88320;

            var result = 0xFFFFFFFF;

            var buffer = new byte[bufferSize];
            var tableCrc32 = new uint[256];

            unchecked
            {
                //
                // Инициалиазация таблицы
                //
                for (var i = 0; i < 256; i++)
                {
                    var crc32 = (uint)i;

                    for (var j = 8; j > 0; j--)
                    {
                        if ((crc32 & 1) == 1)
                            crc32 = (crc32 >> 1) ^ polynomial;
                        else
                            crc32 >>= 1;
                    }

                    tableCrc32[i] = crc32;
                }

                //
                // Чтение из буфера
                //
                var count = stream.Read(buffer, 0, bufferSize);

                //
                // Вычисление CRC
                //
                while (count > 0)
                {
                    for (var i = 0; i < count; i++)
                    {
                        result = ((result) >> 8)
                                 ^ tableCrc32[(buffer[i])
                                               ^ ((result) & 0x000000FF)];
                    }

                    count = stream.Read(buffer, 0, bufferSize);
                }
            }

            return ~result;
        }
    }
}
