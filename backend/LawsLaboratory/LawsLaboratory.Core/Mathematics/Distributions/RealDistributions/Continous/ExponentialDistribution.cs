// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// ExponentialDistribution.cs
//
// Represents the exponential distribution with rate parameter lambda.
//
// Samples are generated using inverse transform sampling:
//
//     X = -ln(U) / lambda
//
// where U is uniformly distributed on (0, 1).
//
// Requires lambda > 0.
// Reference:
// Devroye, L. (1986).
// Non-Uniform Random Variate Generation.
// Springer.
// -----------------------------------------------------------------------------
using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

    public sealed class ExponentialDistribution : IDistribution<double>
    {
        private readonly double _lambda;
        private readonly IRandomGenerator _random;

        public ExponentialDistribution(double lambda, IRandomGenerator random) {
            if (lambda <= 0)
                throw new   ArgumentOutOfRangeException(nameof(lambda));

            _lambda = -1 * lambda;
            _random = random;
        }

        public double Generate()
        {
            double u;
            do { 
                u = _random.NextDouble(); 
            } while (u <=0);

            return Math.Log(u) / _lambda;
        }
    }

