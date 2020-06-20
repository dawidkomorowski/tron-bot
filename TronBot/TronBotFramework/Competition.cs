using System;
using System.Collections.Generic;
using System.Linq;

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

        public (int X, int Y) BluePosition => GetPositionOfHead(Color.Blue);
        public (int X, int Y) RedPosition => GetPositionOfHead(Color.Red);

        public IReadOnlyCollection<Move> GetAvailableMoves(Color color)
        {
            var moves = new List<Move>();

            if (GetFieldBasedOnMove(color, Move.Up) == Field.Empty) moves.Add(Move.Up);
            if (GetFieldBasedOnMove(color, Move.Down) == Field.Empty) moves.Add(Move.Down);
            if (GetFieldBasedOnMove(color, Move.Left) == Field.Empty) moves.Add(Move.Left);
            if (GetFieldBasedOnMove(color, Move.Right) == Field.Empty) moves.Add(Move.Right);

            return moves.AsReadOnly();
        }

        public Field GetField(int x, int y) => _board.GetField(x, y);

        public void MakeMove(Color color, Move move)
        {
            if (GetAvailableMoves(color).Contains(move) == false)
            {
                throw new ArgumentException($"Move {move} is not available at current state for {color}.", nameof(move));
            }

            (int X, int Y) position;
            Field tail;
            Field head;
            switch (color)
            {
                case Color.Blue:
                    position = BluePosition;
                    tail = Field.BlueTail;
                    head = Field.BlueHead;
                    break;
                case Color.Red:
                    position = RedPosition;
                    tail = Field.RedTail;
                    head = Field.RedHead;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, "Incorrect color provided.");
            }

            var (x, y) = Transform(position, move);

            _board.SetField(position.X, position.Y, tail);
            _board.SetField(x, y, head);
        }

        public void RevertMove(Color color, Move move)
        {
            var oppositeMove = Opposite(move);

            (int X, int Y) position;
            Field tail;
            Field head;
            switch (color)
            {
                case Color.Blue:
                    position = BluePosition;
                    tail = Field.BlueTail;
                    head = Field.BlueHead;
                    break;
                case Color.Red:
                    position = RedPosition;
                    tail = Field.RedTail;
                    head = Field.RedHead;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, "Incorrect color provided.");
            }

            var (x, y) = Transform(position, oppositeMove);

            if (GetField(x, y) != tail)
            {
                throw new ArgumentException($"Move {move} is impossible to revert at current state for {color}.", nameof(move));
            }

            _board.SetField(position.X, position.Y, Field.Empty);
            _board.SetField(x, y, head);
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

        private (int X, int Y) GetPositionOfHead(Color color)
        {
            for (var x = 0; x < _board.Width; x++)
            {
                for (var y = 0; y < _board.Height; y++)
                {
                    var field = _board.GetField(x, y);
                    if (color == Color.Blue && field == Field.BlueHead
                        || color == Color.Red && field == Field.RedHead) return (x, y);
                }
            }

            throw new InvalidOperationException("Head position not found.");
        }

        private Field GetFieldBasedOnMove(Color color, Move move)
        {
            var position = GetPositionOfHead(color);
            var (x, y) = Transform(position, move);
            return _board.GetField(x, y);
        }

        private static (int X, int Y) Transform((int X, int Y) position, Move move) => move switch
        {
            Move.Up => (position.X, position.Y - 1),
            Move.Down => (position.X, position.Y + 1),
            Move.Left => (position.X - 1, position.Y),
            Move.Right => (position.X + 1, position.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(move), move, "Incorrect move provided.")
        };

        private static Move Opposite(Move move) => move switch
        {
            Move.Up => Move.Down,
            Move.Down => Move.Up,
            Move.Left => Move.Right,
            Move.Right => Move.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(move), move, "Incorrect move provided.")
        };
    }
}