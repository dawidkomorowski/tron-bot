using System;

namespace TronBotFramework
{
    public sealed class TronCompetition
    {
        private readonly TronBoard _tronBoard;

        public TronCompetition(TronBoard tronBoard)
        {
            ValidateTronBoard(tronBoard);
            _tronBoard = tronBoard;
        }

        private static void ValidateTronBoard(TronBoard tronBoard)
        {
            var blueHeadCount = 0;
            var redHeadCount = 0;
            foreach (var field in tronBoard)
            {
                switch (field)
                {
                    case TronBoardField.Empty:
                        break;
                    case TronBoardField.Obstacle:
                        break;
                    case TronBoardField.BlueTail:
                        break;
                    case TronBoardField.BlueHead:
                        blueHeadCount++;
                        break;
                    case TronBoardField.RedTail:
                        break;
                    case TronBoardField.RedHead:
                        redHeadCount++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (blueHeadCount != 1)
                throw new ArgumentException($"{nameof(TronBoard)} should contain exactly one field of type {nameof(TronBoardField.BlueHead)}.",
                    nameof(tronBoard));
            if (redHeadCount != 1)
                throw new ArgumentException($"{nameof(TronBoard)} should contain exactly one field of type {nameof(TronBoardField.RedHead)}.",
                    nameof(tronBoard));

            var xMax = tronBoard.Width - 1;
            var yMax = tronBoard.Height - 1;

            for (var x = 0; x < tronBoard.Width; x++)
            {
                var topBorderField = tronBoard.GetField(x, 0);
                var bottomBorderField = tronBoard.GetField(x, yMax);

                if (topBorderField != TronBoardField.Obstacle)
                    throw new ArgumentException($"{nameof(TronBoard)} is missing an obstacle on position: ({x}, 0).", nameof(tronBoard));
                if (bottomBorderField != TronBoardField.Obstacle)
                    throw new ArgumentException($"{nameof(TronBoard)} is missing an obstacle on position: ({x}, {yMax}).", nameof(tronBoard));
            }

            for (var y = 0; y < tronBoard.Height; y++)
            {
                var leftBorderField = tronBoard.GetField(0, y);
                var rightBorderField = tronBoard.GetField(xMax, y);

                if (leftBorderField != TronBoardField.Obstacle)
                    throw new ArgumentException($"{nameof(TronBoard)} is missing an obstacle on position: (0, {y}).", nameof(tronBoard));
                if (rightBorderField != TronBoardField.Obstacle)
                    throw new ArgumentException($"{nameof(TronBoard)} is missing an obstacle on position: ({xMax}, {y}).", nameof(tronBoard));
            }
        }
    }
}