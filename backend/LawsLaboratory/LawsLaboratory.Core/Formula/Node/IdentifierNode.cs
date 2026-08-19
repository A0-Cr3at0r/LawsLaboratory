// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Node
//
// IdentifierNode.cs
//
// Represents a named variable or identifier as expressed in the formula
// language.
//
// The identifier name is preserved at this stage so that it can later be
// resolved to the corresponding simulation parameter.
// -----------------------------------------------------------------------------

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