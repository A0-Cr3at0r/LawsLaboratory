using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Core.Simulation.SpatialManagement.Execution;

public sealed class SpatialPacket
{
    public int CellId { get; private set; }

    public ushort ParamId { get; private set; }

    public IValue[] Values { get; }


    public SpatialPacket(int valueCount)
    {
        Values = new IValue[valueCount];

        Clear();
    }


    public void Set(
        int cellId,
        ushort paramId)
    {
        CellId = cellId;
        ParamId = paramId;
    }


    public void Clear()
    {
        CellId = -1;
        ParamId = 0;

        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] = Dead.Instance;
        }
    }


    public void Write(
        int index,
        IValue value)
    {
        Values[index] = value;
    }
}