using LawsLaboratory.Core.Mathematics.RandomGenerators;

namespace LawsLaboratory.Core.Mathematics.Distributions.DiscreteDistributions;

/// <summary>
/// Generates Poisson distributed values using a hybrid approach:
/// Knuth's algorithm for small lambda values and PTRS
/// (Poisson Transformed Rejection with Squeeze) for large lambda values.
///
/// PTRS is an efficient exact rejection algorithm designed for
/// Poisson distributions with large lambda values.
///
/// References:
/// Hörmann, W. (1993).
/// The Transformed Rejection Method for Generating Poisson Random Variables.
/// Insurance: Mathematics and Economics, 12(1), 39-45.
///
/// Knuth, D. E. (1969).
/// The Art of Computer Programming, Volume 2:
/// Seminumerical Algorithms.
/// Addison-Wesley.
/// </summary>
public sealed class PoissonDistribution : IDistribution<int>
{
    private readonly double _lambda;
    private readonly IRandomGenerator _random;


    public PoissonDistribution(
        double lambda,
        IRandomGenerator random)
    {
        if (lambda <= 0)
            throw new ArgumentOutOfRangeException(nameof(lambda));

        _lambda = lambda;
        _random = random;
    }


    public int Generate()
    {
        if (_lambda < 30)
            return GenerateKnuth();


        return GeneratePTRS();
    }


    private int GeneratePTRS()
    {
        double sqrtLambda = Math.Sqrt(_lambda);
        double logLambda = Math.Log(_lambda);

        double b =
            0.931 +
            2.53 * sqrtLambda;

        double a =
            -0.059 +
            0.02483 * b;

        double invAlpha =
            1.1239 +
            1.1328 / (b - 3.4);

        double vR =
            0.9277 -
            3.6224 / (b - 2);


        while (true)
        {
            double u = _random.NextDouble() - 0.5;
            double v = _random.NextDouble();


            double us =
                0.5 - Math.Abs(u);


            int k =
                (int)Math.Floor(
                    (2 * a / us + b) * u
                    + _lambda
                );


            if (k < 0)
                continue;


            if (us >= 0.07 &&
                v <= vR)
            {
                return k;
            }


            double lhs =
                Math.Log(v * invAlpha /
                (a / (us * us) + b));


            double rhs =
                -_lambda +
                k * logLambda -
                LogFactorial(k);


            if (lhs <= rhs)
                return k;
        }
    }


    private int GenerateKnuth()
    {
        double limit =
            Math.Exp(-_lambda);

        int count = 0;

        double product = 1;


        while (product > limit)
        {
            count++;

            double u;

            do
            {
                u = _random.NextDouble();
            }
            while (u <= 0);


            product *= u;
        }


        return count - 1;
    }


    private static double LogFactorial(int n)
    {
        double result = 0;


        for (int i = 2; i <= n; i++)
        {
            result += Math.Log(i);
        }


        return result;
    }
}