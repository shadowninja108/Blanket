using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Blanket
{
    class Program
    {
        static uint[] ImportantSection;

        static readonly string[] Paths = new string[]
        {
            "main",
            "Villager0/personal",
            "Villager0/photo_studio_island",
            "Villager0/postbox",
            "Villager0/profile"
        };
        static void Main(string[] args)
        {
            string root = "B:/Downloads/";
            DirectoryInfo rootDir = new DirectoryInfo(root);

            foreach(string f in Paths)
            {

                FileInfo file = rootDir.GetFile($"{f}.dat");
                if (!file.Exists)
                    continue;

                Console.WriteLine($"Decrypting {f}...");
                FileInfo header = rootDir.GetFile($"{f}Header.dat");

                byte[] headerData = header.ReadAll();
                int importantSectionCount = (headerData.Length - 0x100) / sizeof(uint);
                ImportantSection = headerData.Skip(0x100)
                    .Split(sizeof(uint), importantSectionCount)
                    .Select(x => x.ToUInt32()).ToArray();

                byte[] key = GenKey(0);
                Console.WriteLine($"Key: {key.ToHexString()}");
                byte[] iv = GenKey(2);
                Console.WriteLine($"IV: {iv.ToHexString()}");

                byte[] fileData = file.ReadAll();
                byte[] dec = Decrypt(fileData, key, iv);
                rootDir.GetFile($"{f}.dec").WriteAll(dec);
            }
        }

        static byte[] GenKey(int s)
        {
            uint tickCount = ImportantSection[ImportantSection[s + 1] & 0x7F] & 0xF;
            tickCount++;
            SeadRandom random = new SeadRandom(ImportantSection[ImportantSection[s] & 0x7F]);
            for (int i = 0; i < tickCount; i++)
                random.GetU64();

            byte[] key = new byte[0x10];
            for(int i = 0; i < 0x10; i++)
                key[i] = (byte)((random.GetU32() >> 24) & 0xFF);

            return key;
        }

        static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            Aes128CounterMode am = new Aes128CounterMode(iv);
            ICryptoTransform ict = am.CreateDecryptor(key, iv);
            byte[] dec = new byte[data.Length];
            ict.TransformBlock(data, 0, data.Length, dec, 0);
            return dec;
        }
    }
}
