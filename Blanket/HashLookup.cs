using System;
using System.Collections.Generic;
using System.Text;

namespace Blanket
{
    class HashLookup
    {
        public HashLookup(int hash, int data, int length)
        {
            Hash = hash;
            Data = data;
            Length = length;
        }
        public readonly int Hash, Data, Length;

        public static HashLookup[] MainLookups =
        {
            new HashLookup(0x000108, 0x00010C, 0x1D6D4C),
            new HashLookup(0x1D6E58, 0x1D6E5C, 0x323384),
            new HashLookup(0x4FA2E8, 0x4FA2EC, 0x035AC4),
            new HashLookup(0x52FDB0, 0x52FDB4, 0x03607C),
            new HashLookup(0x565F38, 0x565F3C, 0x035AC4),
            new HashLookup(0x59BA00, 0x59BA04, 0x03607C),
            new HashLookup(0x5D1B88, 0x5D1B8C, 0x035AC4),
            new HashLookup(0x607650, 0x607654, 0x03607C),
            new HashLookup(0x63D7D8, 0x63D7DC, 0x035AC4),
            new HashLookup(0x6732A0, 0x6732A4, 0x03607C),
            new HashLookup(0x6A9428, 0x6A942C, 0x035AC4),
            new HashLookup(0x6DEEF0, 0x6DEEF4, 0x03607C),
            new HashLookup(0x715078, 0x71507C, 0x035AC4),
            new HashLookup(0x74AB40, 0x74AB44, 0x03607C),
            new HashLookup(0x780CC8, 0x780CCC, 0x035AC4),
            new HashLookup(0x7B6790, 0x7B6794, 0x03607C),
            new HashLookup(0x7EC918, 0x7EC91C, 0x035AC4),
            new HashLookup(0x8223E0, 0x8223E4, 0x03607C),
            new HashLookup(0x858460, 0x858464, 0x2684D4),
        };

        public static HashLookup[] PersonalLookups =
        {
            new HashLookup(0x00108, 0x0010C, 0x35AC4),
            new HashLookup(0x35BD0, 0x35BD4, 0x3607C),
        };

        public static HashLookup[] PostboxLookups =
        {
            new HashLookup(0x100, 0x104, 0xB4447C),
        };

        public static HashLookup[] PhotoStudioIslandLookups =
        {
            new HashLookup(0x100, 0x104, 0x262B0),
        };

        public static HashLookup[] ProfileLookups =
        {
            new HashLookup(0x100, 0x104, 0x69404),
        };

    }
}
