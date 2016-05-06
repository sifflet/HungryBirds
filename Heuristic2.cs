using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    class Heuristic2:HeuristicAlg
    {
        public Heuristic2()
        {

        }

        double HeuristicAlg.calculateHeuristic(GameState state)
        {
            double result = 0;
            double birdWeight = 0.5;
            double firstRowWeight = 1.0;
            double leftColumnWeight = 0.0;
            double rightColumnWeight = 0.0;

            Piece[,] gameBoard = state.getGameBoard();

            Piece larva = new Larva(0, 0, gameBoard);
            List<Piece> birds = new List<Piece>();

            for (int i = 0; i < Board.Instance.getXLength(); ++i)
            {
                for (int j = 0; j < Board.Instance.getYLength(); ++j)
                {
                    if (gameBoard[i, j] == null) continue;

                    if (gameBoard[i, j].GetType().Equals(typeof(Larva)))
                    {
                        larva = gameBoard[i, j];
                        larva.update();
                    }
                    else if (gameBoard[i, j].GetType().Equals(typeof(Bird)))
                    {
                        birds.Add(gameBoard[i, j]);
                    }
                }
            }
            
            foreach (Piece bird in birds)
            {
                bird.update();
            }

            if (larva.getPosY() == 0) return 9999;

            int highestDistance = 0;
            for (int i = 0; i < birds.Count; ++i)
            {
                highestDistance = Math.Max(highestDistance, manhattanDistance(larva, birds[i]));
            }

            result += highestDistance * birdWeight;
            result += 20 - manhattanDistance(larva, 4, 0) * firstRowWeight;
            result += manhattanDistance(larva, 0, larva.getPosY()) * leftColumnWeight;
            result += manhattanDistance(larva, 7, larva.getPosY()) * rightColumnWeight;

            return result;
        }

        private int manhattanDistance(Piece p1, Piece p2)
        {
            return Math.Abs(p1.getPosX() - p2.getPosX()) + Math.Abs(p1.getPosY() - p2.getPosY());
        }

        private int manhattanDistance(Piece p1, int posX, int posY)
        {
            return Math.Abs(p1.getPosX() - posX) + Math.Abs(p1.getPosY() - posY);
        }
    }
}
