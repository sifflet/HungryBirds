using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    class Board
    {
        private static Board instance;

        private const int X_LENGTH = 8;
        private const int Y_LENGTH = 8;

        private Piece[,] gameBoard;

        public static Board Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Board();
                }
                return instance;
            }
        }

        public int getXLength()
        {
            return X_LENGTH;
        }

        public int getYLength()
        {
            return Y_LENGTH;
        }

        public Piece[,] getGameBoard()
        {
            return this.gameBoard;
        }

        public void printGameBoard()
        {
            Console.WriteLine();
            Console.Write("  ");

            for (int i = 0; i < X_LENGTH; ++i)
            {
                Console.Write((char)('A' + i) + " ");
            }

            Console.WriteLine();
            for (int i = Y_LENGTH; i > 0; --i)
            {
                Console.Write(i + " ");
                for (int j = 0; j < X_LENGTH; ++j)
                {
                    if (gameBoard[j, i - 1] == null)
                    {
                        Console.Write("_ ");
                    }
                    else
                    {
                        if (gameBoard[j, i - 1].GetType().Equals(typeof(Bird)))
                        {
                            Console.Write("B ");
                        }
                        else
                        {
                            Console.Write("L ");
                        }
                    }
                }
                Console.Write(i);
                Console.WriteLine();
            }

            Console.Write("  ");
            for (int i = 0; i < X_LENGTH; ++i)
            {
                Console.Write((char)('A' + i) + " ");
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private Board()
        {
            this.gameBoard = new Piece[X_LENGTH, Y_LENGTH];

            for (int i = 0; i < X_LENGTH; ++i)
            {
                for (int j = 0; j < Y_LENGTH; ++j)
                {
                    this.gameBoard[i, j] = null;
                }
            }
        }
    }
}
