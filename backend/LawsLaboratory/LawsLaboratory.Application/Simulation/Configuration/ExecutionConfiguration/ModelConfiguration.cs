// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Configuration / ExecutionConfiguration
//
// ModelConfiguration.cs
//
// Defines the complete declarative model configuration of a simulation.
//
// The configuration associates each user-facing parameter name with the law
// governing its variation, transmission and initialization. Parameter names
// remain the configuration-level identity; the ParameterRegistry resolves
// them to runtime parameter identifiers when the model is built.
//
// The Initializer and its builders consume  this configuration to construct the complete
// runtime Laws model.
// -----------------------------------------------------------------------------
namespace LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;

public sealed record ModelConfiguration
{
    public required Dictionary<string, LawConfiguration> ParametersLaws { get; init; }
}
