namespace LawsLaboratory.Application.Simulation.Registry;

public interface IParameterRegistry
{
    int Count { get; }

    IReadOnlyCollection<string> ParameterNames { get; }

    bool TryGetParameterId(
        string parameterName,
        out ushort parameterId);

    bool TryGetParameterName(
        ushort parameterId,
        out string parameterName);
}