using System;
using System.Linq;
using TronBotFramework;

namespace RandBot
{
    public sealed class TronBot : IBot
    {
        private readonly Random _random = new Random();

        public Move FindMove(Board board, Color color)
        {
            var competition = new Competition(board);
            var moves = competition.GetAvailableMoves(color);
            return moves.Any() ? moves.ElementAt(_random.Next(0, moves.Count)) : Move.Up;
        }
    }
}