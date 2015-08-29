using System;

namespace Super_Mario_RNN
{
    internal class Tile
    {

        public BackgroundEnum Backgroud { get; private set; }
        public int Brick { get; private set; }
        public TileContents Contents { get; private set; }
        public bool PageFlag { get; private set; }
        public SceneryEnum Scenery { get; private set; }
        public int Size { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Tile(byte v1, byte v2)
        {
            Backgroud = BackgroundEnum.IGNORED;
            Scenery = SceneryEnum.IGNORED;
            int yNybble = (v1 & 0xF);
            //if (yNybble == 0xF)
            //    throw new Exception("This tile should be 3 bytes");

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

                if (Contents == TileContents.HORIZONTAL_QUESTION_BLOCK_COIN_V_3)
                    Y = 3;
                else if (Contents == TileContents.BRIDGE_V_7 ||
                    Contents == TileContents.HORIZONTAL_QUESTION_BLOCK_COIN_V_7)
                    Y = 7;
                else if (Contents == TileContents.BRIDGE_V_8)
                    Y = 8;
                else if (Contents == TileContents.BRIDGE_V_10)
                    Y = 10;
            }
            else if (yNybble == 0xD)
            {
                if (obj < 0x40)
                {
                    Size = obj & 0x3F;
                    Contents = TileContents.PAGE_SKIP;
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
                    Contents = (TileContents)(obj - 0x45 + 0xD00);
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
                    Brick = (obj & 0xF);
                    Scenery = (SceneryEnum)(obj & 0x30);
                }
                else
                {
                    Backgroud = (BackgroundEnum)(obj & 0x7);
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

        //public Tile(byte v1, byte v2, byte v3)
        //{
        //    Backgroud = BackgroundEnum.IGNORED;
        //    Scenery = SceneryEnum.IGNORED;
        //    if ((v1 & 0xF) != 0xF)
        //        throw new Exception("This tile should be 2 bytes");

        //    X = v1 >> 4;
        //    Y = v2 >> 4;

        //    int variable = v3 & 0xF;
        //    int obj = (v2 & 0x0F) | (v3 & 0xF0);

        //    PageFlag = (obj & 0x80) == 0x80;

        //    switch (variable)
        //    {
        //        case 0x0:
        //            Size = obj & 0xF;
        //            Contents = (TileContents)(obj - Size + 0x1000);
        //            break;
        //        case 0x2:
        //            Size = obj & 0xF;
        //            Contents = TileContents.STAIRS_FOR_BEGINNING;
        //            break;
        //        case 0x4:
        //            Size = obj & 0xF;
        //            Contents = TileContents.SQUARE_CEILING_TILES;
        //            break;
        //        case 0x6:
        //            Size = obj & 0xF;
        //            Contents = TileContents.HORIZONTALLY_EXTENDABLE_EDGES_RIGHT;
        //            break;
        //        case 0x8:
        //            Size = obj & 0xF;
        //            Contents = (TileContents)(obj - Size + 0x10800);
        //            break;
        //        case 0xA:
        //            Size = obj & 0xF;
        //            Contents = TileContents.HORIZONTALLY_EXTENDABLE_BOTTOM_LEFT_WALL;
        //            break;
        //        case 0xC:
        //            Size = obj & 0xF;
        //            Contents = TileContents.HORIZONTALLY_EXTENDABLE_BOTTOM_RIGHT_WALL;
        //            break;
        //        case 0xE:
        //            Size = obj & 0xF;
        //            Contents = TileContents.VERTICAL_SEA_BLOCK;
        //            break;
        //    }
        //}

        public override string ToString()
        {
            string pageString = "";
            if (PageFlag)
                pageString = "P";

            if (Scenery != SceneryEnum.IGNORED)
                return string.Format("({0}){1} {2} {3}", X, pageString, Brick, Scenery);
            if (Backgroud != BackgroundEnum.IGNORED)
                return string.Format("({0}){1} {2}", X, pageString, Backgroud);

            return string.Format("({0},{1}){2} {3} x{4}", X, Y, pageString, Contents, Size + 1);
        }
    }
}