using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    class AIPlayer
    {
        private Type pieceType;
        private HeuristicAlg heuristicAlg;
        private int lookAhead;

        public AIPlayer(Type pieceType, HeuristicAlg alg, int lookAhead)
        {
            this.pieceType = pieceType;
            this.heuristicAlg = alg;
            this.lookAhead = lookAhead;
        }

        public GameState play()
        {
            GameState root = new GameState();
            List<GameState> bestStates = new List<GameState>();

            double rootValue = 0;

            if (pieceType.Equals(typeof(Larva)))
            {
                rootValue = MinMax(root, lookAhead - 1, true);
            }
            else
            {
                rootValue = MinMax(root, lookAhead - 1, false);
            }

            foreach(GameState child in root.getChildStates())
            {
                if (child.getMinMaxValue() == rootValue)
                {
                    bestStates.Add(child);
                }
            }

            //printChildStates(root.getChildStates());

            Random rnd = new Random();
            int bestStateChoice = rnd.Next(0, bestStates.Count - 1);

            return bestStates[bestStateChoice];
        }

        private void printChildStates(List<GameState> childs)
        {
            String input = String.Empty;
            foreach (GameState level2Child in childs)
            {
                Console.WriteLine();
                Console.WriteLine("Level 2");
                Console.WriteLine("MinMax value: " + level2Child.getMinMaxValue());
                level2Child.printGameState();
                Console.WriteLine("Press [enter] to continue or 'skip' to skip prints...");
                input = Console.ReadLine();
                if (input.Equals("skip")) return;

                foreach (GameState level3Child in level2Child.getChildStates())
                {
                    Console.WriteLine();
                    Console.WriteLine("Level 3");
                    Console.WriteLine("Heuristic value: " + level3Child.getHeuristicValue());
                    level3Child.printGameState();
                    Console.WriteLine("Press [enter] to continue or 'skip' to skip prints...");
                    input = Console.ReadLine();
                    if (input.Equals("skip")) return;
                }
            }
        }

        private double MinMax(GameState state, int level, bool maximizing)
        {
            if (maximizing)
            {
                state.createChildStates(typeof(Larva));
            }
            else
            {
                state.createChildStates(typeof(Bird));
            }

            if (level == 0 || state.getChildStates().Count == 0 || state.getLarva().getPosY() == 0)
            {
                double heuristicValue = heuristicAlg.calculateHeuristic(state);
                state.setHeuristicValue(heuristicValue);
                return heuristicValue;
            }

            if (maximizing)
            {
                double bestValue = -9999;
                foreach (GameState child in state.getChildStates())
                {
                    double value = MinMax(child, level - 1, false);
                    child.setMinMaxValue(value);
                    bestValue = Math.Max(bestValue, value);
                }

                return bestValue;
            }
            else
            {
                double bestValue = 9999;
                foreach (GameState child in state.getChildStates())
                {
                    double value = MinMax(child, level - 1, true);
                    child.setMinMaxValue(value);
                    bestValue = Math.Min(bestValue, value);
                }

                return bestValue;
            }
        }
    }
}
