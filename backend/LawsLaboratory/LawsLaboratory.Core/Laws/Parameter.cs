namespace LawsLaboratory.Core.Laws;

using LawsLaboratory.Core.Value;

public sealed class Parameter
{
    private readonly Func<double, IValue> _factory;

    public ushort Id { get; }

    public string Name { get; }


    public Parameter(
        ushort id,
        string name,
        Func<double, IValue> factory)
    {
        Id = id;
        Name = name;
        _factory = factory;
    }


    public IValue CreateValue(double value)
    {
        return _factory(value);
    }
}