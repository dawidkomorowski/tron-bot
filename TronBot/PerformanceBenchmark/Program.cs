using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using TronBotFramework;
using TronBotFramework.Interface.Version1;

namespace PerformanceBenchmark
{
    public class TronBotPerformance
    {
        private const Color Color = TronBotFramework.Color.Blue;

        private readonly TronString _tronString =
            new TronString("oooooooooooo/oB9o/o10o/o10o/o10o/o10o/o10o/o10o/o10o/o10o/o9Ro/oooooooooooo");

        private readonly RandBot.TronBot _randBot = new RandBot.TronBot();
        private readonly MinimusMaximus.TronBot _minimusMaximus = new MinimusMaximus.TronBot();
        private readonly MonteusCarlus.TronBot _monteusCarlus = new MonteusCarlus.TronBot();

        // [Benchmark]
        // public void RandBot()
        // {
        //     _randBot.FindMove(_tronString.Create(), Color);
        // }

        [Benchmark]
        public void MinimusMaximus()
        {
            _minimusMaximus.FindMove(_tronString.Create(), Color);
        }

        [Benchmark]
        public void MonteusCarlus()
        {
            _monteusCarlus.FindMove(_tronString.Create(), Color);
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            BenchmarkRunner.Run<TronBotPerformance>();
        }
    }
}