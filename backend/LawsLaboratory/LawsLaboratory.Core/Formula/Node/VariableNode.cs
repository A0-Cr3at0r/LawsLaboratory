using LawsLaboratory.Core.SpatialModel;

namespace LawsLaboratory.Core.Formula.Node;

public sealed class VariableNode : ExpressionNode
{
    public int ParameterId { get; }

    public RelativePosition Position { get; }


    public VariableNode( int parameterId, RelativePosition position)
    {
        ParameterId = parameterId;
        Position = position;
    }
}
