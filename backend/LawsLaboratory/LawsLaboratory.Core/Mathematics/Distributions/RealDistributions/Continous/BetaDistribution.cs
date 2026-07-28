using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions
{
    public sealed class BetaDistribution : IDistribution<double>
    {   
        private readonly double _alpha;
        private readonly double _beta;

        private readonly IRandomGenerator _random;
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

            _alpha = alpha;
            _beta = beta;

            _random = random;

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
