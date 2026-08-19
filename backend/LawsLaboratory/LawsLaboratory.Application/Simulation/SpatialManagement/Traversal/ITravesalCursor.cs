// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / SpatialManagement / Traversal
//
// ITraversalCursor.cs
//
// Represents the mutable state of an active traversal.
//
// The cursor exposes the current position, the number of positions to
// traverse, and operations for advancing or resetting the traversal.
//
// TraversalCount may be adjusted by the caller when only a portion of the
// available domain must be processed.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;

internal interface ITraversalCursor
{
    int Current { get; }

    int TraversalCount { get; set; }

    bool TryAdvance();

    void Reset();
}