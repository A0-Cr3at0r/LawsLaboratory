// ----------------------------------------------------------------------------- 
// LawsLaboratory 
// Application / FormulaCompiler / Optimization
// 
// FormulaOptimizer.cs
// 
// Applies optional mathematical transformations to the scientific 
// expression tree in order to reduce the cost of later evaluation.
// 
// Optimization is intended primarily for the internal Laws Laboratory
// calculation engine and is not required for compilation. 
//
// The optimizer targets the real-valued formula model of the current
// version and rejects constant expressions that produce invalid real 
// values such as NaN or Infinity. 
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Formula;
using LawsLaboratory.Core.Formula.Node;

namespace LawsLaboratory.Application.FormulaCompiler.Optimization;

public sealed class FormulaOptimizer
{
    private const int MaxOptimizationPasses = 32;

    private static readonly ConstantNode ZeroNode = new(0);
    private static readonly ConstantNode OneNode = new(1);
    private static readonly ConstantNode MinusOneNode = new(-1);


    public ExpressionNode Optimize(ExpressionNode expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        ExpressionNode current = expression;

        for (int pass = 0; pass < MaxOptimizationPasses; pass++)
        {
            bool changed = false;

            current = OptimizeNode(current, ref changed);

            if (!changed)
                return current;
        }

        throw new InvalidOperationException(
            $"Formula optimization did not stabilize after {MaxOptimizationPasses} passes.");
    }


    private ExpressionNode OptimizeNode(
        ExpressionNode node,
        ref bool changed)
    {

        if (node is not OperatorNode operatorNode)
            return node;

        ExpressionNode[] optimizedChildren =
            new ExpressionNode[operatorNode.Children.Count];

        for (int i = 0; i < optimizedChildren.Length; i++)
        {
            optimizedChildren[i] =
                OptimizeNode(operatorNode.Children[i], ref changed);
        }


        ExpressionNode optimized =
            new OperatorNode(
                operatorNode.Operator,
                optimizedChildren);


        optimized = TryConstantFold(
            optimized,
            ref changed);

        optimized = TrySimplifyExactIdentity(
            optimized,
            ref changed);

        optimized = TrySimplifyAlgebraic(
            optimized,
            ref changed);


        return optimized;
    }


    private static bool IsConstant(
        ExpressionNode node)
    {
        return node is ConstantNode;
    }


    private static bool IsConstantZero(
        ExpressionNode node)
    {
        return node is ConstantNode constant &&
               constant.Value == 0;
    }


    private static bool IsConstantOne(
        ExpressionNode node)
    {
        return node is ConstantNode constant &&
               constant.Value == 1;
    }


    private static double GetConstantValue(
        ExpressionNode node)
    {
        return ((ConstantNode)node).Value;
    }


    private static bool IsSymbol(
        ExpressionNode node,
        SymbolType symbol)
    {
        return node is SymbolNode symbolNode &&
               symbolNode.Symbol == symbol;
    }

    private static ConstantNode Zero()
    {
        return ZeroNode;
    }


    private static ConstantNode One()
    {
        return OneNode;
    }


    private static ConstantNode MinusOne()
    {
        return MinusOneNode;
    }

    private ExpressionNode TryConstantFold(
       ExpressionNode node,
       ref bool changed)
    {
        if (node is not OperatorNode operatorNode)
            return node;

        if (!operatorNode.Children.All(IsConstant))
            return node;


        if (!TryEvaluateConstantOperator(
                operatorNode,
                out double value))
        {
            return node;
        }


        changed = true;

        return new ConstantNode(value);
    }


    private bool TryEvaluateConstantOperator(
        OperatorNode node,
        out double result)
    {

        switch (node.Operator)
        {
            case OperatorType.Add:
                {
                    result =
                        GetConstant(node, 0) +
                        GetConstant(node, 1);

                    break;
                }

            case OperatorType.Subtract:
                {
                    result =
                        GetConstant(node, 0) -
                        GetConstant(node, 1);

                    break;
                }

            case OperatorType.Multiply:
                {
                    result =
                        GetConstant(node, 0) *
                        GetConstant(node, 1);

                    break;
                }

            case OperatorType.Divide:
                {
                    double denominator =
                        GetConstant(node, 1);


                    if (denominator == 0)
                    {
                        throw new InvalidOperationException(
                            "Division by zero in constant expression.");
                    }

                    result =
                        GetConstant(node, 0) /
                        denominator;

                    break;
                }

            case OperatorType.Power:
                {
                    double basis =
                        GetConstant(node, 0);

                    double exponent =
                        GetConstant(node, 1);

                    if (basis == 0 && exponent == 0)
                    {
                        throw new InvalidOperationException(
                            "0^0 is undefined.");
                    }

                    result =
                        Math.Pow(
                            basis,
                            exponent);

                    break;
                }

            case OperatorType.Sqrt:
                {
                    double value =
                        GetConstant(node, 0);

                    if (value < 0)
                    {
                        throw new InvalidOperationException(
                            "Square root of negative constant.");
                    }

                    result =
                        Math.Sqrt(value);

                    break;
                }

            case OperatorType.Sin:
                {
                    result =
                        Math.Sin(
                            GetConstant(node, 0));

                    break;
                }

            case OperatorType.Cos:
                {
                    result =
                        Math.Cos(
                            GetConstant(node, 0));

                    break;
                }

            case OperatorType.Ln:
                {
                    result =
                        Math.Log(
                            GetConstant(node, 0));

                    break;
                }

            case OperatorType.Log:
                {
    
                    double value =
                        GetConstant(node, 0);

                    double basis =
                        GetConstant(node, 1);


                    result =
                        Math.Log(
                            value,
                            basis);

                    break;
                }

            case OperatorType.Floor:
                {
                    result =
                        Math.Floor(
                            GetConstant(node, 0));

                    break;
                }

            case OperatorType.Ceil:
                {
                    result =
                        Math.Ceiling(
                            GetConstant(node, 0));

                    break;
                }

            case OperatorType.And:
            case OperatorType.Or:
            case OperatorType.Xor:
            case OperatorType.Not:

                result = default;
                return false;

            default:

                throw new NotSupportedException(
                    $"Operator '{node.Operator}' is not supported.");
        }


        if (double.IsNaN(result) ||
            double.IsInfinity(result))
        {
            throw new InvalidOperationException(
                $"Constant expression produced invalid value: {result}.");
        }

        return true;
    }


    private static double GetConstant(
        OperatorNode node,
        int index)
    {
        return GetConstantValue(
            node.Children[index]);
    }

    private ExpressionNode TrySimplifyExactIdentity(
    ExpressionNode node,
    ref bool changed)
    {
        if (node is not OperatorNode operatorNode)
            return node;


        switch (operatorNode.Operator)
        {
            case OperatorType.Sin:
                {
                    ExpressionNode argument = operatorNode.Children[0];

                    if (IsConstantZero(argument) ||
                        IsSymbol(argument, SymbolType.Pi))
                    {
                        changed = true;
                        return Zero();
                    }

                    break;
                }


            case OperatorType.Cos:
                {
                    ExpressionNode argument = operatorNode.Children[0];

                    if (IsConstantZero(argument))
                    {
                        changed = true;
                        return One();
                    }

                    if (IsSymbol(argument, SymbolType.Pi))
                    {
                        changed = true;
                        return MinusOne();
                    }

                    break;
                }

            case OperatorType.Ln:
                {
                    ExpressionNode argument = operatorNode.Children[0];

                    if (IsConstantOne(argument))
                    {
                        changed = true;
                        return Zero();
                    }

                    if(IsSymbol(argument, SymbolType.Euler))
                    {
                        changed = true;
                        return One();
                    }

                    break;
                }
        }


        return node;
    }

    private ExpressionNode TrySimplifyAlgebraic(
    ExpressionNode node,
    ref bool changed)
    {
        if (node is not OperatorNode operatorNode)
            return node;


        if (operatorNode.Children.Count != 2)
            return node;

        ExpressionNode left =
            operatorNode.Children[0];

        ExpressionNode right =
            operatorNode.Children[1];

        switch (operatorNode.Operator)
        {
            case OperatorType.Add:
                {
                    if (IsConstantZero(left))
                    {
                        changed = true;
                        return right;
                    }

                    if (IsConstantZero(right))
                    {
                        changed = true;
                        return left;
                    }

                    break;
                }

            case OperatorType.Subtract:
                {
                    if (IsConstantZero(right))
                    {
                        changed = true;
                        return left;
                    }

                    break;
                }

            case OperatorType.Multiply:
                {
                    if (IsConstantZero(left) ||
                        IsConstantZero(right))
                    {
                        changed = true;
                        return Zero();
                    }

                    if (IsConstantOne(left))
                    {
                        changed = true;
                        return right;
                    }

                    if (IsConstantOne(right))
                    {
                        changed = true;
                        return left;
                    }

                    break;
                }


            case OperatorType.Divide:
                {
                    if (IsConstantOne(right))
                    {
                        changed = true;
                        return left;
                    }

                    break;
                }

            case OperatorType.Power:
                {
      
                    if (IsConstantOne(right))
                    {
                        changed = true;
                        return left;
                    }

                    if (IsConstantZero(right))
                    {
                        if (IsConstant(left) &&
                            !IsConstantZero(left))
                        {
                            changed = true;
                            return One();
                        }

                        if (IsSymbol(left, SymbolType.Pi) || IsSymbol(left, SymbolType.Euler))
                        {
                            changed = true;
                            return One();
                        }
                    }

                    break;
                }
        }

        return node;
    }
}