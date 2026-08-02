namespace LawsLaboratory.Application.FormulaCompiler.Lexer;

public enum TokenType
{
    Number,

    Identifier,

    Plus,
    Minus,
    Multiply,
    Divide,
    Power,

    And,
    Or,
    Not,
    Xor,

    OpenParenthesis,
    CloseParenthesis,

    OpenBracket,
    CloseBracket,

    Comma,

    EndOfFile
}
public sealed class FormulaToken
{
    public TokenType Type { get; }

    public string Lexeme { get; }

    public int Position { get; }

    public FormulaToken(TokenType type, string lexeme, int position)
    {
        Type = type;
        Lexeme = lexeme;
        Position = position;
    }
}
