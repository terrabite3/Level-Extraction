using System;
using System.Text.RegularExpressions;

namespace SuperMarioRNN
{
    public class Tile
    {

        public TileContents? Contents { get; private set; }
        public bool PageFlag { get; set; }
        public int? Size { get; private set; }
        public int X { get; private set; }
        public int? Y { get; private set; }
        public bool End { get; set; }

        public Tile(byte v1, byte v2)
        {
            if (v1 == 0xFD)
            {
                End = true;
                return;
            }
            int yNybble = (v1 & 0xF);

            X = v1 >> 4;

            int obj = v2 & 0x7F;

            if (yNybble <= 0xB)
            {
                Y = yNybble;
                if (obj < 0x10)
                {
                    Contents = (TileContents)obj;
                }
                else if (obj >= 0x10 &&
                    obj < 0x70)
                {
                    Size = obj & 0xF;
                    Contents = (TileContents)(obj - Size);
                }
                else
                {
                    Size = obj & 0x7;
                    Contents = (TileContents)(obj - Size);
                }
            }
            else if (yNybble == 0xC)
            {
                Size = obj & 0xF;
                Contents = (TileContents)(obj - Size + 0xC00);
            }
            else if (yNybble == 0xD)
            {
                if (obj < 0x10)
                {
                    Size = obj & 0xF;      // Size can only be up to 0xF
                    Contents = TileContents.PAGE_SKIP0;
                }
                else if (obj < 0x20)
                {
                    Size = obj & 0xF;      // Size can only be up to 0xF
                    Contents = TileContents.PAGE_SKIP1;
                }
                else if (obj < 0x30)
                {
                    Size = obj & 0xF;      // Size can only be up to 0xF
                    Contents = TileContents.PAGE_SKIP2;
                }
                else if (obj < 0x40)
                {
                    Size = obj & 0xF;      // Size can only be up to 0xF
                    Contents = TileContents.PAGE_SKIP3;
                }
                else if (obj >= 0x40 &&
                    obj < 0x45)
                {
                    Contents = (TileContents)(obj + 0xD00);
                }
                else if (obj >= 0x45 &&
                    obj < 0x48)
                {
                    Size = obj - 0x45;
                    Contents = TileContents.SCROLL_STOP;
                }
                else if (obj >= 0x48 &&
                    obj < 0x4C)
                {
                    Contents = (TileContents)(obj + 0xD00);
                }
            }
            else if (yNybble == 0xE)
            {
                if (obj < 0x40)
                {
                    Size = (obj & 0xF);  // The bricks type is stored in size
                    Contents = (TileContents)((obj & 0x30) + 0xE00);
                }
                else
                {
                    Contents = (TileContents)((obj & 0x7) + 0xE40);
                }
            }
            else if (yNybble == 0xF)
            {
                if (obj < 0x10)
                {
                    Contents = TileContents.LIFT_VERTICAL_ROPE;
                }
                else if (obj < 0x20)
                {
                    Contents = TileContents.BALANCE_VERTICAL_LIFT;
                }
                else if (obj < 0x30)
                {
                    Contents = TileContents.CASTLE;
                }
                else if (obj < 0x40)
                {
                    Contents = TileContents.STAIRS;
                }
                else if (obj < 0x50)
                {
                    Contents = TileContents.LONG_REVERSE_L_PIPE;
                }
                else if (obj < 0x60)
                {
                    Contents = TileContents.VERTICAL_BALLS;
                }
                Size = obj & 0xF;
            }

            PageFlag = (v2 & 0x80) == 0x80;


        }

        /// <summary>
        /// Construct a Tile from an array of floats.
        /// 
        /// </summary>
        /// <param name="fp"></param>
        public Tile(float[] fp)
        {
            float endFloat = fp[0];
            float pageFloat = fp[1];
            float[] xFloats = new float[16];
            float[] yFloats = new float[16];
            float[] sizeFloats = new float[16];
            float[] contentsFloats = new float[64];

            Array.Copy(fp, 2, xFloats, 0, 16);
            Array.Copy(fp, 18, yFloats, 0, 16);
            Array.Copy(fp, 34, sizeFloats, 0, 16);
            Array.Copy(fp, 50, contentsFloats, 0, 64);

            // 1:End
            End = true;
            for (int i = 1; i < fp.Length; i++)
                if (fp[i] > endFloat)
                    End = false;
            if (End)
                return;
            // 1:Page
            PageFlag = pageFloat > 0.5;
            // 16:X
            X = GetMaxIndex(xFloats);
            // 16:Y
            Y = GetMaxIndexOrNull(yFloats);
            // 16:Size
            Size = GetMaxIndexOrNull(sizeFloats);
            // 64:Contents
            var tileValues = Enum.GetValues(typeof(TileContents)) as TileContents[];
            int maxTile = GetMaxIndex(contentsFloats);
            Contents = tileValues[(int)maxTile];

        }

        private int? GetMaxIndexOrNull(float[] array)
        {
            float max = array[0];
            int maxIndex = 0;
            bool allEqual = true;
            for (int i = 0; i < array.Length; i++)
            {
                allEqual &= max == array[i];
                if (array[i] > max)
                {
                    max = array[i];
                    maxIndex = i;
                }
            }
            if (allEqual)
                return null;
            return maxIndex;
        }

        private int GetMaxIndex(float[] array)
        {
            float max = float.NegativeInfinity;
            int maxIndex = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

        public override string ToString()
        {
            string pageString = "";
            if (PageFlag)
                pageString = "P";

            return string.Format("({0},{1}){2} {3} x{4}", X, Y, pageString, Contents, Size + 1);
        }

        public string ToVector()
        {
            if (End)
            {
                return OneHot(0, 114);
            }
            string result = "0";
            // Page
            if (PageFlag)
                result += 1;
            else
                result += 0;
            // X
            result += OneHot(X, 16);
            // Y
            if (Y == null)
                result += OneHot(-1, 16);
            else
                result += OneHot((int)Y, 16);
            // Size
            if (Size == null)
                result += OneHot(-1, 16);
            else
                result += OneHot((int)Size, 16);
            // Contents (??)
            var tileValues = Enum.GetValues(typeof(TileContents));
            result += OneHot(Array.IndexOf(tileValues, Contents), tileValues.Length);

            return result;
        }

        public string ToWord()
        {
            if (End)
                return "!";

            string result = "";

            var tileValues = Enum.GetValues(typeof(TileContents));
            int index = Array.IndexOf(tileValues, Contents);
            result += (char)(index + '0');

            int xVal = X + (PageFlag ? 16 : 0);
            result += (char)(xVal + '0');

            if (Y != null)
                result += (char) (Y + 'A');

            if (Size != null)
                result += (char) (Size + 'a');

            return result;
        }

        private static Regex WordValidator = new Regex(@"!|[0-o][0-O][A-P]?[a-p]?");

        public Tile(string word)
        {
            if (!WordValidator.Match(word).Success)
                throw new Exception();

            if (word[0] == '!')
            {
                End = true;
                return;
            }

            var tileValues = Enum.GetValues(typeof(TileContents)) as TileContents[];
            int index = word[0] - '0';
            Contents = tileValues[index];

            int xVal = word[1] - '0';
            X = xVal & 0xF;
            PageFlag = (xVal & 0x10) != 0;


            if (word.Length == 3)
            {
                if (word[2] - 'A' < 16)
                {
                    Y = word[2] - 'A';
                }
                else
                {
                    Size = word[2] - 'a';
                }
            }
            else if (word.Length == 4)
            {
                Y = word[2] - 'A';
                Size = word[3] - 'a';
            }
        }

        private static string OneHot(int hot, int span)
        {
            if (hot < 0 || hot >= span)
                return new string('0', span);

            string before = new string('0', hot);
            string after = new string('0', span - hot - 1);
            return before + '1' + after;
        }

        public ushort ToBytes()
        {
            if (End)
                return 0xFD00;

            ushort result = (ushort)(X << 12);
            if (Y != null)
                result |= (ushort)(Y << 8);

            result |= (ushort)Contents;

            if (Size != null)
            {
                result |= (ushort)Size;
            }

            if (PageFlag)
                result |= 1 << 7;


            return result;
        }
    }
}