//public static class Direction
//{
//    public const int PACKET_MOVE_DIR_LL = 0;
//    public const int PACKET_MOVE_DIR_LU = 1;
//    public const int PACKET_MOVE_DIR_UU = 2;
//    public const int PACKET_MOVE_DIR_RU = 3;
//    public const int PACKET_MOVE_DIR_RR = 4;
//    public const int PACKET_MOVE_DIR_RD = 5;
//    public const int PACKET_MOVE_DIR_DD = 6;
//    public const int PACKET_MOVE_DIR_LD = 7;
//}

using System;

public static class GameServer
{
    public const string IP = "127.0.0.1";
    public const UInt16 Port = 12121;
}

public static class BattleMap
{
    public const UInt16 RANGE_X = 6400;
    public const UInt16 RANGE_Y = 6400;
}

public enum Direction
{
    DIR_LL,
    DIR_LU,
    DIR_UU,
    DIR_RU,
    DIR_RR,
    DIR_RD,
    DIR_DD,
    DIR_LD,
    NONE
}

public static class Delta
{
    public const float DELTA_TIME_PER_FRAME = 0.02f;
    public const int DELTA_X = 3;
    public const int DELTA_Y = 2;
}