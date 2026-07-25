namespace LawsLaboratory.Core.Value
{
    public sealed class ScalarValue : IValue
    {
        private double Value { get; set;  }
        internal ScalarValue(double value) { 
            Value = value;
        }
        public IValue Set(double value) {
            Value = value;
            return this;
        }

        internal double get()
        {
            return Value;
        }

     }
}
