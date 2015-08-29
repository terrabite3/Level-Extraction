using System;
using System.Collections.Generic;
using System.IO;

namespace Super_Mario_RNN
{
    class LevelExtractor
    {
        private static readonly byte END_OF_LEVEL = 0xFD;
        private static readonly string ROM_PATH = "../../Super_mario_brothers.nes";

        static void Main(string[] args)
        {
            byte[] rom = File.ReadAllBytes(ROM_PATH);

            int level1_1 = 0x26A0;

            List<Tile> tiles = new List<Tile>();

            for (int i = level1_1; rom[i] != END_OF_LEVEL;)
            {
                Tile t;
                //if ((rom[i] & 0xF) == 0xF)
                //{
                //    t = new Tile(rom[i], rom[i + 1], rom[i + 2]);
                //    i += 3;
                //}
                //else
                //{
                t = new Tile(rom[i], rom[i + 1]);
                i += 2;
                //}
                Console.WriteLine(t);
                tiles.Add(t);
            }

        }
    }
}
