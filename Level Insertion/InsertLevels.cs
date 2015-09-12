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
        private const string VECTOR_FILE_PATH = "../../../Super Mario RNN/vector.txt";

        static void Main(string[] args)
        {
            byte[] rom = File.ReadAllBytes(ROM_PATH);

            string[] vectors = File.ReadAllLines(VECTOR_FILE_PATH);

            List<Tile> tiles = new List<Tile>();

            foreach (string v in vectors)
            {
                if (v.Contains(","))
                {
                    var q =
                        from f in v.Split(new[] { ',' })
                        select float.Parse(f);
                    tiles.Add(new Tile(q.ToArray()));
                }
                else
                {   // Zeros and ones with no separator
                    var q =
                        from c in v.ToCharArray()
                        select float.Parse(c.ToString());
                    tiles.Add(new Tile(q.ToArray()));
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

            int selectedLevel = 1;
            int romPointer = LevelExtractor.LEVELS[0];
            foreach (Tile t in levels[selectedLevel])
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
    }
}
