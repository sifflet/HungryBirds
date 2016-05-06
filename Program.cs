using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HungryBirds
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller c = new Controller();
            c.initializeGame();

            Type aiPiece = getAIPieceType();

            if (aiPiece == null)
            {
                c.play();
            }
            else
            {
                c.playWithAI(aiPiece);
            }
            Console.Read();
        }

        static Type getAIPieceType()
        {
            String input = String.Empty;
            Type type = null;

            do{
                Console.Write("Play against: ");
                input = Console.ReadLine();
            }while(!input.Equals("Larva") && !input.Equals("Birds") && !input.Equals("Human"));

            switch (input)
            {
                case "Larva":
                    type = typeof(Larva);
                    break;
                case "Birds":
                    type = typeof(Bird);
                    break;
                case "Human":
                    type = null;
                    break;
            }

            return type;
        }
    }
}
