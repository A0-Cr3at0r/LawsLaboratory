// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Node
//
// ConstantNode.cs
//
// Represents a numeric constant in a formula expression.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.Formula.Node;

public sealed class ConstantNode : ExpressionNode
{
    public double Value { get; }

    public ConstantNode(double value)
    {
        Value = value;
    }
}
