using Murmur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Blanket
{
    public class SaveFile
    {
        public string Name;
        public byte[] Data, Header;
        public byte[] Key => GenKey(0);
        public byte[] IV => GenKey(2);
        private int RandomSectionCount => (Header.Length - 0x100) / sizeof(uint);
        private uint[] RandomSection => 
            Header.Skip(0x100)
            .Split(sizeof(uint), RandomSectionCount)
            .Select(x => x.ToUInt32()).ToArray();
        public SaveFile(string name, byte[] data, byte[] header)
        {
            Name = name;
            Header = header;

            Data = Crypt(data, Key, IV);
        }

        public void UpdateCrc()
        {
            HashAlgorithm murmur = MurmurHash.Create32();

            HashLookup[] lookups;
            switch (Name)
            {
                case "main":
                    lookups = HashLookup.MainLookups;
                    break;
                case "Villager0/personal":
                    lookups = HashLookup.PersonalLookups;
                    break;
                case "Villager0/photo_studio_island":
                    lookups = HashLookup.PhotoStudioIslandLookups;
                    break;
                case "Villager0/postbox":
                    lookups = HashLookup.PostboxLookups;
                    break;
                case "Villager0/profile":
                    lookups = HashLookup.ProfileLookups;
                    break;
                default:
                    Console.WriteLine($"{Name} not supported for updating hashes.");
                    return;
            }
            foreach (HashLookup lookup in lookups)
            {
                murmur.TransformFinalBlock(Data, lookup.Data, lookup.Length);
                Console.WriteLine($"{murmur.Hash.ToHexString()} == {Data.Skip(lookup.Hash).Take(4).ToHexString()}");
            }
        }

        public byte[] GetEncryptedData()
        {
            UpdateCrc();
            return Crypt(Data, Key, IV);
        }

        private byte[] GenKey(int s)
        {
            uint tickCount = RandomSection[RandomSection[s + 1] & 0x7F] & 0xF;
            tickCount++;
            SeadRandom random = new SeadRandom(RandomSection[RandomSection[s] & 0x7F]);
            for (int i = 0; i < tickCount; i++)
                random.GetU64();

            byte[] key = new byte[0x10];
            for (int i = 0; i < 0x10; i++)
                key[i] = (byte)((random.GetU32() >> 24) & 0xFF);

            return key;
        }
        private static byte[] Crypt(byte[] data, byte[] key, byte[] iv)
        {
            Aes128CounterMode am = new Aes128CounterMode(iv);
            ICryptoTransform ict = am.CreateDecryptor(key, iv);
            byte[] dec = new byte[data.Length];
            ict.TransformBlock(data, 0, data.Length, dec, 0);
            return dec;
        }
    }
}
