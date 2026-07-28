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
