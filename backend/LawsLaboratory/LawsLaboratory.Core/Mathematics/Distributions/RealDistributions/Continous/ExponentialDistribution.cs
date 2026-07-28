using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

    /// <summary>
    /// Generates exponentially distributed random values.
    /// Uses inverse transform sampling:
    /// X = -ln(U) / lambda
    /// 
    /// Reference:
    /// Devroye, L. (1986).
    /// Non-Uniform Random Variate Generation.
    /// Springer.
    /// </summary>
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

