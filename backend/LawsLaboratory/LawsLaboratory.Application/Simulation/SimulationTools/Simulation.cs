// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation
//
// Simulation.cs
//
// Represents the runtime simulation and orchestrates the execution of
// simulation cycles.
//
// The Simulation is responsible for:
//   1. maintaining the lifecycle state of the simulation;
//   2. executing variation and transmission phases for each cycle;
//   3. applying the configured delay between simulation cycles;
//   4. enforcing the configured maximum number of cycles when one is defined;
//   5. coordinating pause and resume operations;
//   6. exposing user metrics through snapshots;
//   7. exposing the current grid state through a binary grid view.
//
// The Simulation delegates the execution of variation and transmission to the
// TaskCoordinator. It does not directly perform spatial operations, law
// evaluation, or engine communication.
//
// The simulation lifecycle is represented by SimulationState, including the
// initialization, running, paused, and completed states.
//
// A simulation may run for a configured maximum number of cycles or continue
// indefinitely when no maximum cycle count is specified.
//
// GridObserver and UserMetricObserver provide read-only observation of the
// simulation state and metrics without participating in the execution pipeline.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;
using LawsLaboratory.Application.Simulation.Observer.Observer.UserMetrics;
using LawsLaboratory.Application.Simulation.TaskCoordinatorNameSpace;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Application.Simulation.GridObservation;
using LawsLaboratory.Application.Execution.Gateway;


namespace LawsLaboratory.Application.Simulation.SimulationTools;

enum SimulationState
{
    Initiating,
    Initiated,
    InitializationFailed,
    Running,
    Paused,
    Completed,
}

internal sealed class SimulationRuntime
{
    private long? MaxCycle;

    private  int? DelayMsPerCycle;
    private TaskCoordinator TaskCoordinator { get; }

    private UserMetricObserver UserMetricObserver { get; }

    private GridObserver<PlanePosition> GridObserver { get; }

    private EngineGateway EngineGateway { get; }

    public SimulationState SimulationState { get; private set; }

    public SimulationRuntime(  
                        GridObserver<PlanePosition> gridObserver,
                        TaskCoordinator taskCoordinator,
                        UserMetricObserver userMetricObserver,
                        TimeConfiguration timeConfiguration,
                        EngineGateway engineGateway)
    {
        GridObserver = gridObserver;
        TaskCoordinator = taskCoordinator;
        UserMetricObserver = userMetricObserver;
        MaxCycle = timeConfiguration.MaxCycles;
        DelayMsPerCycle = timeConfiguration.DelayMsPerCycle;
        EngineGateway = engineGateway;

        SimulationState = SimulationState.Initiated;
    }

    public async Task LaunchSimulation()
    {
   
        SimulationState = SimulationState.Running;

        if (MaxCycle.HasValue)
        {   
            for (int i = 0; i < MaxCycle.Value; i++) 
            {
                await executeSimulation(DelayMsPerCycle ?? 0);
            }

            await TaskCoordinator.StartVariation();

            SimulationState = SimulationState.Completed;

            return;
        }
        while (true) {
            await executeSimulation(DelayMsPerCycle ?? 0);
        }
    }

    public UserMetricSnapshot GetUserMetric()
    {
        return UserMetricObserver.CreateSnapshot();
    }

    private async Task executeSimulation(int delayMs)
    {
        await TaskCoordinator.StartVariation();

        await TaskCoordinator.StartTransmission();

        await Task.Delay(delayMs);
    }

    public void Pause() 
    {
        if (SimulationState != SimulationState.Running) 
            return; 

        SimulationState = SimulationState.Paused;

        TaskCoordinator.Pause();
    }

    public void Resume()
    {
        if (SimulationState != SimulationState.Paused)
            return;

        SimulationState = SimulationState.Running;

        TaskCoordinator.Resume();
    }

    public  GridBinaryView getGridView()
    {
        return GridObserver.CaptureGrid();
    }
}


