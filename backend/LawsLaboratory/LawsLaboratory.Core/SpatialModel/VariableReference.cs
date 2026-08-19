
// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / SpatialModel / Position
//
// VariableReference.cs
//
// Describes a reference to a parameter at a position relative to a reference
// cell.
//
// The relative position is interpreted by the component resolving the
// reference.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.SpatialModel.Position;

public readonly record struct VariableReference(
    ushort ParameterId,
    PlanePosition RelativePosition
    );
