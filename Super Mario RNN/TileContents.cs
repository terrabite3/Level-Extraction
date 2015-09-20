namespace SuperMarioRNN
{
    public enum TileContents
    {
        // Single tiles
        QUESTION_BLOCK_MUSHROOM = 0x00,
        QUESTION_BLOCK_COIN = 0x01,
        HIDDEN_BLOCK_COIN = 0x02,
        HIDDEN_BLOCK_1UP = 0x03,
        BRICK_MUSHROOM = 0x04,
        BRICK_BEANSTALK = 0x05,
        BRICK_STAR = 0x06,
        BRICK_MULTIPLE_COINS = 0x07,
        BRICK_1UP = 0x08,
        SIDEWAYS_PIPE = 0x09,
        USED_BLOCK = 0x0A,
        TRAMPOLINE = 0x0B,
        REVERSE_L_PIPE = 0x0C,
        FLAG_POLE = 0x0D,
        BOWSERS_BRIDGE = 0x0E,
        NOTHING = 0x0F,
        // Objects with length or height
        ISLAND = 0x10,
        HORIZONTAL_BRICK = 0x20,
        HORIZONTAL_BLOCK = 0x30,
        HORIZONTAL_COINS = 0x40,
        VERTICAL_BRICK = 0x50,
        VERTICAL_BLOCK = 0x60,
        INACTIVE_PIPE = 0x70,
        ACTIVE_PIPE = 0x78,
        // Objects for Y = 0xC
        HOLE = 0xC00,
        BALANCE_HORIZONTAL_ROPE = 0xC10,
        BRIDGE_V_7 = 0xC20,
        BRIDGE_V_8 = 0xC30,
        BRIDGE_V_10 = 0xC40,
        HOLE_WATER = 0xC50,
        HORIZONTAL_QUESTION_BLOCK_COIN_V_3 = 0xC60,
        HORIZONTAL_QUESTION_BLOCK_COIN_V_7 = 0xC70,
        // Objects for Y = 0xD
        PAGE_SKIP0 = 0xD00,
        PAGE_SKIP1 = 0xD10,
        PAGE_SKIP2 = 0xD20,
        PAGE_SKIP3 = 0xD30,
        REVERSE_L_PIPE_2 = 0xD40,
        FLAG_POLE_2 = 0xD41,
        BOWSERS_AXE = 0xD42,
        AXE_ROPE = 0xD43,
        BOWSERS_BRIDGE2 = 0xD44,
        SCROLL_STOP = 0xD45,
        RED_CHEEP_CHEEP = 0xD48,
        CONTINUOUS_BB_CC = 0xD49,
        STOP_CONTINUATION = 0xD4A,
        LOOP_COMMAND = 0xD4B,
        // Objects for Y = 0xE
        // Scenery
        SCENERY_NONE = 0xE00,
        SCENERY_CLOUDS = 0xE10,
        SCENERY_MOUNTAINS = 0xE20,
        SCENERY_FENCE = 0xE30,

        // Backgrounds
        BACKGROUND_NOTHING = 0xE40,
        BACKGROUND_WATER = 0xE41,
        BACKGROUND_WALL = 0xE42,
        BACKGROUND_OVER_WATER = 0xE43,
        BACKGROUND_NIGHT = 0xE44,
        BACKGROUND_SNOW = 0xE45,
        BACKGROUND_NIGHT_SNOW = 0xE46,
        BACKGROUND_CASTLE = 0xE47,
        // Objects for Y = 0xF
        LIFT_VERTICAL_ROPE = 0xF00,
        BALANCE_VERTICAL_LIFT = 0xF10,
        CASTLE = 0xF20,
        STAIRS = 0xF30,
        LONG_REVERSE_L_PIPE = 0xF40,
        VERTICAL_BALLS = 0xF50,
        //// V = 0x2
        //STAIRS_FOR_BEGINNING = 0x10230,
        //// V = 0x4
        //SQUARE_CEILING_TILES = 0x10430,
        //// V = 0x6
        //HORIZONTALLY_EXTENDABLE_EDGES_RIGHT = 0x10630,
        //// V = 0x8
        //VERTICALLY_EXTENDABLE_CEILING_TILES = 0x10820,
        //HORIZONTALLY_EXTENDABLE_EDGES_LEFT = 0x10830,
        //// V = 0xA
        //HORIZONTALLY_EXTENDABLE_BOTTOM_LEFT_WALL = 0x10A30,
        //// V = 0xC
        //HORIZONTALLY_EXTENDABLE_BOTTOM_RIGHT_WALL = 0x10C30,
        //// V = 0xE
        //VERTICAL_SEA_BLOCK = 0x10E30,

    }
}
