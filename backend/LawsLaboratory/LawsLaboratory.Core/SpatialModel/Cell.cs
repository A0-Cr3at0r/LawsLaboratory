namespace LawsLaboratory.Core.SpatialModel;

using LawsLaboratory.Core.Value;

internal sealed class Cell
{
    public int Id { get; }

    private readonly IValue[] _values;


    public Cell(int id, int parameterCount)
    {
        Id = id;

        _values = new IValue[parameterCount];

        for (int i = 0; i < parameterCount; i++)
        {
            _values[i] = Dead.Instance;
        }
    }


    public IValue GetValue(int parameterId)
    {
        return _values[parameterId];
    }


    internal void SetValue(int parameterId, IValue value)
    {
        _values[parameterId] = value;
    }


    internal void KillParameter(int parameterId)
    {
        _values[parameterId] = Dead.Instance;
    }
}