using System;
using TronBotFramework;

namespace FunctionalBenchmark
{
    internal class Program
    {
        private static void Main()
        {
            // MinimusMaximus_Vs_RandBot();
            MonteusCarlus_Vs_RandBot();
        }

        private static void MinimusMaximus_Vs_RandBot()
        {
            Console.WriteLine("MinimusMaximus vs RandBot:");
            Console.WriteLine($"  MinimusMaximus - Blue");
            Console.WriteLine($"  RandBot - Red");
            Console.WriteLine();

            var minimusMaximus = new MinimusMaximus.TronBot();
            var randBot = new RandBot.TronBot();

            var statistics = Duel.RunMultipleParallel(minimusMaximus, randBot, CreateDefaultBoard(), 100);
            Console.WriteLine(statistics);
            Console.WriteLine();
        }
        
        private static void MonteusCarlus_Vs_RandBot()
        {
            Console.WriteLine("MonteusCarlus vs RandBot:");
            Console.WriteLine($"  MonteusCarlus - Blue");
            Console.WriteLine($"  RandBot - Red");
            Console.WriteLine();

            var monteusCarlus = new MonteusCarlus.TronBot();
            var randBot = new RandBot.TronBot();

            var statistics = Duel.RunMultipleParallel(monteusCarlus, randBot, CreateDefaultBoard(), 100);
            Console.WriteLine(statistics);
            Console.WriteLine();
        }

        private static Board CreateDefaultBoard()
        {
            var board = new Board(10, 10);

            for (var x = 0; x < board.Width; x++)
            {
                for (var y = 0; y < board.Height; y++)
                {
                    if (x > 0 && x < board.Width - 1 && y > 0 && y < board.Height - 1)
                    {
                        board.SetField(x, y, Field.Empty);
                    }
                    else
                    {
                        board.SetField(x, y, Field.Obstacle);
                    }
                }
            }

            board.SetField(1, 1, Field.BlueHead);
            board.SetField(8, 8, Field.RedHead);

            return board;
        }
    }
}