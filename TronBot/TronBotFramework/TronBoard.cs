using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TronBotFramework
{
    public sealed class TronBoard : IEnumerable<TronBoardField>
    {
        private readonly TronBoardField[] _board;

        public TronBoard(int width, int height)
        {
            if (width <= 0) throw new ArgumentException("Width must be positive.", nameof(width));
            if (height <= 0) throw new ArgumentException("Height must be positive.", nameof(height));

            Width = width;
            Height = height;

            _board = new TronBoardField[width * height];
            for (var i = 0; i < _board.Length; i++)
            {
                _board[i] = TronBoardField.Empty;
            }
        }

        public int Width { get; }
        public int Height { get; }

        public TronBoardField GetField(int x, int y)
        {
            ValidateCoordinates(x, y);
            return _board[y * Width + x];
        }

        public void SetField(int x, int y, TronBoardField field)
        {
            ValidateCoordinates(x, y);
            _board[y * Width + x] = field;
        }

        private void ValidateCoordinates(int x, int y)
        {
            var maxWidth = Width - 1;
            var maxHeight = Height - 1;
            if (x < 0 || x > maxWidth) throw new ArgumentOutOfRangeException(nameof(x), x, $"X must be in range (0, {maxWidth}).");
            if (y < 0 || y > maxHeight) throw new ArgumentOutOfRangeException(nameof(y), y, $"Y must be in range (0, {maxHeight}).");
        }

        public IEnumerator<TronBoardField> GetEnumerator()
        {
            return _board.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}