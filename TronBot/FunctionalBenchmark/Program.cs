using System;
using TronBotFramework.Interface.Version1;

namespace FunctionalBenchmark
{
    internal static class Program
    {
        private static readonly TronString TronString =
            new TronString("oooooooooooo/oB9o/o10o/o10o/o10o/o10o/o10o/o10o/o10o/o10o/o9Ro/oooooooooooo");

        private static void Main()
        {
            // MinimusMaximus_Vs_RandBot();
            // MonteusCarlus_Vs_RandBot();
            MinimusMaximus_Vs_MonteusCarlus();
        }

        private static void MinimusMaximus_Vs_RandBot()
        {
            Console.WriteLine("MinimusMaximus vs RandBot:");
            Console.WriteLine($"  MinimusMaximus - Blue");
            Console.WriteLine($"  RandBot - Red");
            Console.WriteLine();

            var minimusMaximus = new MinimusMaximus.TronBot();
            var randBot = new RandBot.TronBot();

            var statistics = Duel.RunMultipleParallel(minimusMaximus, randBot, TronString.Create(), 100);
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

            var statistics = Duel.RunMultipleParallel(monteusCarlus, randBot, TronString.Create(), 100);
            Console.WriteLine(statistics);
            Console.WriteLine();
        }

        private static void MinimusMaximus_Vs_MonteusCarlus()
        {
            Console.WriteLine("MinimusMaximus vs MonteusCarlus:");
            Console.WriteLine($"  MinimusMaximus - Blue");
            Console.WriteLine($"  MonteusCarlus - Red");
            Console.WriteLine();

            var minimusMaximus = new MinimusMaximus.TronBot();
            var monteusCarlus = new MonteusCarlus.TronBot();

            var statistics = Duel.RunMultipleParallel(minimusMaximus, monteusCarlus, TronString.Create(), 100);
            Console.WriteLine(statistics);
            Console.WriteLine();
        }
    }
}