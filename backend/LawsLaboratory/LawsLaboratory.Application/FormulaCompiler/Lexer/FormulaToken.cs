// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / FormulaCompiler / Lexer
//
// FormulaToken.cs
//
// Defines the lexical token categories produced by FormulaLexer. 
// These values describe the syntax recognized by the lexer and are distinct 
// from the semantic OperatorType and SymbolType representations.
//
// Represents a single lexical token produced by FormulaLexer.
//
// Each token stores its lexical category, original lexeme, and position in
// the source expression so that later compilation stages can report
// meaningful syntax or semantic errors.
// -----------------------------------------------------------------------------


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
