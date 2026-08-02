namespace LawsLaboratory.Core.Value
{
    public sealed class Dead : IValue
    {
        public static Dead Instance { get; } = new();

        public IValue Set(double value)
        {
            return new ScalarValue(value);
        }

    }
}