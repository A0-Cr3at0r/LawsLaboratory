using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;
    /// <summary>
    /// Generates log-normal distributed values.
    /// A random variable X is log-normal if ln(X)
    /// follows a normal distribution N(mean, standardDeviation²).
    /// </summary>
    public sealed class LogNormalDistribution : IDistribution<double>
        {
        private readonly NormalDistribution _normalDistribution;

        public LogNormalDistribution(
            double mean,
            double standardDeviation,
            IRandomGenerator random)
        {
            _normalDistribution = new NormalDistribution(
                mean,
                standardDeviation,
                random);
        }

        public double Generate()
        {
            double normalValue = _normalDistribution.Generate();

            return Math.Exp(normalValue);
        }
    }
