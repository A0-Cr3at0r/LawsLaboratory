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