// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Formula / Element
//
// ConstantElement.cs
//
// Represents a numeric constant in the intermediate formula representation.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.Formula.Element;

public sealed class ConstantElement : ExpressionElement
    {   
        public double Value { get; }
        public ConstantElement( double value) { 
            Value = value;
        }

    }

