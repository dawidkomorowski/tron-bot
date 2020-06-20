using System;
using System.Linq;
using TronBotFramework;

namespace ComparisonTests
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

            return new Statistics(numberOfRuns, draw, blueWon, redWon);
        }
    }
}