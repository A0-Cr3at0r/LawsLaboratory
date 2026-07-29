namespace LawsLaboratory.Application.ExecutionStage.Traversal;
internal interface ITraversalStrategy<T>
{
    IEnumerable<int> Traverse(T gridContext);
}