// -----------------------------------------------------------------------------
// LawsLaboratory
// Tests / SimulationTest
//
// FormulaCompilerTests.cs
//
// Verifies the observable contract of FormulaCompiler.
//
// The tests treat FormulaCompiler as a black box. They validate that a source
// formula is transformed into the expected CompiledExpression, including:
// - executable expression programs;
// - variable reference ordering;
// - relative parameter positions;
// - symbolic constants;
// - mathematical and logical operators;
// - optional optimization;
// - invalid input handling.
//
// Individual compiler stages such as lexing, parsing, semantic analysis, and
// optimization are intentionally not tested in isolation here.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.FormulaCompiler;
using LawsLaboratory.Application.FormulaCompiler.Semantic;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Parameter;
using LawsLaboratory.Core.Formula;
using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.Formula.Program;
using LawsLaboratory.Core.SpatialModel.Position;


namespace LawsLaboratory.Tests.FormulaCompilerTests;

public sealed class FormulaCompilerTests
{
    private static ParameterRegistry CreateParameterRegistry()
    {
        return new ParameterRegistry(
            new[]
            {
                "a",
                "b",
                "temperature",
                "pressure",
                "enabled"
            });
    }


    private static FormulaCompiler CreateCompiler()
    {
        return new FormulaCompiler(
            CreateParameterRegistry());
    }


    private static void AssertProgram(
        CompiledExpression expression,
        params ExpressionInstruction[] expected)
    {
        Assert.Equal(
            expected.Length,
            expression.Program.Count);

        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(
                expected[i],
                expression.Program[i]);
        }
    }


    private static void AssertVariableReferences(
        CompiledExpression expression,
        params VariableReference[] expected)
    {
        IReadOnlyList<VariableReference> references =
            expression.GetVariableReferences();

        Assert.Equal(
            expected.Length,
            references.Count);

        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(
                expected[i],
                references[i]);
        }
    }


    [Fact]
    public void Compile_Constant_ReturnsExpectedConstantInstruction()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "42",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                42));

        AssertVariableReferences(
            expression);
    }


    [Fact]
    public void Compile_Variable_ReturnsExpectedVariableInstructionAndReference()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "temperature",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0));

        AssertVariableReferences(
            expression,
            new VariableReference(
                2,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_VariableWithRelativePosition_ReturnsExpectedReference()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "temperature[1,-2]",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0));

        AssertVariableReferences(
            expression,
            new VariableReference(
                2,
                new PlanePosition(1, -2)));
    }


    [Fact]
    public void Compile_Addition_ReturnsExpectedPostfixProgram()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a + b",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)),
            new VariableReference(
                1,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_OperatorPrecedence_ReturnsExpectedPostfixProgram()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a + b * 2",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Multiply),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)),
            new VariableReference(
                1,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_ParenthesizedExpression_ReturnsExpectedPostfixProgram()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "(a + b) * 2",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Multiply));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)),
            new VariableReference(
                1,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_Subtraction_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a - b",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Subtract));
    }


    [Fact]
    public void Compile_Multiplication_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a * b",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Multiply));
    }


    [Fact]
    public void Compile_Division_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a / b",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Divide));
    }


    [Fact]
    public void Compile_Power_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a ^ b",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Power));
    }


    [Fact]
    public void Compile_MultipleVariablesWithRelativePositions_PreservesReferenceOrder()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a[1,0] + a[-1,0]",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(1, 0)),
            new VariableReference(
                0,
                new PlanePosition(-1, 0)));
    }


    [Fact]
    public void Compile_RepeatedParameterOccurrences_CreatesDistinctVariableReferences()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a + a + b",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)),
            new VariableReference(
                0,
                new PlanePosition(0, 0)),
            new VariableReference(
                1,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_Sin_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "sin(a)",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Sin));
    }


    [Fact]
    public void Compile_Cos_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "cos(a)",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Cos));
    }


    [Fact]
    public void Compile_SquareRoot_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "sqrt(a)",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Sqrt));
    }


    [Fact]
    public void Compile_NaturalLogarithm_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "ln(a)",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Ln));
    }


    [Fact]
    public void Compile_LogarithmWithBase_ReturnsExpectedPostfixProgram()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "log(a, 10)",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                10),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Log));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_Floor_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "floor(a)",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Floor));
    }


    [Fact]
    public void Compile_Ceil_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "ceil(a)",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Ceil));
    }


    [Fact]
    public void Compile_LogicalAnd_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a && b",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.And));
    }


    [Fact]
    public void Compile_LogicalOr_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a || b",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Or));
    }


    [Fact]
    public void Compile_LogicalXor_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a xor b",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Xor));
    }


    [Fact]
    public void Compile_LogicalNot_ReturnsExpectedOperator()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "!a",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Not));
    }


    [Fact]
    public void Compile_Symbols_ReturnsExpectedSymbolInstructions()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "pi + e",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Symbol,
                (double)SymbolType.Pi),
            new ExpressionInstruction(
                ExpressionKinds.Symbol,
                (double)SymbolType.Euler),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));

        AssertVariableReferences(
            expression);
    }


    [Fact]
    public void Compile_ComplexExpression_ReturnsExpectedPostfixProgram()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "sin(a) + sqrt(b * 2) ^ 2",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Sin),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Multiply),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Sqrt),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Power),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)),
            new VariableReference(
                1,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_OptimizationDisabled_PreservesUnoptimizedExpression()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a + 0",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_OptimizationEnabled_AppliesAlgebraicSimplification()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a + 0",
                optimize: true);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_OptimizationEnabled_FoldsConstantExpression()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "2 + 3 * 4",
                optimize: true);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                14));

        AssertVariableReferences(
            expression);
    }


    [Fact]
    public void Compile_OptimizationDisabled_DoesNotFoldConstantExpression()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "2 + 3 * 4",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                3),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                4),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Multiply),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));

        AssertVariableReferences(
            expression);
    }


    [Fact]
    public void Compile_NullFormula_ThrowsArgumentNullException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<ArgumentNullException>(
            () => compiler.Compile(null!));
    }


    [Fact]
    public void Compile_EmptyFormula_ThrowsArgumentException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<ArgumentException>(
            () => compiler.Compile(""));
    }


    [Fact]
    public void Compile_WhitespaceFormula_ThrowsArgumentException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<ArgumentException>(
            () => compiler.Compile("   "));
    }


    [Fact]
    public void Compile_UnknownIdentifier_ThrowsSemanticException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<SemanticException>(
            () => compiler.Compile("unknown"));
    }

    [Fact]
    public void Compile_LogarithmWithoutBase_ThrowsSemanticException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<SemanticException>(
            () => compiler.Compile("log(a)"));
    }

    // -----------------------------------------------------------------------------
    // LawsLaboratory
    // Tests / SimulationTest
    //
    // FormulaCompilerTests.cs
    //
    // Additional tests covering operator associativity, spatial reference
    // preservation, semantic validation, syntax validation, and optimization
    // safety through the public FormulaCompiler contract.
    // -----------------------------------------------------------------------------

    [Fact]
    public void Compile_Subtraction_IsLeftAssociative()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a - b - 2",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Subtract),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Subtract));
    }


    [Fact]
    public void Compile_Division_IsLeftAssociative()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a / b / temperature",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Divide),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Divide));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)),
            new VariableReference(
                1,
                new PlanePosition(0, 0)),
            new VariableReference(
                2,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_Power_IsRightAssociative()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a ^ b ^ 2",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Power),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Power));
    }


    [Fact]
    public void Compile_MultipleLogicalOperators_PreservesLogicalPrecedence()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a || b && temperature",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.And),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Or));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)),
            new VariableReference(
                1,
                new PlanePosition(0, 0)),
            new VariableReference(
                2,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_MixedRelativeReferences_PreservesParameterAndPositionAssociation()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "temperature[1,0] + pressure[-1,2] * a[0,-1]",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Multiply),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));

        AssertVariableReferences(
            expression,
            new VariableReference(
                2,
                new PlanePosition(1, 0)),
            new VariableReference(
                3,
                new PlanePosition(-1, 2)),
            new VariableReference(
                0,
                new PlanePosition(0, -1)));
    }


    [Fact]
    public void Compile_DefaultRelativePosition_IsZeroZero()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a",
                optimize: false);

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_FunctionNames_AreCaseInsensitive()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "SIN(a)",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Sin));
    }


    [Fact]
    public void Compile_ParameterNames_AreCaseSensitive()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<SemanticException>(
            () => compiler.Compile("A"));
    }


    [Fact]
    public void Compile_SymbolNames_AreCaseInsensitive()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "PI + E",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Symbol,
                (double)SymbolType.Pi),
            new ExpressionInstruction(
                ExpressionKinds.Symbol,
                (double)SymbolType.Euler),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));
    }


    [Fact]
    public void Compile_UnknownFunction_ThrowsSemanticException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<SemanticException>(
            () => compiler.Compile(
                "unknown(a)"));
    }


    [Fact]
    public void Compile_FunctionWithTooManyArguments_ThrowsSemanticException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<SemanticException>(
            () => compiler.Compile("sin(a, b)"));
    }


    [Fact]
    public void Compile_FunctionWithTooFewArguments_ThrowsSemanticException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<SemanticException>(
            () => compiler.Compile("sqrt()"));
    }


    [Fact]
    public void Compile_MalformedRelativePosition_ThrowsException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.ThrowsAny<Exception>(
            () => compiler.Compile("a[1]"));
    }


    [Fact]
    public void Compile_IncompleteBinaryExpression_ThrowsException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.ThrowsAny<Exception>(
            () => compiler.Compile("a +"));
    }


    [Fact]
    public void Compile_UnclosedParenthesis_ThrowsException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.ThrowsAny<Exception>(
            () => compiler.Compile("(a + b"));
    }


    [Fact]
    public void Compile_InvalidOperatorSequence_ThrowsException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.ThrowsAny<Exception>(
            () => compiler.Compile("a ** b"));
    }


    [Fact]
    public void Compile_OptimizationEnabled_SimplifiesMultiplicationByZero()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a * 0",
                optimize: true);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                0));

        AssertVariableReferences(
            expression);
    }


    [Fact]
    public void Compile_OptimizationEnabled_SimplifiesDivisionByOne()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a / 1",
                optimize: true);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0));

        AssertVariableReferences(
            expression,
            new VariableReference(
                0,
                new PlanePosition(0, 0)));
    }


    [Fact]
    public void Compile_OptimizationEnabled_SimplifiesExactMathematicalIdentities()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "sin(0) + cos(0) + ln(1)",
                optimize: true);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                1));

        AssertVariableReferences(
            expression);
    }


    [Fact]
    public void Compile_OptimizationEnabled_FoldsNaturalLogarithmOfEuler()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "ln(e)",
                optimize: true);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                1));

        AssertVariableReferences(
            expression);
    }


    [Fact]
    public void Compile_OptimizationEnabled_FoldsConstantPower()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "2 ^ 3",
                optimize: true);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                8));

        AssertVariableReferences(
            expression);
    }


    [Fact]
    public void Compile_OptimizationEnabled_PreservesVariableReferencesAfterSimplification()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "1 * temperature[1,-2]",
                optimize: true);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0));

        AssertVariableReferences(
            expression,
            new VariableReference(
                2,
                new PlanePosition(1, -2)));
    }


    [Fact]
    public void Compile_OptimizationEnabled_DivisionByZero_ThrowsInvalidOperationException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<InvalidOperationException>(
            () => compiler.Compile(
                "1 / 0",
                optimize: true));
    }


    [Fact]
    public void Compile_OptimizationEnabled_UndefinedZeroPowerZero_ThrowsInvalidOperationException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<InvalidOperationException>(
            () => compiler.Compile(
                "0 ^ 0",
                optimize: true));
    }


    [Fact]
    public void Compile_OptimizationEnabled_NegativeSquareRoot_ThrowsInvalidOperationException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<InvalidOperationException>(
            () => compiler.Compile(
                "sqrt(-1)",
                optimize: true));
    }


    [Fact]
    public void Compile_OptimizationDisabled_AllowsNumericallyUnsafeConstantExpressions()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "1 / 0",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Divide));

        AssertVariableReferences(
            expression);
    }

    [Fact]
    public void Compile_NegativeVariable_ReturnsSubtractionFromZero()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "-a",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Subtract));
    }

    [Fact]
    public void Compile_NestedUnaryOperators_ReturnsExpectedPostfixProgram()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "--a",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Constant,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Subtract),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Subtract));
    }

    [Fact]
    public void Compile_NestedLogicalNot_ReturnsExpectedPostfixProgram()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "!!a",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Not),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Not));
    }

    [Fact]
    public void Compile_LogicalOperatorPrecedence_ReturnsExpectedPostfixProgram()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "a || b && temperature",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                1),
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                2),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.And),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Or));
    }

    [Fact]
    public void Compile_VariableWithNegativeRelativePosition_PreservesBothCoordinates()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "temperature[-3,-4]",
                optimize: false);

        AssertVariableReferences(
            expression,
            new VariableReference(
                2,
                new PlanePosition(-3, -4)));
    }

    [Fact]
    public void Compile_FunctionNamesAreCaseInsensitive()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "SIN(a)",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Variable,
                0),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Sin));
    }

    [Fact]
    public void Compile_SymbolNamesAreCaseInsensitive()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "PI + E",
                optimize: false);

        AssertProgram(
            expression,
            new ExpressionInstruction(
                ExpressionKinds.Symbol,
                (double)SymbolType.Pi),
            new ExpressionInstruction(
                ExpressionKinds.Symbol,
                (double)SymbolType.Euler),
            new ExpressionInstruction(
                ExpressionKinds.Operator,
                (double)OperatorType.Add));
    }

    [Fact]
    public void Compile_ConstantExpression_ContainsNoVariableReferences()
    {
        FormulaCompiler compiler = CreateCompiler();

        CompiledExpression expression =
            compiler.Compile(
                "2 + 3",
                optimize: false);

        AssertVariableReferences(expression);
    }

    [Fact]
    public void Compile_MissingOperatorOperand_ThrowsInvalidOperationException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<InvalidOperationException>(
            () => compiler.Compile(
                "a +"));
    }

    [Fact]
    public void Compile_UnclosedRelativePosition_ThrowsInvalidOperationException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<InvalidOperationException>(
            () => compiler.Compile(
                "a[1,2"));
    }

    [Fact]
    public void Compile_MissingFunctionArgument_ThrowsInvalidOperationException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<InvalidOperationException>(
            () => compiler.Compile(
                "sin(,)"));
    }

    [Fact]
    public void Compile_UnclosedFunctionCall_ThrowsInvalidOperationException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<InvalidOperationException>(
            () => compiler.Compile(
                "sin(a"));
    }

    [Fact]
    public void Compile_UnclosedParenthesis_ThrowsInvalidOperationException()
    {
        FormulaCompiler compiler = CreateCompiler();

        Assert.Throws<InvalidOperationException>(
            () => compiler.Compile(
                "(a + b"));
    }
}