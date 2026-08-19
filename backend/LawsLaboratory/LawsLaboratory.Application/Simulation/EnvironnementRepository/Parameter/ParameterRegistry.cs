// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / EnvironnementRepository / Parameter
//
// ParameterRegistry.cs
//
// Provides the canonical registration and identity mapping of simulation
// parameters.
//
// Parameters are assigned consecutive ushort identifiers in registration
// order. The registry maintains both:
//
//     name → ParameterId
//     ParameterId → name
//
// Parameter identifiers are stable within the registry and are used throughout
// the simulation runtime to avoid repeatedly carrying parameter names.
//
// The registry validates parameter names, rejects duplicates, and enforces
// the maximum number of parameters representable by ushort identifiers.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.EnvironnementRepository.Parameter;

using System.Collections.Immutable;

public sealed class ParameterRegistry : IParameterRegistry
{
    private readonly ImmutableDictionary<string, ushort> _parameterIds;

    private readonly ImmutableArray<string> _parameterNames;


    public int Count => _parameterNames.Length;


    public IReadOnlyCollection<string> ParameterNames =>
        _parameterNames;

    public ParameterRegistry(
        IReadOnlyCollection<string> parameterNames)
    {
        ArgumentNullException.ThrowIfNull(parameterNames);

        ImmutableDictionary<string, ushort>.Builder builder =
            ImmutableDictionary.CreateBuilder<string, ushort>();

        ImmutableArray<string>.Builder names =
            ImmutableArray.CreateBuilder<string>(
                parameterNames.Count);


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

    public bool ContainsParameter(string parameterName)
    {
        return _parameterIds.ContainsKey(parameterName);
    }


    public ushort GetParameterId(
    string parameterName)
    {
        ArgumentNullException.ThrowIfNull(parameterName);

        if (!_parameterIds.TryGetValue(
                parameterName,
                out ushort parameterId))
        {
            throw new KeyNotFoundException(
                $"Unknown parameter '{parameterName}'.");
        }

        return parameterId;
    }
    public string GetParameterName(
    ushort parameterId)
    {
        if (parameterId >= _parameterNames.Length)
        {
            throw new ArgumentOutOfRangeException(
                nameof(parameterId));
        }

        return _parameterNames[parameterId];
    }
}