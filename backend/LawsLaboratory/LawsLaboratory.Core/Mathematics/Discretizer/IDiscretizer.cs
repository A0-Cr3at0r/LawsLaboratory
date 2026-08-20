// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Discretizer
//
// IDiscretizer.cs
//
// Defines a transformation from a continuous or otherwise non-discrete
// representation into a discrete representation.
//
// A discretizer performs only the mathematical conversion between the input
// and output representations. It does not perform validation, spatial
// containment, or simulation logic.
//
// The abstraction is generic so that different discretization strategies can
// be provided for different mathematical representations.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.Mathematics.Discretizer;

public interface IDiscretizer<TInput, TOutput>
{
    TOutput Discretize(TInput value);
}