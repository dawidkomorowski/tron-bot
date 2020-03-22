using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TronBotFramework
{
    public sealed class Board : IEnumerable<Field>
    {
        private readonly Field[] _board;

        public Board(int width, int height)
        {
            if (width <= 0) throw new ArgumentException("Width must be positive.", nameof(width));
            if (height <= 0) throw new ArgumentException("Height must be positive.", nameof(height));

            Width = width;
            Height = height;

            _board = new Field[width * height];
            for (var i = 0; i < _board.Length; i++)
            {
                _board[i] = Field.Empty;
            }
        }

        public int Width { get; }
        public int Height { get; }

        public Field GetField(int x, int y)
        {
            ValidateCoordinates(x, y);
            return _board[y * Width + x];
        }

        public void SetField(int x, int y, Field field)
        {
            ValidateCoordinates(x, y);
            _board[y * Width + x] = field;
        }

        public Board Clone()
        {
            var clone = new Board(Width, Height);

            for (var i = 0; i < _board.Length; i++)
            {
                clone._board[i] = _board[i];
            }

            return clone;
        }

        public IEnumerator<Field> GetEnumerator()
        {
            return _board.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ValidateCoordinates(int x, int y)
        {
            var xMax = Width - 1;
            var yMax = Height - 1;
            if (x < 0 || x > xMax) throw new ArgumentOutOfRangeException(nameof(x), x, $"X must be in range (0, {xMax}).");
            if (y < 0 || y > yMax) throw new ArgumentOutOfRangeException(nameof(y), y, $"Y must be in range (0, {yMax}).");
        }
    }
}