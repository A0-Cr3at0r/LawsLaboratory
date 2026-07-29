namespace LawsLaboratory.Core.Simulation.SpatialManagement.Traversal;
internal interface ITraversalStrategy<T>
{
    IEnumerable<int> Traverse(T gridContext);
}