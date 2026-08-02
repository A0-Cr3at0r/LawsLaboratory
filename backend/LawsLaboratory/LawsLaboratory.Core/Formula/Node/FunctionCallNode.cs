namespace LawsLaboratory.Core.Formula.Node;

public sealed class FunctionCallNode : ExpressionNode
{
    public string Name { get; }

    public IReadOnlyList<ExpressionNode> Arguments { get; }


    public FunctionCallNode(
        string name,
        IEnumerable<ExpressionNode> arguments)
    {
        Name = name;
        Arguments = arguments.ToList();
    }
}