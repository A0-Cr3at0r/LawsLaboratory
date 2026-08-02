using LawsLaboratory.Core.SpatialModel.Position;

namespace LawsLaboratory.Core.Formula.Node;

public sealed class IdentifierNode : ExpressionNode
{
    public string Name { get; }

    public PlanePosition RelativePosition { get; }


    public IdentifierNode(
        string name,
        PlanePosition relativePosition)
    {
        Name = name;
        RelativePosition = relativePosition;
    }
}