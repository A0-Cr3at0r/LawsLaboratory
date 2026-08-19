// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Node
//
// OperatorNode.cs
//
// Represents an operator expression and its operand expressions.
//
// The number of children depends on the operator's arity. OperatorNode itself
// stores the expression structure; validation of operator arity belongs to
// the formula parsing or compilation stage.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Formula;
namespace LawsLaboratory.Core.Formula.Node;

public sealed class OperatorNode : ExpressionNode
{
    public OperatorType Operator { get; }

    public IReadOnlyList<ExpressionNode> Children { get; }

    public OperatorNode(
        OperatorType op,
        IEnumerable<ExpressionNode> children)
    {
        Operator = op;
        Children = children.ToList();
    }
}