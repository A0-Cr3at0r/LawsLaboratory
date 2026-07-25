namespace LawsLaboratory.Core.SpatialModel;

using LawsLaboratory.Core.Value;

internal sealed class Parameter
{
    private IValue _value;

    public int Id { get; }

    public IValue Value => _value;

    public Parameter(int id)
    {
        Id = id;
        _value = Dead.Instance;
    }

    public void Set(double value)
    {
        _value = _value.Set(value);
    }

    public void Set(IValue value)
    {
        _value = value;
    }

    public void Kill()
    {
        _value = Dead.Instance;
    }
}