namespace LawsLaboratory.Application.FormulaCompiler.Lexer;

internal sealed class FormulaLexer
{
    private static readonly IReadOnlyDictionary<char, TokenType> SingleCharacterTokens =
        new Dictionary<char, TokenType>
        {
            ['+'] = TokenType.Plus,
            ['-'] = TokenType.Minus,
            ['*'] = TokenType.Multiply,
            ['/'] = TokenType.Divide,
            ['^'] = TokenType.Power,
            ['('] = TokenType.OpenParenthesis,
            [')'] = TokenType.CloseParenthesis,
            ['['] = TokenType.OpenBracket,
            [']'] = TokenType.CloseBracket,
            [','] = TokenType.Comma
        };

    private string _source = string.Empty;
    private int _position;

    public IReadOnlyList<FormulaToken> Tokenize(string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        _source = source;
        _position = 0;

        List<FormulaToken> tokens = new();

        while (!IsAtEnd())
        {
            SkipWhiteSpaces();

            if (IsAtEnd())
            {
                break;
            }

            char current = Peek();

            if (char.IsDigit(current))
            {
                tokens.Add(ReadNumber());
                continue;
            }

            if (char.IsLetter(current) || current == '_')
            {
                tokens.Add(ReadIdentifier());
                continue;
            }

            int tokenPosition = _position;

            current = Advance();

            if (SingleCharacterTokens.TryGetValue(current, out TokenType tokenType))
            {
                tokens.Add(
                    new FormulaToken(
                        tokenType,
                        current.ToString(),
                        tokenPosition));

                continue;
            }

            throw new InvalidOperationException(
                $"Unexpected character '{current}' at position {tokenPosition}.");
        }

        tokens.Add(
            new FormulaToken(
                TokenType.EndOfFile,
                string.Empty,
                _position));

        return tokens;
    }

    private FormulaToken ReadNumber()
    {
        int start = _position;

        while (char.IsDigit(Peek()))
        {
            Advance();
        }

        if (Peek() == '.' || Peek() == ',')
        {
            Advance();

            while (char.IsDigit(Peek()))
            {
                Advance();
            }
        }

        string lexeme = _source[start.._position];

        return new FormulaToken(
            TokenType.Number,
            lexeme,
            start);
    }

    private FormulaToken ReadIdentifier()
    {
        int start = _position;

        while (char.IsLetterOrDigit(Peek()) || Peek() == '_')
        {
            Advance();
        }

        string lexeme = _source[start.._position];

        return new FormulaToken(
            TokenType.Identifier,
            lexeme,
            start);
    }

    private void SkipWhiteSpaces()
    {
        while (char.IsWhiteSpace(Peek()))
        {
            Advance();
        }
    }

    private char Peek()
    {
        return IsAtEnd()
            ? '\0'
            : _source[_position];
    }

    private char PeekNext()
    {
        return _position + 1 >= _source.Length
            ? '\0'
            : _source[_position + 1];
    }

    private bool Match(char expected)
    {
        if (IsAtEnd())
        {
            return false;
        }

        if (_source[_position] != expected)
        {
            return false;
        }

        _position++;
        return true;
    }

    private char Advance()
    {
        return _source[_position++];
    }

    private bool IsAtEnd()
    {
        return _position >= _source.Length;
    }
}