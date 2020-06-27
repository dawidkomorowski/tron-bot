using System;
using System.Collections.Generic;
using System.Linq;
using TronBotFramework;

namespace MonteusCarlus
{
    internal static class PlayOut
    {
        private static readonly Random Random = new Random();

        public static ScoredMove Play(Board board, Color color, Move move, int numberOfPlayOuts)
        {
            var numberOfWonPlayOuts = 0;

            for (var i = 0; i < numberOfPlayOuts; i++)
            {
                var result = PlayOnce(board.Clone(), color, move);
                var won = color == Color.Blue && result == Result.BlueWon ||
                          color == Color.Red && result == Result.RedWon;

                if (won) numberOfWonPlayOuts++;
            }

            return new ScoredMove(move, numberOfWonPlayOuts, numberOfPlayOuts);
        }

        private static Result PlayOnce(Board board, Color color, Move move)
        {
            var competition = new Competition(board);
            var isFirstPass = true;

            while (true)
            {
                var blueAvailableMoves = competition.GetAvailableMoves(Color.Blue);
                var redAvailableMoves = competition.GetAvailableMoves(Color.Red);

                var blueIsOver = blueAvailableMoves.Count == 0;
                var redIsOver = redAvailableMoves.Count == 0;

                if (blueIsOver && redIsOver) return Result.Draw;
                if (blueIsOver) return Result.RedWon;
                if (redIsOver) return Result.BlueWon;

                var blueMove = isFirstPass && color == Color.Blue ? move : GetRandom(blueAvailableMoves);
                var redMove = isFirstPass && color == Color.Red ? move : GetRandom(redAvailableMoves);

                competition.MakeMove(Color.Blue, blueMove);

                var blueAndRedChosenTheSameField = !competition.GetAvailableMoves(Color.Red).Contains(redMove);

                if (blueAndRedChosenTheSameField) return Result.Draw;

                competition.MakeMove(Color.Red, redMove);

                isFirstPass = false;
            }
        }

        private static Move GetRandom(IReadOnlyCollection<Move> availableMoves) =>
            availableMoves.ElementAt(Random.Next(0, availableMoves.Count));

        private enum Result
        {
            BlueWon,
            RedWon,
            Draw
        }
    }
}