using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    class Controller
    {
        private Piece[,] gameBoard;
        private Piece larva;
        private List<Piece> birds;
        private String winner;

        public Controller()
        {
            this.gameBoard = Board.Instance.getGameBoard();
            this.birds = new List<Piece>();
            this.winner = String.Empty;
        }

        public void initializeGame()
        {
            gameBoard[0, 0] = new Bird(0, 0, gameBoard);
            birds.Add(gameBoard[0, 0]);

            gameBoard[2, 0] = new Bird(2, 0, gameBoard);
            birds.Add(gameBoard[2, 0]);

            gameBoard[4, 0] = new Bird(4, 0, gameBoard);
            birds.Add(gameBoard[4, 0]);

            gameBoard[6, 0] = new Bird(6, 0, gameBoard);
            birds.Add(gameBoard[6, 0]);

            gameBoard[3, 1] = new Larva(3, 1, gameBoard);
            larva = gameBoard[3, 1];

            updatePieces();
        }

        public void play()
        {
            String input = String.Empty;
            Type currentPlayer = typeof(Larva);
            bool legalMove = false;

            Board.Instance.printGameBoard();

            do
            {
                Console.Write(currentPlayer.Name + " turn:");

                legalMove = false;
                while (!legalMove)
                {
                    input = Console.ReadLine();
                    legalMove = computeMove(input, currentPlayer);

                    if (!legalMove)
                    {
                        Console.WriteLine("Illegal Move");
                        Console.Write(currentPlayer.Name + " turn:");
                    }
                }

                getWinner();
                Board.Instance.printGameBoard();

                if (!winner.Equals(String.Empty)) break;

                if (currentPlayer.Equals(typeof(Larva)))
                {
                    currentPlayer = typeof(Bird);
                }
                else
                {
                    currentPlayer = typeof(Larva);
                }
            } while (true);

            Console.WriteLine("player " + winner + " wins");
        }

        public void playWithAI(Type AIPiece)
        {
            AIPlayer ai = new AIPlayer(AIPiece, new Heuristic2(), 5);
            String input = String.Empty;
            Type currentPlayer = typeof(Larva);
            bool legalMove = false;

            Console.WriteLine("*******************");
            Board.Instance.printGameBoard();

            do
            {
                Console.Write(currentPlayer.Name + " turn:");

                legalMove = false;
                while (!legalMove)
                {
                    if (currentPlayer.Equals(AIPiece))
                    {
                        GameState moveState = ai.play();
                        input = moveState.getMove();
                        Console.WriteLine("AI plays " + input + ", MinMax value: " + moveState.getMinMaxValue());
                    }
                    else
                    {
                        input = Console.ReadLine();
                    }
                 
                    legalMove = computeMove(input, currentPlayer);

                    if (!legalMove)
                    {
                        Console.WriteLine("Illegal Move");
                        Console.Write(currentPlayer.Name + " turn:");
                    }
                }

                getWinner();
                Console.WriteLine("*******************");
                Board.Instance.printGameBoard();

                if (!winner.Equals(String.Empty)) break;

                if (currentPlayer.Equals(typeof(Larva)))
                {
                    currentPlayer = typeof(Bird);
                }
                else
                {
                    currentPlayer = typeof(Larva);
                }
            } while (true);

            Console.WriteLine("player " + winner + " wins");
        }

        private bool computeMove(String move, System.Type pieceType)
        {
            bool valid = false;

            if (move.Length != 5) return false;
            if (move[2] != ' ') return false;
            if(move[0] < 'A' || move[0] > 'H') return false;
            if (move[1] < '1' || move[1] > '8') return false;
            if (move[3] < 'A' || move[3] > 'H') return false;
            if (move[4] < '1' || move[4] > '8') return false;

            int pieceX = move[0] - 'A';
            int pieceY = move[1] - '1';
            int moveX = move[3] - 'A';
            int moveY = move[4] - '1';

            Piece piece = Board.Instance.getGameBoard()[pieceX, pieceY];

            if(piece == null) return false;
            if (!piece.GetType().Equals(pieceType)) return false;

            valid = piece.move(moveX, moveY);
            if (valid) updatePieces();

            return valid;
        }

        private void getWinner()
        {
            // if the larva got to the fence
            if ((larva as Larva).wins())
            {
                winner = "Larva";
                return;
            }

            // if the larva can't move
            if ((larva as Larva).loses())
            {
                winner = "Birds";
                return;
            }

            // if birds can't move
            bool larvaWins = true;
            foreach (Piece bird in birds)
            {
                if (bird.getPossibleMoves().Count > 0) larvaWins = false;
            }

            if (larvaWins) winner = "Larva";
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
