namespace LawsLaboratory.Application.Simulation.Registry;

using System.Collections.Immutable;

public sealed class ParameterRegistry : IParameterRegistry
{
    private readonly ImmutableDictionary<string, ushort> _parameterIds;

    private readonly ImmutableArray<string> _parameterNames;


    public int Count => _parameterNames.Length;


    public IReadOnlyCollection<string> ParameterNames =>
        _parameterNames;

    public ParameterRegistry(
        IEnumerable<string> parameterNames)
    {
        ArgumentNullException.ThrowIfNull(parameterNames);

        ImmutableDictionary<string, ushort>.Builder builder =
            ImmutableDictionary.CreateBuilder<string, ushort>();

        ImmutableArray<string>.Builder names =
            ImmutableArray.CreateBuilder<string>();


        foreach (string parameterName in parameterNames)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(
                parameterName);

            if (builder.ContainsKey(parameterName))
            {
                throw new ArgumentException(
                    $"Parameter '{parameterName}' is already registered.");
            }

            if (names.Count == ushort.MaxValue + 1)
            {
                throw new InvalidOperationException(
                    "The maximum number of parameters has been reached.");
            }

            ushort parameterId = (ushort)names.Count;

            builder.Add(
                parameterName,
                parameterId);

            names.Add(parameterName);
        }

        _parameterIds = builder.ToImmutable();

        _parameterNames = names.MoveToImmutable();
    }


    public bool TryGetParameterId(
        string parameterName,
        out ushort parameterId)
    {
        ArgumentNullException.ThrowIfNull(parameterName);

        return _parameterIds.TryGetValue(
            parameterName,
            out parameterId);
    }


    public bool TryGetParameterName(
        ushort parameterId,
        out string parameterName)
    {
        if (parameterId >= _parameterNames.Length)
        {
            parameterName = string.Empty;
            return false;
        }

        parameterName = _parameterNames[parameterId];
        return true;
    }
}