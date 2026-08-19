// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / FormulaCompiler / Parser
//
// FormulaParser.cs
//
// Performs syntactic analysis of the formula language using recursive
// descent parsing.
//
// Converts the token sequence produced by FormulaLexer into a syntax AST
// while enforcing the grammar, operator precedence, associativity, function
// call structure, and relative-position syntax.
//
// The parser does not resolve identifiers, symbols, parameters, or functions.
// Those responsibilities belong to SemanticAnalyzer.
// -----------------------------------------------------------------------------



namespace LawsLaboratory.Application.FormulaCompiler.Parser;

using LawsLaboratory.Application.FormulaCompiler.Lexer;
using LawsLaboratory.Core.Formula;
using LawsLaboratory.Core.Formula.Node;
using LawsLaboratory.Core.SpatialModel.Position;
using System.Globalization;

internal sealed partial class FormulaParser
{
    private IReadOnlyList<FormulaToken> _tokens = null!;

    private int _position;


    public ExpressionNode Parse(
        IReadOnlyList<FormulaToken> tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);

        _tokens = tokens;
        _position = 0;

        ExpressionNode expression =
            ParseExpression();

        Expect(
            TokenType.EndOfFile,
            "Unexpected token after expression.");

        return expression;
    }

    /*
     *
     * Expression
     *      -> LogicalOr
     *
     */

    private ExpressionNode ParseExpression()
    {
        return ParseLogicalOr();
    }

    /*
     *
     * LogicalOr
     *      -> LogicalXor
     *          ( OR LogicalXor )*
     *
     */

    private ExpressionNode ParseLogicalOr()
    {
        ExpressionNode expression =
            ParseLogicalXor();

        while (Match(TokenType.Or))
        {
            ExpressionNode right =
                ParseLogicalXor();

            expression =
                new OperatorNode(
                    OperatorType.Or,
                    new[]
                    {
                        expression,
                        right
                    });
        }

        return expression;
    }

    /*
     *
     * LogicalXor
     *      -> LogicalAnd
     *          ( XOR LogicalAnd )*
     *
     */

    private ExpressionNode ParseLogicalXor()
    {
        ExpressionNode expression =
            ParseLogicalAnd();

        while (Match(TokenType.Xor))
        {
            ExpressionNode right =
                ParseLogicalAnd();

            expression =
                new OperatorNode(
                    OperatorType.Xor,
                    new[]
                    {
                        expression,
                        right
                    });
        }

        return expression;
    }

    /*
     *
     * LogicalAnd
     *      -> Addition
     *          ( AND Addition )*
     *
     */

    private ExpressionNode ParseLogicalAnd()
    {
        ExpressionNode expression =
            ParseAddition();


        while (Match(TokenType.And))
        {
            ExpressionNode right =
                ParseAddition();

            expression =
                new OperatorNode(
                    OperatorType.And,
                    new[]
                    {
                        expression,
                        right
                    });
        }

        return expression;
    }

    /*
     *
     * Addition
     *      -> Multiplication
     *          ( ("+" | "-") Multiplication )*
     *
     */

    private ExpressionNode ParseAddition()
    {
        ExpressionNode expression =
            ParseMultiplication();


        while (Match(
            TokenType.Plus,
            TokenType.Minus))
        {
            TokenType operation =
                Previous().Type;

            ExpressionNode right =
                ParseMultiplication();

            expression =
                new OperatorNode(
                    operation == TokenType.Plus
                        ? OperatorType.Add
                        : OperatorType.Subtract,

                    new[]
                    {
                        expression,
                        right
                    });
        }

        return expression;
    }

    /*
 *
 * Multiplication
 *      -> Power
 *          ( ("*" | "/") Power )*
 *
 */

    private ExpressionNode ParseMultiplication()
    {
        ExpressionNode expression =
            ParsePower();


        while (Match(
            TokenType.Multiply,
            TokenType.Divide))
        {
            TokenType operation =
                Previous().Type;

            ExpressionNode right =
                ParsePower();

            expression =
                new OperatorNode(
                    operation == TokenType.Multiply
                        ? OperatorType.Multiply
                        : OperatorType.Divide,

                    new[]
                    {
                        expression,
                        right
                    });
        }

        return expression;
    }

    /*
     *
     * Power
     *      -> Unary
     *          ( "^" Unary )*
     *
     */

    private ExpressionNode ParsePower()
    {
        ExpressionNode expression =
            ParseUnary();


        if (Match(TokenType.Power))
        {
            ExpressionNode right =
                ParsePower();

            return new OperatorNode(
                OperatorType.Power,

                new[]
                {
                expression,
                right
                });
        }

        return expression;
    }

    /*
     *
     * Unary
     *      -> "-" Unary
     *      -> "!" Unary
     *      -> Primary
     *
     */

    private ExpressionNode ParseUnary()
    {
        if (Match(TokenType.Minus))
        {
            return new OperatorNode(
                OperatorType.Subtract,

                new ExpressionNode[]
                {
                    new ConstantNode(0),
                    ParseUnary()
                });
        }

        if (Match(TokenType.Not))
        {
            return new OperatorNode(
                OperatorType.Not,

                new ExpressionNode[]
                {
                    ParseUnary()
                });
        }

        return ParsePrimary();
    }

    /*
     *
     * Primary
     *      -> Number
     *      -> Identifier
     *      -> FunctionCall
     *      -> "(" Expression ")"
     *
     */

    private ExpressionNode ParsePrimary()
    {
        if (Match(TokenType.Number))
        {
            double value =
                double.Parse(
                    Previous().Lexeme,
                    System.Globalization.CultureInfo.InvariantCulture);

            return new ConstantNode(value);
        }

        if (Match(TokenType.Identifier))
        {
            string name =
                Previous().Lexeme;


            if (Match(TokenType.OpenParenthesis))
            {
                return ParseFunctionCall(name);
            }

            PlanePosition position =
                new PlanePosition(0, 0);

            if (Match(TokenType.OpenBracket))
            {
                position =
                    ParseRelativePosition();
            }


            return new IdentifierNode(
                name,
                position);
        }

        if (Match(TokenType.OpenParenthesis))
        {
            ExpressionNode expression =
                ParseExpression();


            Expect(
                TokenType.CloseParenthesis,
                "Expected ')' after expression.");


            return expression;
        }

        throw Error(
            "Expected expression.");
    }

    /*
    *
    * FunctionCall
    *
    * Example:
    *
    *     sin(x)
    *
    * produces:
    *
    *     FunctionCallNode
    *
    *     Name = "sin"
    *
    *     Arguments:
    *         IdentifierNode("x")
    *
    */

    private ExpressionNode ParseFunctionCall(
        string name)
    {
        List<ExpressionNode> arguments =
            new();

        if (!Check(TokenType.CloseParenthesis))
        {
            do
            {
                arguments.Add(
                    ParseExpression());
            }
            while (Match(TokenType.Comma));
        }

        Expect(
            TokenType.CloseParenthesis,
            "Expected ')' after function arguments.");


        return new FunctionCallNode(
            name,
            arguments);
    }

    private PlanePosition ParseRelativePosition()
    {
        int x =
            ParseSignedInteger();


        Expect(
            TokenType.Comma,
            "Expected ',' between coordinates.");


        int y =
            ParseSignedInteger();


        Expect(
            TokenType.CloseBracket,
            "Expected ']' after relative position.");


        return new PlanePosition(x, y);
    }

    private int ParseSignedInteger()
    {
        int sign = 1;

        if (Match(TokenType.Minus))
        {
            sign = -1;
        }

        Expect(
            TokenType.Number,
            "Expected integer coordinate.");

        return sign *
            int.Parse(
                Previous().Lexeme,
                CultureInfo.InvariantCulture);
    }

    /*
    *
    * Token navigation helper methods
    *
    */

    private bool Match(
        params TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (Check(type))
            {
                Advance();

                return true;
            }
        }

        return false;
    }

    private bool Check(
        TokenType type)
    {
        return Peek().Type == type;
    }

    private FormulaToken Advance()
    {
        if (!IsAtEnd())
        {
            _position++;
        }

        return Previous();
    }


    private FormulaToken Peek()
    {
        return _tokens[_position];
    }


    private FormulaToken Previous()
    {
        return _tokens[_position - 1];
    }


    private void Expect(
        TokenType type,
        string message)
    {
        if (Check(type))
        {
            Advance();

            return;
        }

        throw Error(message);
    }


    private Exception Error(
        string message)
    {
        FormulaToken token =
            Peek();

        return new InvalidOperationException(
            $"{message} Token '{token.Lexeme}' at position {token.Position}.");
    }


    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EndOfFile;
    }
}
