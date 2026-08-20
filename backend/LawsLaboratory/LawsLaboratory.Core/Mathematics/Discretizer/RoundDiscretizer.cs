// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Discretizer
//
// RoundDiscretizer.cs
//
// Discretizes a real-valued scalar by rounding it to the nearest integer.
//
// The discretizer performs only the rounding operation; interpretation of the
// resulting integer is left to the caller.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.Mathematics.Discretizer;

public sealed class RoundDiscretizer : IDiscretizer<double, int>
{
    public int Discretize(double value)
    {
        return (int)Math.Round(value);
    }
}