using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SuperMarioRNN
{
    public class LevelExtractor
    {
        private const byte END_OF_LEVEL = 0xFD;
        private const string ROM_PATH = "../../Super_mario_brothers.nes";
        private const string VECTOR_FILE_PATH = "../../words.txt";

        public static readonly int[] LEVELS =
        {
            // World 1
            0x26A0,
            0x2C47,
            0x2705,
            0x21C1,
            // World 2
            0x27DF,
            0x2E57,
            0x275A,
            0x22A1,
            // World 3
            0x262B,
            0x2C14,
            0x247D,
            0x2314,
            // World 4
            0x2549,
            0x2CEA,
            0x28A1,
            0x2222,
            // World 5
            0x284D,
            0x2AA4,
            0x2705,
            0x2AA1,
            // World 6
            0x296D,
            0x259C,
            0x2908,
            0x21C1,
            // World 7
            0x2B90,
            0x2E57,
            0x275A,
            0x2381,
            // World 8
            0x2A11,
            0x2B17,
            0x24E0,
            0x240C
        };

        static void Main(string[] args)
        {
            byte[] rom = File.ReadAllBytes(ROM_PATH);
            
            List<Tile> tiles = new List<Tile>();
            using (StreamWriter outfile = new StreamWriter(VECTOR_FILE_PATH))
                for (int level = 0; level < LEVELS.Length; level++)
                {
                    Tile t = null;
                    for (int i = LEVELS[level]; (t == null) || (!t.End); i += 2)
                    {
                        byte rom_0 = rom[i];
                        byte rom_1 = rom[i + 1];

                        t = new Tile(rom_0, rom_1);
                        string word = t.ToWord();
                        outfile.WriteLine(word + " ")

                        Tile recreated = new Tile(word);

                        if (t.ToString() != recreated.ToString())
                        {
                            throw new Exception("Recreated tile failed to match!");
                        }

                        ushort recreatedBytes = recreated.ToBytes();
                        byte byte0 = (byte)((recreatedBytes & 0xFF00) >> 8);
                        byte byte1 = (byte)(recreatedBytes & 0xFF);

                        if (rom_0 != byte0)
                        {
                            //throw new Exception("Recreated bytes failed to match!");
                            Debugger.Break();
                        }
                        if (!recreated.End &&
                            rom_1 != byte1)
                        {
                            //throw new Exception("Recreated bytes failed to match!");
                            Debugger.Break();
                        }

                        tiles.Add(t);
                    }

                }

        }
    }
}
