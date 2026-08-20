// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Observer / Observer / UserMetrics
//
// UserMetricSnapshot.cs
//
// Defines immutable snapshots of the statistics collected for simulation
// parameters.
//
// StatisticsSnapshot contains the numerical statistics of one observation set.
// ParameterMetricSnapshot groups the spatial and temporal statistics of one
// parameter. UserMetricSnapshot combines all parameter statistics with the
// covariance and correlation matrices computed across parameters.
//
// Snapshots expose collected results without exposing the mutable accumulators
// used during observation.
// -----------------------------------------------------------------------------

using MathNet.Numerics.LinearAlgebra;

namespace LawsLaboratory.Application.Simulation.Observer.Observer.UserMetrics;

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