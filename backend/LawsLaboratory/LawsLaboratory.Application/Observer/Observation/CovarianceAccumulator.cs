using MathNet.Numerics.LinearAlgebra;

namespace LawsLaboratory.Application.Observer.Observation;


public sealed class CovarianceAccumulator
{
    private readonly int _dimension;

    private long _count;

    private Vector<double> _mean;

    private Matrix<double> _m2;


    public CovarianceAccumulator(int dimension)
    {
        _dimension = dimension;

        _mean =
            Vector<double>.Build.Dense(dimension);

        _m2 =
            Matrix<double>.Build.Dense(
                dimension,
                dimension);
    }

    public void Push(Vector<double> values)
    {
        if (values.Count != _dimension)
            throw new ArgumentException();

        _count++;

        Vector<double> delta =
            values - _mean;

        _mean += delta / _count;

        Vector<double> delta2 =
            values - _mean;

        _m2 +=
            delta.OuterProduct(delta2);
    }

    public Matrix<double> Covariance =>
        _count > 0
            ? _m2 / _count
            : Matrix<double>.Build.Dense(
                _dimension,
                _dimension);

    public Matrix<double> Correlation
    {
        get
        {
            var covariance = Covariance;

            var result =
                Matrix<double>.Build.Dense(
                    _dimension,
                    _dimension);

            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    double denominator =
                        Math.Sqrt(
                            covariance[i, i] *
                            covariance[j, j]);

                    result[i, j] =
                        denominator == 0
                        ? 0
                        : covariance[i, j] /
                          denominator;
                }
            }

            return result;
        }
    }
}