// -----------------------------------------------------------------------------
// LawsLaboratory
// Core / Mathematics / Distributions / RealDistributions
//
// StudentTDistribution.cs
//
// Represents the Student's t-distribution with the specified degrees of
// freedom.
//
// Samples are generated from the relationship:
//
//     T = Z / sqrt(V / nu)
//
// where Z ~ Normal(0, 1) and V ~ ChiSquare(nu).
//
// The chi-square variable is generated as a Gamma distribution with
// shape = nu / 2 and scale = 2.
//
// Requires degreesOfFreedom > 0.
// -----------------------------------------------------------------------------

using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.RealDistributions;

public sealed class StudentTDistribution : IDistribution<double>
{
    private readonly double _degreesOfFreedom;

    private readonly IRandomGenerator _random;

    private readonly NormalDistribution _normal;
    private readonly GammaDistribution _chiSquare;


    public StudentTDistribution(
        double degreesOfFreedom,
        IRandomGenerator random)
    {
        if (degreesOfFreedom <= 0)
            throw new ArgumentOutOfRangeException(nameof(degreesOfFreedom));

        _degreesOfFreedom = degreesOfFreedom;
        _random = random;

        _normal = new NormalDistribution(
            0,
            1,
            random);

        _chiSquare = new GammaDistribution(
            degreesOfFreedom / 2,
            2,
            random);
    }


    public double Generate()
    {
        double z = _normal.Generate();

        double v = _chiSquare.Generate();

        return z /
               Math.Sqrt(
                   v / _degreesOfFreedom);
    }
}