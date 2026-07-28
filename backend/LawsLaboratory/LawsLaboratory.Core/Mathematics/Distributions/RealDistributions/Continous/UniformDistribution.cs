using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;
    public sealed class UniformDistribution : IDistribution<double>
    {
        private readonly double _min;
        private readonly double _rangeSize;
        private readonly IRandomGenerator _random;

        public UniformDistribution(double min, double max, IRandomGenerator random) {

            if (max < min)
                throw new ArgumentException("Maximum must be greater than minimum.");

            _min = min;
            _rangeSize = max - min;
            _random = random;
        }

        public double Generate()
        {
            double u = _random.NextDouble(); 

            return u * _rangeSize + _min;   
        }         
    }

