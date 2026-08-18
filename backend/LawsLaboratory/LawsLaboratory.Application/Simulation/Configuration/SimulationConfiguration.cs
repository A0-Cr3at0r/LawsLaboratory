using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;

namespace LawsLaboratory.Application.Simulation.Configuration;

public class SimulationConfiguration
{
    public required ModelConfiguration Model { get; init; }

    public required RuntimeConfiguration runtime { get; init; }

}