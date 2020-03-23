using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TronBotFramework.Interface.Version1
{
    public sealed class TronString
    {
        private readonly Board _board;

        public TronString(string tronString)
        {
            var tokens = ParseTokens(tronString);
            _board = ParseDimensions(tokens);
            ParseState(_board, tokens, tronString);
        }

        public Board Create()
        {
            return _board.Clone();
        }

        private static IReadOnlyCollection<Token> ParseTokens(string tronString)
        {
            var tokens = new List<Token>();
            var position = 0;

            while (position < tronString.Length)
            {
                if (TryParseSymbol(tronString, position, out var token))
                {
                    tokens.Add(token);
                    position++;
                }
                else if (TryParseNumber(tronString, position, out var positionIncrement, out token))
                {
                    tokens.Add(token);
                    position += positionIncrement;
                }
                else if (TryParseRowSeparator(tronString, position, out token))
                {
                    tokens.Add(token);
                    position++;
                }
                else
                {
                    var symbol = tronString[position];
                    throw new TronStringParsingException(tronString, $"Unrecognized symbol '{symbol}' at position {position}.");
                }
            }

            return tokens.AsReadOnly();
        }

        private static bool TryParseSymbol(string tronString, int position, out Token token)
        {
            token = default;

            var symbol = tronString[position];
            switch (symbol)
            {
                case 'o':
                    token = new Token(TokenType.Symbol, position, Field.Obstacle, default);
                    return true;
                case 'b':
                    token = new Token(TokenType.Symbol, position, Field.BlueTail, default);
                    return true;
                case 'B':
                    token = new Token(TokenType.Symbol, position, Field.BlueHead, default);
                    return true;
                case 'r':
                    token = new Token(TokenType.Symbol, position, Field.RedTail, default);
                    return true;
                case 'R':
                    token = new Token(TokenType.Symbol, position, Field.RedHead, default);
                    return true;
                default:
                    return false;
            }
        }

        private static bool TryParseNumber(string tronString, int position, out int positionIncrement, out Token token)
        {
            token = default;
            positionIncrement = default;

            var stringBuilder = new StringBuilder();
            var localPosition = position;
            while (localPosition < tronString.Length && char.IsDigit(tronString[localPosition]))
            {
                stringBuilder.Append(tronString[localPosition]);
                localPosition++;
            }

            if (stringBuilder.Length > 0)
            {
                positionIncrement = stringBuilder.Length;
                var number = int.Parse(stringBuilder.ToString());
                token = new Token(TokenType.Number, position, default, number);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool TryParseRowSeparator(string tronString, int position, out Token token)
        {
            token = default;

            if (tronString[position] == '/')
            {
                token = new Token(TokenType.RowSeparator, position, default, default);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Board ParseDimensions(IReadOnlyCollection<Token> tokens)
        {
            var width = tokens.TakeWhile(t => t.Type != TokenType.RowSeparator)
                .Select(t => t.Type switch
                    {
                        TokenType.Symbol => 1,
                        TokenType.Number => t.Number,
                        _ => 0
                    }
                ).Sum();
            var height = tokens.Count(t => t.Type == TokenType.RowSeparator) + 1;

            return new Board(width, height);
        }

        private static void ParseState(Board board, IReadOnlyCollection<Token> tokens, string tronString)
        {
            var x = 0;
            var y = 0;

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Symbol:
                        if (x >= board.Width)
                        {
                            throw new TronStringParsingException(tronString,
                                $"Number of fields for row {y} exceeded. Field over limit defined by symbol at {token.Position}.");
                        }

                        board.SetField(x, y, token.Symbol);
                        x++;
                        break;
                    case TokenType.Number:
                        if (x + token.Number - 1 >= board.Width)
                        {
                            throw new TronStringParsingException(tronString,
                                $"Number of fields for row {y} exceeded. Empty fields over limit defined by symbol at {token.Position}.");
                        }

                        for (var i = 0; i < token.Number; i++)
                        {
                            board.SetField(x, y, Field.Empty);
                            x++;
                        }

                        break;
                    case TokenType.RowSeparator:
                        if (x != board.Width)
                        {
                            throw new TronStringParsingException(tronString,
                                $"There are missing fields for row {y}. Row unexpected ending defined by symbol at {token.Position}.");
                        }

                        x = 0;
                        y++;

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (x != board.Width)
            {
                throw new TronStringParsingException(tronString,
                    $"There are missing fields for row {y}. Tron string unexpectedly ended.");
            }
        }

        private enum TokenType
        {
            Symbol,
            Number,
            RowSeparator
        }

        private struct Token
        {
            public Token(TokenType type, int position, Field symbol, int number)
            {
                Type = type;
                Position = position;
                Symbol = symbol;
                Number = number;
            }

            public TokenType Type { get; }
            public int Position { get; }
            public Field Symbol { get; }
            public int Number { get; }

            public override string ToString()
            {
                return $"{nameof(Type)}: {Type}, {nameof(Position)}: {Position}, {nameof(Symbol)}: {Symbol}, {nameof(Number)}: {Number}";
            }
        }
    }

    public sealed class TronStringParsingException : Exception
    {
        public TronStringParsingException(string rawTronString, string reason) : base($"Cannot parse following tron string: {rawTronString}. {reason}")
        {
            RawTronString = rawTronString;
        }

        public string RawTronString { get; }
    }
}