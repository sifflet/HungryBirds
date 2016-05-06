using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    interface HeuristicAlg
    {
        double calculateHeuristic(GameState state);
    }
}
