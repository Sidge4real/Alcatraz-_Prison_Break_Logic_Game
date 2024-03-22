using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Alcatraz__Prison_Break_Logic_Game
{
    public class Player
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Player() { }
        public Player(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}