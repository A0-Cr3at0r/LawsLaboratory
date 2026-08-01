namespace LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;

internal interface ITraversalCursor
{
    int Current { get; }

    bool TryAdvance();

    void Reset();
}