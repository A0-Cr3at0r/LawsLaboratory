using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;

namespace LawsLaboratory.Application.Simulation.Configuration;

public class SimulationConfiguration
{
    public ModelConfiguration Model { get; init; }

    public RuntimeConfiguration runtime { get; init; }

}