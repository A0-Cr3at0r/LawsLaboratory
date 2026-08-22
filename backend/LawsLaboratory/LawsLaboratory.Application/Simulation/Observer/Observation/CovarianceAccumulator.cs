// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Observer / Observation
//
// CovarianceAccumulator.cs
//
// Incrementally accumulates the covariance matrix of multivariate observations.
//
// A vector may contain NaN components when a parameter has no valid numerical
// observation for an iteration. Such components are excluded from covariance
// updates. Each covariance entry therefore uses only iterations for which both
// corresponding parameters have valid observations.
// -----------------------------------------------------------------------------

using MathNet.Numerics.LinearAlgebra;

namespace LawsLaboratory.Application.Simulation.Observer.Observation;

public sealed class CovarianceAccumulator
{
    private readonly int _dimension;

    private readonly long[,] _counts;

    private readonly Vector<double> _mean;

    private readonly Matrix<double> _m2;


    public CovarianceAccumulator(int dimension)
    {
        if (dimension <= 0)
            throw new ArgumentOutOfRangeException(nameof(dimension));


        _dimension = dimension;

        _counts =
            new long[dimension, dimension];

        _mean =
            Vector<double>.Build.Dense(dimension);

        _m2 =
            Matrix<double>.Build.Dense(
                dimension,
                dimension);
    }


    public void Push(
        Vector<double> values)
    {
        if (values.Count != _dimension)
            throw new ArgumentException(
                "The vector dimension does not match the accumulator dimension.",
                nameof(values));


        for (int i = 0; i < _dimension; i++)
        {
            double valueI =
                values[i];

            if (double.IsNaN(valueI))
                continue;


            for (int j = 0; j < _dimension; j++)
            {
                double valueJ =
                    values[j];

                if (double.IsNaN(valueJ))
                    continue;


                PushPair(
                    i,
                    j,
                    valueI,
                    valueJ);
            }
        }
    }


    private void PushPair(
        int i,
        int j,
        double valueI,
        double valueJ)
    {
        long count =
            ++_counts[i, j];


        double deltaI =
            valueI - _mean[i];

        double deltaJ =
            valueJ - _mean[j];


        _mean[i] +=
            deltaI / count;

        _mean[j] +=
            deltaJ / count;


        double delta2I =
            valueI - _mean[i];

        double delta2J =
            valueJ - _mean[j];


        _m2[i, j] +=
            deltaI * delta2J;
    }


    public Matrix<double> Covariance
    {
        get
        {
            Matrix<double> covariance =
                Matrix<double>.Build.Dense(
                    _dimension,
                    _dimension);


            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    long count =
                        _counts[i, j];

                    covariance[i, j] =
                        count > 0
                            ? _m2[i, j] / count
                            : double.NaN;
                }
            }


            return covariance;
        }
    }


    public Matrix<double> Correlation
    {
        get
        {
            Matrix<double> covariance =
                Covariance;

            Matrix<double> result =
                Matrix<double>.Build.Dense(
                    _dimension,
                    _dimension);


            for (int i = 0; i < _dimension; i++)
            {
                for (int j = 0; j < _dimension; j++)
                {
                    double covarianceValue =
                        covariance[i, j];

                    if (double.IsNaN(covarianceValue))
                    {
                        result[i, j] =
                            double.NaN;

                        continue;
                    }


                    double covarianceI =
                        covariance[i, i];

                    double covarianceJ =
                        covariance[j, j];


                    if (double.IsNaN(covarianceI) ||
                        double.IsNaN(covarianceJ))
                    {
                        result[i, j] =
                            double.NaN;

                        continue;
                    }


                    double denominator =
                        Math.Sqrt(
                            covarianceI *
                            covarianceJ);


                    result[i, j] =
                        denominator == 0
                            ? 0
                            : covarianceValue /
                              denominator;
                }
            }


            return result;
        }
    }
}