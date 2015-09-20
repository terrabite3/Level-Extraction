using NDesk.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioRNN
{
    class InsertLevels
    {
        private const string ROM_PATH = "../../../Super Mario RNN/Super_mario_brothers.nes";
        //private const string VECTOR_FILE_PATH = "../../../Super Mario RNN/vector.txt";
        private const string WORD_FILE_PATH = "../../../mario_00.txt";

        static void Main(string[] args)
        {
            byte[] rom = File.ReadAllBytes(ROM_PATH);

            string allText = File.ReadAllText(WORD_FILE_PATH);

            string[] words = allText.Split();

            List<Tile> tiles = new List<Tile>();

            foreach (string w in words)
            {
                if (w.Length == 0)
                {
                    continue;
                }
                try
                {
                    tiles.Add(new Tile(w));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Bad word: {0}", w);
                }
            }


            List<Tile[]> levels = new List<Tile[]>();
            List<Tile> level = new List<Tile>();
            foreach (Tile t in tiles)
            {
                level.Add(t);
                if (t.End)
                {
                    levels.Add(level.ToArray());
                    level.Clear();
                }
            }

            Console.WriteLine("Found {0} levels", levels.Count);
            //int selectedLevel = 1;
            for (int i = 0; i < levels.Count; i++)
            {
                int romPointer = LevelExtractor.LEVELS[0];
                var l = levels[i];
                Console.Write("Level {0} of {1} has {2} tiles. Press enter to write ROM.", 
                    i, levels.Count, 
                    l.Length);
                Console.ReadLine();

                foreach (Tile t in l)
                {
                    ushort tileBytes = t.ToBytes();
                    byte b0 = (byte)(tileBytes >> 8);
                    byte b1 = (byte)(tileBytes & 0xFF);
                    byte rom0 = rom[romPointer];
                    byte rom1 = rom[romPointer + 1];
                    rom[romPointer++] = b0;
                    if (t.End)
                        break;
                    rom[romPointer++] = b1;
                }
                
                string outFileName = Path.Combine(
                    Path.GetDirectoryName(ROM_PATH),
                    Path.GetFileNameWithoutExtension(ROM_PATH) +
                    "_out" +
                    Path.GetExtension(ROM_PATH));

                File.WriteAllBytes(outFileName, rom);
            }

            Console.WriteLine("All levels done. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
