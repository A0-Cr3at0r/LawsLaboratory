using LawsLaboratory.Core.Formula;
using LawsLaboratory.Core.Formula.Program;

namespace LawsLaboratory.Application.Engine;

using Program = List<ExpressionInstruction>;


internal sealed class DefaultEngine
{
    private readonly Program _expression;

    private readonly Stack<double> _stack = new();
    public DefaultEngine(Program  expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        _expression = expression;
    }

    public double Evaluate(double?[] values)
    {
        ArgumentNullException.ThrowIfNull(values);

        _stack.Clear();

        foreach (var entry in _expression)
        {
            switch (entry.Kind)
            {
                case ExpressionKinds.Constant:
                case ExpressionKinds.Symbol:
                case ExpressionKinds.Variable:
                    _stack.Push(
                        ResolveValue(entry, values));
                    break;

                case ExpressionKinds.Operator:
                    ExecuteOperator(
                        (OperatorType)(byte)entry.Value,
                        _stack);
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Unknown expression kind: {entry.Kind}");
            }
        }

        if (_stack.Count != 1)
        {
            throw new InvalidOperationException(
                $"Invalid postfix expression. " +
                $"Expected one result, but stack contains {_stack.Count} values.");
        }

        return _stack.Pop();
    }

    private static double ResolveValue(
    ExpressionInstruction entry,
    double?[] values)
    {
        switch (entry.Kind)
        {
            case ExpressionKinds.Constant:
                return entry.Value;

            case ExpressionKinds.Variable:
                {
                    var index = GetVariableIndex(entry);

                    if ((uint)index >= (uint)values.Length)
                    {
                        throw new IndexOutOfRangeException(
                            $"Variable index {index} is outside the supplied values.");
                    }

                    double? value = values[index];

                    if (!value.HasValue)
                    {
                        throw new InvalidOperationException(
                            $"Variable index {index} has no value.");
                    }

                    return value.Value;
                }

            case ExpressionKinds.Symbol:
                return ResolveSymbol(entry);

            default:
                throw new InvalidOperationException(
                    $"Expression entry of kind {entry.Kind} is not a value.");
        }
    }

    private static int GetVariableIndex(ExpressionInstruction entry)
    {
        return checked((int)entry.Value);
    }

    private static double ResolveSymbol(ExpressionInstruction entry)
    {
        var symbol = (SymbolType)(byte)entry.Value;

        return symbol switch
        {
            SymbolType.Pi => Math.PI,
            SymbolType.Euler => Math.E,

            _ => throw new InvalidOperationException(
                $"Unknown symbol: {symbol}")
        };
    }

    private static void ExecuteOperator(
        OperatorType operation,
        Stack<double> stack)
    {
        switch (operation)
        {
            case OperatorType.Add:
                ExecuteBinary(stack, static (left, right) => left + right);
                break;

            case OperatorType.Subtract:
                ExecuteBinary(stack, static (left, right) => left - right);
                break;

            case OperatorType.Multiply:
                ExecuteBinary(stack, static (left, right) => left * right);
                break;

            case OperatorType.Divide:
                ExecuteBinary(stack, static (left, right) => left / right);
                break;

            case OperatorType.Power:
                ExecuteBinary(stack, Math.Pow);
                break;

            case OperatorType.Log:
                ExecuteBinary(stack, (baseValue, value) => Math.Log(value, baseValue));
                break;

            case OperatorType.Ln:
                ExecuteUnary(stack, Math.Log);
                break;

            case OperatorType.Sqrt:
                ExecuteUnary(stack, Math.Sqrt);
                break;

            case OperatorType.Sin:
                ExecuteUnary(stack, Math.Sin);
                break;

            case OperatorType.Cos:
                ExecuteUnary(stack, Math.Cos);
                break;

            case OperatorType.Floor:
                ExecuteUnary(stack, Math.Floor);
                break;

            case OperatorType.Ceil:
                ExecuteUnary(stack, Math.Ceiling);
                break;

            case OperatorType.Not:
                ExecuteUnary(stack, BitwiseNot);
                break;

            case OperatorType.And:
                ExecuteBinary(stack, BitwiseAnd);
                break;

            case OperatorType.Or:
                ExecuteBinary(stack, BitwiseOr);
                break;

            case OperatorType.Xor:
                ExecuteBinary(stack, BitwiseXor);
                break;

            default:
                throw new InvalidOperationException(
                    $"Unknown operator: {operation}");
        }
    }

    private static void ExecuteUnary(
        Stack<double> stack,
        Func<double, double> operation)
    {
        EnsureStackSize(stack, 1);

        var value = stack.Pop();
        stack.Push(operation(value));
    }

    private static void ExecuteBinary(
        Stack<double> stack,
        Func<double, double, double> operation)
    {
        EnsureStackSize(stack, 2);

        var right = stack.Pop();
        var left = stack.Pop();

            stack.Push(operation(left, right));
    }

    private static void EnsureStackSize(
        Stack<double> stack,
        int required)
    {
        if (stack.Count < required)
        {
            throw new InvalidOperationException(
                $"Invalid postfix expression. " +
                $"Operator requires {required} operand(s), " +
                $"but only {stack.Count} available.");
        }
    }

    private static double BitwiseAnd(double left, double right)
    {
        return (long)left & (long)right;
    }

    private static double BitwiseOr(double left, double right)
    {
        return (long)left | (long)right;
    }

    private static double BitwiseXor(double left, double right)
    {
        return (long)left ^ (long)right;
    }

    private static double BitwiseNot(double value)
    {
        return ~(long)value;
    }
}