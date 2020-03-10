using System;

namespace TronBotFramework
{
    public sealed class Competition
    {
        private readonly Board _board;

        public Competition(Board board)
        {
            ValidateBoard(board);
            _board = board;
        }

        private static void ValidateBoard(Board board)
        {
            var blueHeadCount = 0;
            var redHeadCount = 0;
            foreach (var field in board)
            {
                switch (field)
                {
                    case Field.Empty:
                        break;
                    case Field.Obstacle:
                        break;
                    case Field.BlueTail:
                        break;
                    case Field.BlueHead:
                        blueHeadCount++;
                        break;
                    case Field.RedTail:
                        break;
                    case Field.RedHead:
                        redHeadCount++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (blueHeadCount != 1)
                throw new ArgumentException($"{nameof(Board)} should contain exactly one field of type {nameof(Field.BlueHead)}.",
                    nameof(board));
            if (redHeadCount != 1)
                throw new ArgumentException($"{nameof(Board)} should contain exactly one field of type {nameof(Field.RedHead)}.",
                    nameof(board));

            var xMax = board.Width - 1;
            var yMax = board.Height - 1;

            for (var x = 0; x < board.Width; x++)
            {
                var topBorderField = board.GetField(x, 0);
                var bottomBorderField = board.GetField(x, yMax);

                if (topBorderField != Field.Obstacle)
                    throw new ArgumentException($"{nameof(Board)} is missing an obstacle on position: ({x}, 0).", nameof(board));
                if (bottomBorderField != Field.Obstacle)
                    throw new ArgumentException($"{nameof(Board)} is missing an obstacle on position: ({x}, {yMax}).", nameof(board));
            }

            for (var y = 0; y < board.Height; y++)
            {
                var leftBorderField = board.GetField(0, y);
                var rightBorderField = board.GetField(xMax, y);

                if (leftBorderField != Field.Obstacle)
                    throw new ArgumentException($"{nameof(Board)} is missing an obstacle on position: (0, {y}).", nameof(board));
                if (rightBorderField != Field.Obstacle)
                    throw new ArgumentException($"{nameof(Board)} is missing an obstacle on position: ({xMax}, {y}).", nameof(board));
            }
        }
    }
}