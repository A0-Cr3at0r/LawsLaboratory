namespace LawsLaboratory.Application.Execution.ExecutionStage.Traversal;
internal interface ITraversalStrategy<T>
{
    IEnumerable<int> Traverse(T gridContext);
}