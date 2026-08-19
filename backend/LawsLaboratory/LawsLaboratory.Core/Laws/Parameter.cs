// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Laws
//
// Law.cs
//
// Represents the complete set of rules governing a simulation parameter.
//
// A law associates a target parameter with its initialization, variation, and
// transmission rules.
// -----------------------------------------------------------------------------

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