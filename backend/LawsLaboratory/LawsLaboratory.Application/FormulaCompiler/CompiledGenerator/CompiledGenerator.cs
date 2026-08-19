// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / FormulaCompiler / CompiledGenerator
//
// CompiledGenerator.cs
//
// Converts the scientific expression tree into the linear postfix
// representation stored by CompiledExpression.
//
// The generator performs no semantic analysis or optimization.
// Each expression node is translated into its corresponding
// ExpressionElement, with operator children emitted before the operator.
// -----------------------------------------------------------------------------


using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.Formula.Node;

namespace LawsLaboratory.Application.FormulaCompiler.COmpiledGenerator;


public sealed class CompiledGenerator
{
    /// <summary>
    /// Generates a compiled expression from the specified scientific AST.
    /// </summary>
    /// <param name="expression">Root of the scientific expression tree.</param>
    /// <returns>A new compiled expression in Reverse Polish Notation.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="expression"/> is null.
    /// </exception>
    public CompiledExpression Generate(ExpressionNode expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        List<ExpressionElement> elements = [];

        Compile(expression, elements);

        return new CompiledExpression(elements);
    }

    private void Compile(
        ExpressionNode node,
        List<ExpressionElement> elements)
    {
        switch (node)
        {
            case ConstantNode constant:
                elements.Add(
                    new ConstantElement(constant.Value));
                return;

            case VariableNode variable:
                elements.Add(
                    new VariableElement(
                        variable.ParameterId,
                        variable.RelativePosition));
                return;

            case SymbolNode symbol:
                elements.Add(
                    new SymbolElement(symbol.Symbol));
                return;

            case OperatorNode op:
                foreach (ExpressionNode child in op.Children)
                {
                    Compile(child, elements);
                }

                elements.Add(
                    new OperatorElement(op.Operator));
                return;

            default:
                throw new NotSupportedException(
                    $"Expression node type '{node.GetType().Name}' is not supported by the compiled generator.");
        }
    }
}