// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / RandomGenerators
//
// IRandomGenerator.cs
//
// Provides the source of pseudo-random values used by probabilistic models.
//
// Implementations abstract the underlying random number generator from
// probability distributions, allowing the sampling algorithms to remain
// independent of the concrete random number source.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Core.Mathematics.RandomGenerators;

public interface IRandomGenerator
{
    double NextDouble();
}