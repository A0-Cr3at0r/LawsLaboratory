using LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;
using LawsLaboratory.Application.Simulation.Observer.Observer.UserMetrics;
using LawsLaboratory.Application.Simulation.TaskCoordinatorNameSpace;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Application.Simulation.GridObservation;


namespace LawsLaboratory.Application.Simulation;

enum SimulationState
{
    Initiating,
    Initiated,
    Running,
    Paused,
    Completed,
}

internal sealed class Simulation
{
    private long? MaxCycle;

    private  int? DelayMsPerCycle;
    private TaskCoordinator TaskCoordinator { get; }

    private UserMetricObserver UserMetricObserver { get; }

    private GridObserver<PlanePosition> GridObserver { get; }

    public SimulationState SimulationState { get; private set; }

    public Simulation(  
                        GridObserver<PlanePosition> gridObserver,
                        TaskCoordinator taskCoordinator,
                        UserMetricObserver userMetricObserver,
                        TimeConfiguration timeConfiguration)
    {
        GridObserver = gridObserver;
        TaskCoordinator = taskCoordinator;
        UserMetricObserver = userMetricObserver;
        MaxCycle = timeConfiguration.MaxCycles;
        DelayMsPerCycle = timeConfiguration.DelayMsPerCycle;

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

        Task.Delay(delayMs).Wait();
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


