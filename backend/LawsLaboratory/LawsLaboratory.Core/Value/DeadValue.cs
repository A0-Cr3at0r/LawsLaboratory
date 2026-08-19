// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Value
//
// Dead.cs
//
// Represents the absence of a live value in the simulation.
// Dead is stateless and therefore implemented as a singleton.
//
// A Dead value:
// - returns null from Get();
// - becomes a ScalarValue when assigned a scalar;
// - clones to Dead.Instance.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Value;

public sealed class Dead : IValue
{
    public static Dead Instance { get; } = new();

    private Dead()
    {
    }

    public IValue Set(double value)
    {
        return new ScalarValue(value);
    }

    public IValue Set(IValue value)
    {
        return value.Clone();
    }

    public IValue Clone()
    {
        return Instance;
    }

    public double? Get()
    {
        return null;
    }
}