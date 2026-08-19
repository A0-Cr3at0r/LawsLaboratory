// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Node
//
// FunctionCallNode.cs
//
// Represents a function call within a formula expression.
//
// The function name and its argument expressions are preserved as part of the
// expression tree and can be resolved or compiled at a later stage.
// -----------------------------------------------------------------------------

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