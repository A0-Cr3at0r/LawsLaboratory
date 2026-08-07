namespace LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;

internal interface ITraversalCursor
{
    int Current { get; }

    int TraversalCount { get; set; }

    bool TryAdvance();

    void Reset();
}