using LawsLaboratory.Application.Observer.Observation;
using MathNet.Numerics.LinearAlgebra;

namespace LawsLaboratory.Application.Observer.Observer.UserMetrics;


public sealed class UserMetricObserver
{
    private readonly Dictionary<ushort, ParameterStatistics> _parameters;

    private readonly ushort[] _parameterOrder;

    private readonly CovarianceAccumulator _covariance;

    private int _completedParameters;


    public UserMetricObserver(
        IEnumerable<ushort> parameterIds,
        int cellCount)
    {
        _parameterOrder = parameterIds.ToArray();


        _parameters =
            _parameterOrder.ToDictionary(
                id => id,
                _ => new ParameterStatistics(cellCount));


        _covariance =
            new CovarianceAccumulator(
                _parameterOrder.Length);


        _completedParameters = 0;
    }


    public void Notify(
        UserMetricObservation observation)
    {
        ParameterStatistics parameter =
            _parameters[observation.ParameterId];
    
        parameter.ObserveCell(observation.Value);

        bool completed =
            parameter.CompleteIterationIfReady();


        if (completed)
        {
            _completedParameters++;

            TryUpdateCovariance();
        }
    }


    private void TryUpdateCovariance()
    {
        if (_completedParameters != _parameters.Count)
            return;


        Vector<double> vector =
            Vector<double>.Build.Dense(
                _parameterOrder.Length);


        for (int i = 0; i < _parameterOrder.Length; i++)
        {
            vector[i] =
                _parameters[_parameterOrder[i]]
                .CurrentTemporalMean;
        }

        _covariance.Push(vector);

        _completedParameters = 0;
    }


    public UserMetricSnapshot CreateSnapshot()
    {
        var parameters =
            _parameters
            .Select(pair =>
                pair.Value.CreateSnapshot(pair.Key))
            .ToDictionary(
                x => x.ParameterId);


        return new UserMetricSnapshot(
            parameters,
            _covariance.Covariance,
            _covariance.Correlation);
    }
}