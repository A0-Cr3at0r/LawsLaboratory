// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration / ExecutionConfiguration
//
// LawConfiguration.cs
//
// Defines the declarative configuration of a parameter law.
//
// A law is described by its variation and transmission formulas, its
// initialization configuration and its transmission destinations. Formulas
// remain represented as strings and are compiled later by the FormulaCompiler.
//
// The Initializer and its builders consume  this configuration to construct the corresponding
// Core Law and its runtime rules.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;

public sealed record LawConfiguration
{
    public required string VariationFormula { get; init; }

    public required string TransmissionFormula { get; init; }

    public required InitializationConfiguration InitializationConfiguration { get; init; }

    public required PlanePositionConfiguration[] TransmissionDestinations { get; init; }
}

public sealed class PlanePositionConfiguration
{
    public int X { get; init; }
    public int Y { get; init; }
}
