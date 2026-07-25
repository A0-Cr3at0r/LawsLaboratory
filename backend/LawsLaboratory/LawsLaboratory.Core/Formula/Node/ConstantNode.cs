namespace LawsLaboratory.Core.Formula.Node;

public sealed class ConstantNode : ExpressionNode
{
    public double Value { get; }

    public ConstantNode(double value)
    {
        Value = value;
    }
}
