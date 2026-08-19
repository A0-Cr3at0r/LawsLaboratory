// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Value
//
// ScalarValue.cs
//
// Represents a live scalar value in the simulation.
//
// ScalarValue is mutable so that existing value instances can be reused
// by the simulation engine and its buffers without unnecessary allocations.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Value;

public sealed class ScalarValue : IValue
{
    private double Value { get; set; }

    public ScalarValue(double value)
    {
        Value = value;
    }

    public IValue Set(double value)
    {
        Value = value;
        return this;
    }

    public IValue Set(IValue value)
    {
        if (value is ScalarValue scalar)
        {
            Value = scalar.Value;
            return this;
        }

        return value.Clone();
    }

    public IValue Clone()
    {
        return new ScalarValue(Value);
    }

    public double? Get()
    {
        return Value;
    }
}