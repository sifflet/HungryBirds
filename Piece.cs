using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    interface Piece
    {
        bool move(int x, int y);
        List<int[]> getPossibleMoves();
        void update();
        int getPosX();
        int getPosY();
    }
}
