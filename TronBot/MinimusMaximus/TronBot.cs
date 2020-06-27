using System;
using System.Collections.Generic;
using TronBotFramework;

namespace MinimusMaximus
{
    public sealed class TronBot : IBot
    {
        private const int MaxDepth = 14;

        public Move FindMove(Board board, Color color)
        {
            var competition = new Competition(board);
            var bestMove = MiniMax(competition, new MiniMaxColor(color, color), MaxDepth);
            return bestMove.Move;
        }

        private static ScoredMove MiniMax(Competition competition, MiniMaxColor color, int depth)
        {
            var availableMoves = competition.GetAvailableMoves(color.Value);

            if (depth == 0 || availableMoves.Count == 0)
            {
                var score = Evaluate(competition, color);
                return new ScoredMove(Move.Up, score);
            }

            var scoredMoves = new List<ScoredMove>();
            foreach (var move in availableMoves)
            {
                competition.MakeMove(color.Value, move);
                var scoredMove = MiniMax(competition, color.Opposite(), depth - 1);
                competition.RevertMove(color.Value, move);
                scoredMoves.Add(new ScoredMove(move, scoredMove.Score));
            }

            var bestMove = GetInfinityMove(color.IsMaximizing);
            foreach (var move in scoredMoves)
            {
                if (color.IsMaximizing)
                {
                    if (move.Score > bestMove.Score)
                    {
                        bestMove = move;
                    }
                }
                else
                {
                    if (move.Score < bestMove.Score)
                    {
                        bestMove = move;
                    }
                }
            }

            return bestMove;
        }

        private static int Evaluate(Competition competition, MiniMaxColor color)
        {
            var myScore = GetScoreForColor(competition, color.Value);
            var opponentScore = GetScoreForColor(competition, color.Opposite().Value);

            var score = myScore - opponentScore;

            return color.IsMaximizing ? score : -score;
        }

        private static int GetScoreForColor(Competition competition, Color color)
        {
            var numberOfAvailableMoves = competition.GetAvailableMoves(color).Count;
            return numberOfAvailableMoves > 0 ? numberOfAvailableMoves : -1000;
        }

        private static ScoredMove GetInfinityMove(bool isMaximizing) =>
            isMaximizing ? new ScoredMove(Move.Up, int.MinValue) : new ScoredMove(Move.Up, int.MaxValue);

        private readonly struct ScoredMove
        {
            public ScoredMove(Move move, int score)
            {
                Move = move;
                Score = score;
            }

            public Move Move { get; }
            public int Score { get; }
        }

        private readonly struct MiniMaxColor
        {
            private readonly Color _maximizingColor;

            public MiniMaxColor(Color value, Color maximizingColor)
            {
                Value = value;
                _maximizingColor = maximizingColor;
            }

            public Color Value { get; }
            public bool IsMaximizing => Value == _maximizingColor;

            public MiniMaxColor Opposite() => Value switch
            {
                Color.Blue => new MiniMaxColor(Color.Red, _maximizingColor),
                Color.Red => new MiniMaxColor(Color.Blue, _maximizingColor),
                _ => throw new ArgumentOutOfRangeException(nameof(Value), Value, "Invalid value.")
            };
        }
    }
}