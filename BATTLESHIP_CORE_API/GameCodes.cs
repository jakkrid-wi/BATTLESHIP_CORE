using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BATTLESHIP_CORE_API
{
    public static class GameCodes
    {
        public static class Status
        {
            public static string INSTALL = "INSTALL";
            public static string READY = "READY";
            public static string END = "END";
        }

        public static class Fire
        {
            public static string HIT = "H";
            public static string MISS = "M";
            public static string MARKX = "X";
        }
    }
}
