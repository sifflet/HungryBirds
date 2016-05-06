using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    class SimpleHeuristic:HeuristicAlg
    {
        private int[,] heuristicTable;

        public SimpleHeuristic()
        {
            this.heuristicTable = new int[Board.Instance.getXLength(), Board.Instance.getYLength()];
            fillTable();
        }

        double HeuristicAlg.calculateHeuristic(GameState state)
        {
            double result = 0;
            Piece[,] gameBoard = state.getGameBoard();

            for (int i = 0; i < Board.Instance.getXLength(); ++i)
            {
                for (int j = 0; j < Board.Instance.getYLength(); ++j)
                {
                    if (gameBoard[i, j] == null) continue;

                    if(gameBoard[i, j].GetType().Equals(typeof(Larva)))
                    {
                        result += heuristicTable[i, j];
                    }
                    else if (gameBoard[i, j].GetType().Equals(typeof(Bird)))
                    {
                        result -= heuristicTable[i, j];
                    }
                }
            }

            return result;
        }

        private void fillTable()
        {
            int xLen = Board.Instance.getXLength();
            int yLen = Board.Instance.getYLength();

            int value = 1;

            for (int i = yLen - 1; i >= 0; --i)
            {
                for (int j = 0; j < xLen; ++j)
                {
                    heuristicTable[j, i] = value;
                    value++;
                }
            }
        }
    }
}
