using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    class Bird:Piece
    {
        private List<int[]> possibleMoveSet;
        private int posX;
        private int posY;
        private Piece[,] gameBoard;

        public Bird(int x, int y, Piece[,] gameBoard)
        {
            this.possibleMoveSet = new List<int[]>();
            this.posX = x;
            this.posY = y;
            this.gameBoard = gameBoard;
        }

        int Piece.getPosX()
        {
            return this.posX;
        }

        int Piece.getPosY()
        {
            return this.posY;
        }

        bool Piece.move(int x, int y)
        {
            bool legal = validateMove(x, y);

            if (legal)
            {
                gameBoard[posX, posY] = null;
                posX = x;
                posY = y;
                gameBoard[posX, posY] = this;
            }

            return legal;
        }

        List<int[]> Piece.getPossibleMoves()
        {
            return this.possibleMoveSet;
        }

        // update the possible move set
        void Piece.update()
        {
            possibleMoveSet.Clear();
            possibleMoveSet.Add(new int[2]{posX - 1, posY + 1});
            possibleMoveSet.Add(new int[2] {posX + 1, posY + 1});

            List<int[]> validMoves = new List<int[]>();

            int boardXLength = Board.Instance.getXLength();
            int boardYLength = Board.Instance.getYLength();

            foreach (int[] move in possibleMoveSet)
            {
                if (move[0] < boardXLength && move[1] < boardYLength)
                {
                    if (move[0] >= 0 && move[1] >= 0 && gameBoard[move[0], move[1]] == null)
                    {
                        validMoves.Add(move);
                    }
                }
            }

            possibleMoveSet = validMoves;
        }

        private bool validateMove(int x, int y)
        {
            bool legal = false;

            foreach (int[] move in possibleMoveSet)
            {
                if (move[0] == x && move[1] == y && gameBoard[x, y] == null)
                {
                    // its a legal move
                    legal = true;
                    break;
                }
            }

            return legal;
        }
    }
}
