using System.Collections.Generic;
using TronBotFramework;

namespace MonteusCarlus
{
    public sealed class TronBot : IBot
    {
        private const int PlayOutsPerMove = 200;

        public Move FindMove(Board board, Color color)
        {
            var competition = new Competition(board);
            var availableMoves = competition.GetAvailableMoves(color);

            if (availableMoves.Count == 0) return Move.Up;

            var scoredMoves = new List<ScoredMove>();
            foreach (var move in availableMoves)
            {
                scoredMoves.Add(PlayOut.Play(board, color, move, PlayOutsPerMove));
            }

            var bestMove = new ScoredMove(Move.Up, -1, -1);
            foreach (var scoredMove in scoredMoves)
            {
                if (scoredMove.Won > bestMove.Won)
                {
                    bestMove = scoredMove;
                }
            }

            return bestMove.Move;
        }
    }
}