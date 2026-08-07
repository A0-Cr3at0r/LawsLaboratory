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