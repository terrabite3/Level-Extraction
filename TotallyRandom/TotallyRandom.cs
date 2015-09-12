using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioRNN
{
    class TotallyRandom
    {
        private const string ROM_PATH = "../../../Super Mario RNN/Super_mario_brothers.nes";

        static void Main(string[] args)
        {
            Random rand = new Random();
            List<Tile> tiles = new List<Tile>();
            int prevX = 0;
            while (true)
            {
                float[] vector = new float[114];
                for (int j = 0; j < 114; j++)
                {
                    vector[j] = (float)rand.NextDouble();
                }
                vector[0] *= tiles.Count / 50;
                vector[1] *= 0.65f;

                Tile t = new Tile(vector);

                t.PageFlag = (t.X < prevX);
                prevX = t.X;

                tiles.Add(t);
                if (t.End)
                    break;
            }

            byte[] rom = File.ReadAllBytes(ROM_PATH);
            int romPointer = LevelExtractor.LEVELS[0];
            foreach (Tile t in tiles)
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

            string outFileName = "totallyRandom.nes";

            File.WriteAllBytes(outFileName, rom);
        }
    }
}
