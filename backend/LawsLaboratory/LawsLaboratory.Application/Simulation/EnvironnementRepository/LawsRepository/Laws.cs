// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / EnvironnementRepository / LawsRepository
//
// Laws.cs
//
// Provides indexed access to the laws governing the simulation parameters.
//
// Each parameter is associated with exactly one Law, and the Law collection
// uses the same ordering as the ParameterRegistry:
//
//     ParameterId == Law collection index
//
// This invariant allows laws to be retrieved directly by ParameterId without
// an additional lookup structure.
//
// The supplied array is defensively copied during construction. The repository
// is therefore immutable after construction: its internal law collection
// cannot be modified through the original input array.
//
// The collection is intended to be a stable repository shared by the execution
// components throughout a simulation.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.EnvironnementRepository.LawsRepository;

using LawsLaboratory.Core.Laws;

public sealed class Laws
{
    private readonly Law[] _laws;

    public int Count => _laws.Length;

    public Laws(Law[] laws)
    {
        ArgumentNullException.ThrowIfNull(laws);

        _laws = (Law[])laws.Clone();

        for (ushort i = 0; i < _laws.Length; i++)
        {
            if (_laws[i].TargetParameterId != i)
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