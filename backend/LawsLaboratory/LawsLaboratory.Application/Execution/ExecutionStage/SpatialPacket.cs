using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Application.Execution.ExecutionStage;

public sealed class SpatialPacket
{
    public int CellId { get; private set; }

    public ushort ParamId { get; private set; }

    public IValue[] Values { get; }


    public SpatialPacket(int valueCount)
    {
        Values = new IValue[valueCount];

        CellId = -1;
        ParamId = 0;

        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] = Dead.Instance;
        }
    }


    public void Set(
        int cellId,
        ushort paramId)
    {
        CellId = cellId;
        ParamId = paramId;
    }


    public void Write(
    int index,
    double value)
    {
        Values[index] =
            Values[index].Set(value);
    }

    public void WriteDead(int index)
    {
        Values[index] = Dead.Instance;
    }
}