using LawsLaboratory.Core.SpatialModel;
using LawsLaboratory.Core.SpatialModel.Position;

namespace LawsLaboratory.Core.Formula.Node;

public sealed class VariableNode : ExpressionNode
{
    public int ParameterId { get; }

    public PlanePosition RelativePosition { get; }


    public VariableNode( int parameterId, PlanePosition relativePosition)
    {
        ParameterId = parameterId;
        RelativePosition = relativePosition;
    }
}
