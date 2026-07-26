using LawsLaboratory.Core.SpatialModel;

namespace LawsLaboratory.Core.Formula.Node;

public sealed class VariableNode : ExpressionNode
{
    public int ParameterId { get; }

    public Position RelativePosition { get; }


    public VariableNode( int parameterId, Position relativePosition)
    {
        ParameterId = parameterId;
        RelativePosition = relativePosition;
    }
}
