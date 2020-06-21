using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TronBotFramework;

namespace FunctionalBenchmark
{
    public static class Duel
    {
        public enum Result
        {
            Draw,
            BlueWon,
            RedWon
        }

        public readonly struct Statistics
        {
            public Statistics(int total, int draw, int blueWon, int redWon)
            {
                Total = total;
                Draw = draw;
                BlueWon = blueWon;
                RedWon = redWon;
            }

            public int Total { get; }
            public int Draw { get; }
            public int BlueWon { get; }
            public int RedWon { get; }

            public override string ToString() =>
                $"{nameof(Total)}: {Total}, {nameof(Draw)}: {Draw} ({Percent(Draw)}%), {nameof(BlueWon)}: {BlueWon} ({Percent(BlueWon)}%), {nameof(RedWon)}: {RedWon} ({Percent(RedWon)}%)";

            private string Percent(int value) => ((int) ((double) value / Total * 100)).ToString();
        }

        public static Result Run(IBot blue, IBot red, Board initialState)
        {
            var competitionBoard = initialState.Clone();
            var competition = new Competition(competitionBoard);

            while (true)
            {
                var blueMove = blue.FindMove(competitionBoard.Clone(), Color.Blue);
                var redMove = red.FindMove(competitionBoard.Clone(), Color.Red);

                var blueCrashed = !competition.GetAvailableMoves(Color.Blue).Contains(blueMove);
                var redCrashed = !competition.GetAvailableMoves(Color.Red).Contains(redMove);

                if (blueCrashed && redCrashed)
                {
                    return Result.Draw;
                }

                if (blueCrashed)
                {
                    return Result.RedWon;
                }

                if (redCrashed)
                {
                    return Result.BlueWon;
                }

                competition.MakeMove(Color.Blue, blueMove);

                if (!competition.GetAvailableMoves(Color.Red).Contains(redMove))
                {
                    return Result.Draw;
                }

                competition.MakeMove(Color.Red, redMove);
            }
        }

        public static Statistics RunMultiple(IBot blue, IBot red, Board initialState, int numberOfRuns)
        {
            var draw = 0;
            var blueWon = 0;
            var redWon = 0;

            for (var i = 0; i < numberOfRuns; i++)
            {
                Console.Write($"Progress: {i}/{numberOfRuns}\r");

                var result = Run(blue, red, initialState);
                switch (result)
                {
                    case Result.Draw:
                        draw++;
                        break;
                    case Result.BlueWon:
                        blueWon++;
                        break;
                    case Result.RedWon:
                        redWon++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Console.WriteLine($"Progress: {numberOfRuns}/{numberOfRuns}");

            return new Statistics(numberOfRuns, draw, blueWon, redWon);
        }

        public static Statistics RunMultipleParallel(IBot blue, IBot red, Board initialState, int numberOfRuns)
        {
            var progress = 0;
            var draw = 0;
            var blueWon = 0;
            var redWon = 0;

            Parallel.For(0, numberOfRuns, i =>
            {
                Console.Write($"Progress: {progress}/{numberOfRuns}\r");

                var result = Run(blue, red, initialState);
                switch (result)
                {
                    case Result.Draw:
                        Interlocked.Increment(ref draw);
                        break;
                    case Result.BlueWon:
                        Interlocked.Increment(ref blueWon);
                        break;
                    case Result.RedWon:
                        Interlocked.Increment(ref redWon);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                Interlocked.Increment(ref progress);
                Console.Write($"Progress: {progress}/{numberOfRuns}\r");
            });

            Console.WriteLine($"Progress: {progress}/{numberOfRuns}");

            return new Statistics(numberOfRuns, draw, blueWon, redWon);
        }
    }
}