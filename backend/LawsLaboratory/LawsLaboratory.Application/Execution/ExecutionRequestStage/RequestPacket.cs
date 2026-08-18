namespace LawsLaboratory.Application.Execution.ExecutionRequestStage;

internal sealed class RequestPacket
{
    public int CellId { get; internal set; }

    public double?[] Values { get; }


    public RequestPacket(int MaxValueCount)
    {
        Values = new double?[MaxValueCount];

        CellId = -1;
    }

    public void Clear()
    {
        for (int i = 0; i < Values.Length; i++) { 
            Values[i] = null;
        }

        CellId = -1;
    }

    public void Write(
    int index,
    double value)
    {
        Values[index] = value;
    }

}