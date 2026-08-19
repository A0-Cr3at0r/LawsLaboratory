// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Laws
//
// VariationRule.cs
//
// Defines the formula used to calculate the next value of a parameter from
// the current simulation state.
//
// The original formula is retained alongside its compiled representation.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.Laws;

using LawsLaboratory.Core.Formula.Element;

public sealed class VariationRule
{
    public string Formula { get; }

    internal CompiledExpression CompiledExpression { get; }

    public VariationRule(string formula, CompiledExpression compiledExpression)
    {
        Formula = formula;
        CompiledExpression = compiledExpression;
    }

}