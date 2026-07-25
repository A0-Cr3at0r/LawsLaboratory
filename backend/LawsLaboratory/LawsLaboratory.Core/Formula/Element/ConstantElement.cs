namespace LawsLaboratory.Core.Formula.Element;
    public sealed class ConstantElement : ExpressionElement
    {   
        public double Value { get; }
        public ConstantElement( double value) { 
            Value = value;
        }

    }

