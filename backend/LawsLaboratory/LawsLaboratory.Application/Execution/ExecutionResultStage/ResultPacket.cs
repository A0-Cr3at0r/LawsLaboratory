namespace LawsLaboratory.Application.Execution.ExecutionResultStage;

internal sealed class ResultPacket<T>
{
    public int CellId { get; private set; }

    public ushort ParameterId { get; private set; }

    public T Result { get; private set; }

    public ResultPacket()
    {
        CellId = -1;
        ParameterId = 0;
        Result = default!;
    }

    public void Set(
        int cellId,
        ushort parameterId,
        T result)
    {
        CellId = cellId;
        ParameterId = parameterId;
        Result = result;
    }
}