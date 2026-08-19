// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// ConstantDistribution.cs
//
// Represents a degenerate probability distribution whose random variable
// always takes the same value.
//
// Each call to Generate() returns the configured constant.
// -----------------------------------------------------------------------------

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

