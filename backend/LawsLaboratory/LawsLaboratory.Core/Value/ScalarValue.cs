namespace LawsLaboratory.Core.Value
{
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

        public double? Get()
        {
            return Value;
        }

    }
}