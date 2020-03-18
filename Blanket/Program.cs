using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Blanket
{
    class Program
    {
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
            if(args.Length == 0)
            {
                Console.WriteLine("Provide a path to the save directory.");
                return;
            }
            string root = args[0];
            DirectoryInfo rootDir = new DirectoryInfo(root);

            if(!rootDir.Exists)
            {
                FileInfo test = new FileInfo(root);
                if (test.Exists)
                    Console.WriteLine($"{test.FullName} is not a directory.");
                else
                    Console.WriteLine($"{rootDir.FullName} doesn't exist.");
                return;
            }

            foreach(string f in Paths)
            {

                FileInfo file = rootDir.GetFile($"{f}.dat");
                if (!file.Exists)
                    continue;

                Console.WriteLine($"Decrypting {f}...");
                FileInfo header = rootDir.GetFile($"{f}Header.dat");

                SaveFile save = new SaveFile(f, file.ReadAll(), header.ReadAll());

                Console.WriteLine($"Key: {save.Key.ToHexString()}");
                Console.WriteLine($"IV: {save.IV.ToHexString()}");

                rootDir.GetFile($"{f}.dec").WriteAll(save.Data);

                save.UpdateCrc();
            }
        }
    }
}
