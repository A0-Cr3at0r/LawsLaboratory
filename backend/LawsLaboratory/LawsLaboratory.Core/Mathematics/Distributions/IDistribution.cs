// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions
//
// IDistribution.cs
//
// Defines an abstraction for a source capable of generating values according
// to a probability distribution.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Core.Mathematics.Distributions;

public interface IDistribution<T>
{
    T Generate();
}