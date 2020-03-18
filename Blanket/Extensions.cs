using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Blanket
{
    static class Extensions
    {
        public static FileInfo GetFile(this DirectoryInfo obj, string filename)
        {
            return new FileInfo($"{obj.FullName}{Path.DirectorySeparatorChar}{filename}");
        }

        public static bool ContainsFile(this DirectoryInfo obj, string filename)
        {
            return obj.GetFile(filename).Exists;
        }

        public static DirectoryInfo GetDirectory(this DirectoryInfo obj, string foldername)
        {
            return new DirectoryInfo($"{obj.FullName}{Path.DirectorySeparatorChar}{foldername.Replace('/', Path.DirectorySeparatorChar)}");
        }

        public static byte[] ReadAll(this FileInfo obj)
        {
            byte[] buff = new byte[obj.Length];
            using (FileStream stream = obj.OpenRead())
                stream.Read(buff);
            return buff;
        }

        public static void WriteAll(this FileInfo obj, byte[] data)
        {
            if (!obj.Exists)
                using (obj.Create());
            using FileStream stream = obj.OpenWrite();
            stream.Write(data, 0, data.Length);
        }
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> e, int split, int count)
        {
            for(int i = 0; i < count; i++)
                yield return e.Skip(i * split).Take(split);
        }


        public static uint ToUInt32(this IEnumerable<byte> bytes)
        {
            return BitConverter.ToUInt32(bytes.ToArray(), 0);
        }


        private static readonly uint[] Lookup32 = CreateLookup32();
        private static uint[] CreateLookup32()
        {
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("X2");
                result[i] = s[0] + ((uint)s[1] << 16);
            }
            return result;
        }

        public static string ToHexString(this IEnumerable<byte> bytes) => ToHexString(bytes.ToArray());
        public static string ToHexString(this byte[] bytes) => ToHexString(bytes.AsSpan());
        public static string ToHexString(this Span<byte> bytes)
        {
            uint[] lookup32 = Lookup32;
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                uint val = lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }

    }
}
