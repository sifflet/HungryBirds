using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    class GameState
    {
        private int xLength;
        private int yLength;
        private GameState parentState;
        private List<GameState> childStates;
        private List<Piece> birds;
        private Piece larva;
        private Piece[,] gameBoard;
        private double minMaxValue;
        private double heuristicValue; 
        private String move;

        /// <summary>
        /// Constructor for root game state in the tree(current game state)
        /// </summary>
        public GameState()
        {
            xLength = Board.Instance.getXLength();
            yLength = Board.Instance.getYLength();
            move = null;

            parentState = null;
            childStates = new List<GameState>();
            birds = new List<Piece>();

            gameBoard = deepCopyBoard(Board.Instance.getGameBoard());
        }

        /// <summary>
        /// Constructor for root's descendants game states
        /// </summary>
        /// <param name="parentState">The parent State</param>
        /// <param name="move">The move required to get to this state</param>
        public GameState(GameState parentState, int[] piecePosition, int[] move)
        {
            xLength = Board.Instance.getXLength();
            yLength = Board.Instance.getYLength();

            this.parentState = parentState;
            this.childStates = new List<GameState>();
            birds = new List<Piece>();

            this.gameBoard = deepCopyBoard(parentState.getGameBoard());

            computeStateGameBoard(piecePosition, move);
            computeMoveString(piecePosition[0], piecePosition[1], move[0], move[1]);
        }

        #region Accessors
        public Piece[,] getGameBoard()
        {
            return this.gameBoard;
        }
        public String getMove()
        {
            return this.move;
        }
        public void setMinMaxValue(double value)
        {
            this.minMaxValue = value;
        }
        public double getMinMaxValue()
        {
            return this.minMaxValue;
        }
        public void setHeuristicValue(double value)
        {
            this.heuristicValue = value;
        }
        public double getHeuristicValue()
        {
            return this.heuristicValue;
        }
        public Piece getLarva()
        {
            return this.larva;
        }
        #endregion

        /// <summary>
        /// Creates the new game board from the parent state's game board
        /// and the required move to get to this state.
        /// </summary>
        private void computeStateGameBoard(int[] piecePosition, int[] move)
        {
            int posX = piecePosition[0];
            int posY = piecePosition[1];

            int moveX = move[0];
            int moveY = move[1];

            gameBoard[posX, posY].move(moveX, moveY);
            updatePieces();
        }

        public void createChildStates(Type turn)
        {
            for (int i = 0; i < xLength; ++i)
            {
                for (int j = 0; j < yLength; ++j)
                {
                    if (gameBoard[i, j] == null) continue;

                    if (gameBoard[i, j].GetType().Equals(turn))
                    {
                        for (int k = 0; k < gameBoard[i, j].getPossibleMoves().Count; ++k)
                        {
                            childStates.Add(new GameState(this, new int[2] { i, j }, gameBoard[i, j].getPossibleMoves()[k]));
                        }
                    }
                }
            }
        }

        public List<GameState> getChildStates()
        {
            return this.childStates;
        }

        public void printGameState()
        {
            Console.WriteLine();
            Console.Write("  ");

            for (int i = 0; i < xLength; ++i)
            {
                Console.Write((char)('A' + i) + " ");
            }

            Console.WriteLine();
            for (int i = yLength; i > 0; --i)
            {
                Console.Write(i + " ");
                for (int j = 0; j < xLength; ++j)
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
            for (int i = 0; i < xLength; ++i)
            {
                Console.Write((char)('A' + i) + " ");
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private void movePiece(int posX, int posY, int moveX, int moveY)
        {
            Piece piece = gameBoard[posX, posY];

            gameBoard[posX, posY] = null;
            gameBoard[moveX, moveY] = piece;
            updatePieces();
        }

        private void computeMoveString(int posX, int posY, int moveX, int moveY)
        {
            move += (char)(posX + 'A');
            move += (char)(posY + '1');
            move += ' ';
            move += (char)(moveX + 'A');
            move += (char)(moveY + '1');
        }

        /// <summary>
        /// Creates an entirely new copy of a gameBoard so we can modify it
        /// without changing the real board.
        /// </summary>
        /// <param name="gameBoard">game board to copy</param>
        /// <returns>a new copy of a game board</returns>
        private Piece[,] deepCopyBoard(Piece[,] gameBoard)
        {
            Piece[,] result = new Piece[xLength, yLength];

            for (int i = 0; i < xLength; ++i)
            {
                for (int j = 0; j < yLength; ++j)
                {
                    result[i, j] = null;
                    if (gameBoard[i, j] == null) continue;

                    if (gameBoard[i, j].GetType().Equals(typeof(Larva)))
                    {
                        result[i, j] = new Larva(i, j, result);
                        larva = result[i, j];
                    }
                    else if (gameBoard[i, j].GetType().Equals(typeof(Bird)))
                    {
                        result[i, j] = new Bird(i, j, result);
                        birds.Add(result[i, j]);
                    }
                }
            }

            updatePieces();

            return result;
        }

        private void updatePieces()
        {
            larva.update();
            foreach (Piece bird in birds)
            {
                bird.update();
            }
        }
    }
}
