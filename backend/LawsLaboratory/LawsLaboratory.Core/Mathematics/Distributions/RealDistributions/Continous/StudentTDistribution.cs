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