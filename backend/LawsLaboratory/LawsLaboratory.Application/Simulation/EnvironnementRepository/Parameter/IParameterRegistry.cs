// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / EnvironnementRepository / Parameter
//
// IParameterRegistry.cs
//
// Defines the registry responsible for the identity of simulation parameters.
//
// The registry provides the bidirectional mapping between parameter names and
// their compact ushort identifiers.
//
// Parameter identifiers are the canonical identifiers used by the simulation
// runtime, while names are primarily used when defining and compiling formulas.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.EnvironnementRepository.Parameter;

public interface IParameterRegistry
{
    int Count { get; }

    IReadOnlyCollection<string> ParameterNames { get; }

    bool ContainsParameter(
        string parameterName);

    ushort GetParameterId(
        string parameterName);

    string GetParameterName(
        ushort parameterId);
}