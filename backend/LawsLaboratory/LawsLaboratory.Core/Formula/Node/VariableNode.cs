// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Node
//
// VariableNode.cs
//
// Represents a resolved reference to a simulation parameter within a formula.
//
// Unlike IdentifierNode, VariableNode identifies the parameter directly using
// its ParameterId.
//
// RelativePosition describes the position of the referenced parameter relative
// to the cell for which the expression is being evaluated.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.SpatialModel.Position;

namespace LawsLaboratory.Core.Formula.Node;

public sealed class VariableNode : ExpressionNode
{
    public ushort ParameterId { get; }

    public PlanePosition RelativePosition { get; }


    public VariableNode(ushort parameterId, PlanePosition relativePosition)
    {
        ParameterId = parameterId;
        RelativePosition = relativePosition;
    }
}
