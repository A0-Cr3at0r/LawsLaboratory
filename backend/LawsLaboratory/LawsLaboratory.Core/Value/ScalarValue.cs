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