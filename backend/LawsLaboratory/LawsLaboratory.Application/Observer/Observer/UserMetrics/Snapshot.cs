using MathNet.Numerics.LinearAlgebra;

namespace LawsLaboratory.Application.Observer.Observer.UserMetrics;

public readonly record struct StatisticsSnapshot
(
    long Count,
    double Mean,
    double Variance,
    double StandardDeviation,
    double Minimum,
    double Maximum
)
{
    public double Range => Maximum - Minimum;
}


public readonly record struct ParameterMetricSnapshot
(
    ushort ParameterId,
    StatisticsSnapshot Spatial,
    StatisticsSnapshot Temporal
);


public readonly record struct UserMetricSnapshot
(
    IReadOnlyDictionary<ushort, ParameterMetricSnapshot> Parameters,
    Matrix<double> Covariance,
    Matrix<double> Correlation
);