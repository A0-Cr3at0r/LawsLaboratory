namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;
    public sealed class ConstantDistribution  : IDistribution<double>
    {   
        private readonly double _constant;
        public ConstantDistribution(double constant)
        {
            _constant = constant;
        }

        public double Generate()
        {
            return _constant;
        }
    }

