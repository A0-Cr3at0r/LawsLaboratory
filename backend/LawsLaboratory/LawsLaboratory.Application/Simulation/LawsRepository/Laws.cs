namespace LawsLaboratory.Application.Simulation.LawsRepository;

using LawsLaboratory.Core.Laws;

public sealed class Laws
{
    private readonly Law[] _laws;

    public int Count => _laws.Length;

    public Laws(Law[] laws)
    {
        ArgumentNullException.ThrowIfNull(laws);

        _laws = laws;

        for (ushort i = 0; i < laws.Length; i++)
        {
            if (laws[i].TargetParameterId != i)
            {
                throw new InvalidOperationException(
                    "Law ordering must match parameter registry.");
            }
        }
    }

    public Law this[ushort parameterId]
    {
        get => _laws[parameterId];
    }
    public Law GetLaw(
        ushort parameterId)
    {
        return _laws[parameterId];
    }

    public IReadOnlyList<Law> GetAll()
    {
        return _laws;
    }
}