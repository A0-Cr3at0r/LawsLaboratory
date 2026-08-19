// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// GeneralizedBetaDistribution.cs
//
// Represents a Beta distribution transformed from [0, 1] to an interval
// starting at min with the specified size.
//
// If B follows Beta(alpha, beta), the generated value is:
//
//     X = min + size * B
//
// Requires size > 0.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions
{
    public sealed class GeneralizedBetaDistribution : IDistribution<double>
    {
        private readonly double _min;
        private readonly double _size;

        private readonly BetaDistribution _beta;

        public GeneralizedBetaDistribution(double min, 
            double size, double alpha, 
            double beta, IRandomGenerator random)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            _min = min;
            _size = size;
            _beta = new BetaDistribution(alpha, beta, random);

        }

        public double Generate()
        {
            return _beta.Generate() * _size  + _min ;    
        }
    }
}
