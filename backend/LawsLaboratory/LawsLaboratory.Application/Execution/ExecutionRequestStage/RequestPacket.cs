using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Application.Execution.ExecutionRequestStage;

public sealed class RequestPacket
{
    public int CellId { get; private set; }

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
    }

    public void Write(
    int index,
    double value)
    {
        Values[index] = value;
    }

}