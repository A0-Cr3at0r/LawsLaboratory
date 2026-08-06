namespace LawsLaboratory.Application.Execution.ExecutionResultStage;

public sealed record ResultPacket<T> : IResult
{
    public int CellId { get; private set; }

    public T Result { get; private set; }

    public ResultPacket()
    {
        Clear();
    }

    public void Set(
        int cellId,
        T result)
    {
        CellId = cellId;
        Result = result;
    }

    public void Clear() {
        CellId = -1;
        Result = default!;
    }

}