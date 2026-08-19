// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// BetaDistribution.cs
//
// Represents the Beta(alpha, beta) probability distribution on the interval
// [0, 1].
//
// Samples are generated using the relationship between the Beta and Gamma
// distributions:
//
//     X ~ Gamma(alpha, 1)
//     Y ~ Gamma(beta, 1)
//     B = X / (X + Y)
//
// Requires alpha > 0 and beta > 0.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions
{
    public sealed class BetaDistribution : IDistribution<double>
    {   

        private readonly GammaDistribution _gammaDistributionAlpha;
        private readonly GammaDistribution _gammaDistributionBeta;


        public BetaDistribution(double alpha, double beta, IRandomGenerator random) {
            if (alpha <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(alpha));
            }

            if (beta <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(beta));
            }


            _gammaDistributionAlpha = new GammaDistribution(alpha, 1, random);
            _gammaDistributionBeta = new GammaDistribution(beta, 1,random);

        }

        public double Generate() { 
            double x = _gammaDistributionAlpha.Generate();
            double y = _gammaDistributionBeta.Generate();
            
            return x / (x + y); 
        }
    }
}
