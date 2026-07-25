namespace LawsLaboratory.Core.Mathematics.Distributions;

public interface IDistribution<T>
{
    T Generate();
}