using LawsLaboratory.Core.Formula;
using LawsLaboratory.Core.Formula.Node;

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