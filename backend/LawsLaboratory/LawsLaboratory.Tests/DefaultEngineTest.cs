using LawsLaboratory.Application.DefaultEngine;
using LawsLaboratory.Application.Execution.EngineGateway.Entry;
using LawsLaboratory.Core.Formula;

namespace LawsLaboratory.Tests.DefaultEngineTest;

public class DefaultEngineTest
{
    [Fact]
    public void Constant_ReturnsValue()
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(42)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Variable_ReturnsValueAtIndex()
    {
        var expression = new List<ExpressionEntry>
        {
            Variable(1)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([10, 42, 100]);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Pi_ReturnsPi()
    {
        var expression = new List<ExpressionEntry>
        {
            Symbol(SymbolType.Pi)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(Math.PI, result);
    }

    [Fact]
    public void Euler_ReturnsEuler()
    {
        var expression = new List<ExpressionEntry>
        {
            Symbol(SymbolType.Euler)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(Math.E, result);
    }

    [Theory]
    [InlineData(10, 5, 15)]
    [InlineData(-2, -5, -7)]
    [InlineData(-10, 5, -5)]
    public void Add_ReturnsSum(
        double left,
        double right,
        double expected)
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(left),
            Constant(right),
            Operator(OperatorType.Add)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(10, 5, 5)]
    [InlineData(5, 10, -5)]
    [InlineData(-10, 5, -15)]
    public void Subtract_ReturnsDifference(
        double left,
        double right,
        double expected)
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(left),
            Constant(right),
            Operator(OperatorType.Subtract)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(10, 5, 50)]
    [InlineData(-10, 5, -50)]
    public void Multiply_ReturnsProduct(
        double left,
        double right,
        double expected)
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(left),
            Constant(right),
            Operator(OperatorType.Multiply)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(10, 5, 2)]
    [InlineData(9, 2, 4.5)]
    public void Divide_ReturnsQuotient(
        double left,
        double right,
        double expected)
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(left),
            Constant(right),
            Operator(OperatorType.Divide)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(2, 3, 8)]
    [InlineData(10, 2, 100)]
    [InlineData(5, 0, 1)]
    public void Power_ReturnsPower(
        double left,
        double right,
        double expected)
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(left),
            Constant(right),
            Operator(OperatorType.Power)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Log_UsesLeftAsBaseAndRightAsValue()
    {
        // log_2(8) = 3
        var expression = new List<ExpressionEntry>
        {
            Constant(2),
            Constant(8),
            Operator(OperatorType.Log)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(3, result, precision: 12);
    }

    [Fact]
    public void Ln_ReturnsNaturalLogarithm()
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(Math.E),
            Operator(OperatorType.Ln)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(1, result, precision: 12);
    }

    [Theory]
    [InlineData(9, 3)]
    [InlineData(16, 4)]
    [InlineData(2.25, 1.5)]
    public void Sqrt_ReturnsSquareRoot(
        double value,
        double expected)
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(value),
            Operator(OperatorType.Sqrt)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(expected, result, precision: 12);
    }

    [Fact]
    public void Sin_ReturnsSine()
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(Math.PI / 2),
            Operator(OperatorType.Sin)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(1, result, precision: 12);
    }

    [Fact]
    public void Cos_ReturnsCosine()
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(Math.PI),
            Operator(OperatorType.Cos)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(-1, result, precision: 12);
    }

    [Theory]
    [InlineData(3.7, 3)]
    [InlineData(-3.7, -4)]
    public void Floor_ReturnsFlooredValue(
        double value,
        double expected)
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(value),
            Operator(OperatorType.Floor)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(3.2, 4)]
    [InlineData(-3.2, -3)]
    public void Ceil_ReturnsCeilingValue(
        double value,
        double expected)
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(value),
            Operator(OperatorType.Ceil)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ComplexPostfixExpression_EvaluatesCorrectly()
    {
        // (p0 + 2) * p1
        //
        // Postfix:
        // p0 2 + p1 *

        var expression = new List<ExpressionEntry>
        {
            Variable(0),
            Constant(2),
            Operator(OperatorType.Add),
            Variable(1),
            Operator(OperatorType.Multiply)
        };

        var engine = new DefaultEngine(expression);

        Assert.Equal(20, engine.Evaluate([3, 4]));
        Assert.Equal(50, engine.Evaluate([8, 5]));
        Assert.Equal(-10, engine.Evaluate([-7, 2]));
    }

    [Fact]
    public void ComplexExpression_WithSymbols_EvaluatesCorrectly()
    {
        // 2 * pi + e
        //
        // Postfix:
        // 2 pi * e +

        var expression = new List<ExpressionEntry>
        {
            Constant(2),
            Symbol(SymbolType.Pi),
            Operator(OperatorType.Multiply),
            Symbol(SymbolType.Euler),
            Operator(OperatorType.Add)
        };

        var engine = new DefaultEngine(expression);

        var result = engine.Evaluate([]);

        Assert.Equal(
            2 * Math.PI + Math.E,
            result,
            precision: 12);
    }

    [Fact]
    public void SameEngine_CanEvaluateMultipleTimes()
    {
        // p0 * 2

        var expression = new List<ExpressionEntry>
        {
            Variable(0),
            Constant(2),
            Operator(OperatorType.Multiply)
        };

        var engine = new DefaultEngine(expression);

        Assert.Equal(4, engine.Evaluate([2]));
        Assert.Equal(20, engine.Evaluate([10]));
        Assert.Equal(-6, engine.Evaluate([-3]));
        Assert.Equal(0, engine.Evaluate([0]));
    }

    [Fact]
    public void BitwiseAnd_ReturnsExpectedValue()
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(6),
            Constant(3),
            Operator(OperatorType.And)
        };

        var engine = new DefaultEngine(expression);

        // 110 & 011 = 010
        Assert.Equal(2, engine.Evaluate([]));
    }

    [Fact]
    public void BitwiseOr_ReturnsExpectedValue()
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(6),
            Constant(3),
            Operator(OperatorType.Or)
        };

        var engine = new DefaultEngine(expression);

        // 110 | 011 = 111
        Assert.Equal(7, engine.Evaluate([]));
    }

    [Fact]
    public void BitwiseXor_ReturnsExpectedValue()
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(6),
            Constant(3),
            Operator(OperatorType.Xor)
        };

        var engine = new DefaultEngine(expression);

        // 110 ^ 011 = 101
        Assert.Equal(5, engine.Evaluate([]));
    }

    [Fact]
    public void BitwiseNot_ReturnsExpectedValue()
    {
        var expression = new List<ExpressionEntry>
        {
            Constant(5),
            Operator(OperatorType.Not)
        };

        var engine = new DefaultEngine(expression);

        Assert.Equal(~5L, (long)engine.Evaluate([]));
    }

    private static ExpressionEntry Constant(double value)
    {
        return new ExpressionEntry(
            ExpressionKinds.Constant,
            value);
    }

    private static ExpressionEntry Variable(int index)
    {
        return new ExpressionEntry(
            ExpressionKinds.Variable,
            index);
    }

    private static ExpressionEntry Symbol(SymbolType symbol)
    {
        return new ExpressionEntry(
            ExpressionKinds.Symbol,
            (double)symbol);
    }

    private static ExpressionEntry Operator(OperatorType operation)
    {
        return new ExpressionEntry(
            ExpressionKinds.Operator,
            (double)operation);
    }
}