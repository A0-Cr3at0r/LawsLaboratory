namespace LawsLaboratory.Core.Mathematics.Discretizer;

public sealed class RoundDiscretizer : IDiscretizer<double, int>
{
    public int Discretize(double value)
    {
        return (int)Math.Round(value);
    }
}