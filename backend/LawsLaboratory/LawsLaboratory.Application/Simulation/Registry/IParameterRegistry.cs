namespace LawsLaboratory.Application.Simulation.Registry;

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