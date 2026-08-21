// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / SimulationTools
//
// Initializer.cs
//
// Builds and assembles the complete runtime infrastructure required to execute
// a simulation.
//
// The Initializer is responsible for:
//   1. building the parameter registry and configured laws;
//   2. constructing the initial simulation grid;
//   3. monitoring grid construction and enforcing the initialization timeout;
//   4. building the spatial repository and execution infrastructure;
//   5. creating and configuring the EngineGateway;
//   6. creating and configuring the TaskCoordinator and observers;
//   7. assembling the final SimulationRuntime.
//
// Grid construction is executed asynchronously because parameter initialization
// may require a significant and potentially unbounded amount of computation.
// Each parameter is allowed a maximum initialization time independently.
//
// The Initializer monitors the GridBuilder while construction is in progress.
// Progress is reported through the provided asynchronous log callback.
//
// The CancellationToken supplied by the caller represents external
// cancellation. The Initializer additionally creates an internal cancellation
// source to enforce the maximum initialization time.
//
// Initialization errors are reported through workflowSimulationLog with the
// InitializationFailed simulation state. The original exception is preserved
// and rethrown after the failure has been reported.
//
// The Initializer exists only during the construction phase. Once initialization
// completes, ownership of the runtime infrastructure is transferred to the
// resulting SimulationRuntime.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Execution.Gateway;
using LawsLaboratory.Application.Simulation.Build;
using LawsLaboratory.Application.Simulation.Build.Factories;
using LawsLaboratory.Application.Simulation.Build.SpatialBuild;
using LawsLaboratory.Application.Simulation.Configuration;
using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.LawsRepository;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Parameter;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;
using LawsLaboratory.Application.Simulation.GridObservation;
using LawsLaboratory.Application.Simulation.Logs;
using LawsLaboratory.Application.Simulation.Observer;
using LawsLaboratory.Application.Simulation.Observer.Observer.UserMetrics;
using LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;
using LawsLaboratory.Application.Simulation.TaskCoordinatorNameSpace;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;

namespace LawsLaboratory.Application.Simulation.SimulationTools;

internal sealed class Initializer
{
    private static readonly TimeSpan ParameterInitializationTimeout =
        TimeSpan.FromMinutes(5);

    private static readonly TimeSpan MonitoringInterval =
        TimeSpan.FromMilliseconds(50);

    private const ushort FirstParameterId = 0;


    public async Task<SimulationRuntime> LaunchInitializationAsync(
        SimulationConfiguration simulationConfiguration,
        CancellationToken cancellation,
        Func<workflowSimulationLog, Task> sendLog)
    {
        ArgumentNullException.ThrowIfNull(simulationConfiguration);
        ArgumentNullException.ThrowIfNull(sendLog);

        try
        {
            await sendLog(
                new workflowSimulationLog(
                    SimulationState.Initiating,
                    "Building simulation configuration."));

            ModelConfiguration modelConfiguration =
                simulationConfiguration.Model;

            IParameterRegistry parameterRegistry =
                BuildParameterRegistry(modelConfiguration);

            int parameterCount =
                parameterRegistry.Count;

            IEnumerable<ushort> parameterIds =
                parameterRegistry.GetParameterIds();

            Laws laws =
                BuildLaws(
                    modelConfiguration,
                    parameterRegistry);

            await sendLog(
                new workflowSimulationLog(
                    SimulationState.Initiating,
                    "Starting grid construction.",
                    0,
                    parameterCount));

            IGrid<PlanePosition> grid =
                await BuildGridAsync(
                    simulationConfiguration.Runtime.Grid,
                    laws,
                    parameterRegistry,
                    cancellation,
                    sendLog);

            int cellCount =
                grid.Size;

            await sendLog(
                new workflowSimulationLog(
                    SimulationState.Initiating,
                    "Grid construction completed.",
                    parameterCount,
                    parameterCount));

            await sendLog(
                new workflowSimulationLog(
                    SimulationState.Initiating,
                    "Building simulation execution infrastructure."));

            SpatialRepository spatialRepository =
                BuildSpatialRepository(
                    laws,
                    grid.Width);

            EngineGateway engineGateway =
                new EngineGateway(
                    cellCount,
                    parameterCount,
                    Environment.ProcessorCount,
                    laws
                        .GetLaw(FirstParameterId)
                        .GetVariationExpression()
                        .Program);

            UserMetricObserver userMetricObserver =
                new UserMetricObserver(
                    parameterIds,
                    cellCount);

            TaskCoordinator taskCoordinator =
                BuildTaskCoordinator(
                    engineGateway,
                    grid,
                    spatialRepository,
                    userMetricObserver,
                    parameterRegistry,
                    laws,
                    cellCount,
                    parameterCount);

            GridObserver<PlanePosition> gridObserver =
                BuildGridObserver(
                    grid,
                    parameterIds,
                    parameterCount);

            await sendLog(
                new workflowSimulationLog(
                    SimulationState.Initiated,
                    "Simulation initialized."));

            return new SimulationRuntime(
                gridObserver,
                taskCoordinator,
                userMetricObserver,
                simulationConfiguration.Runtime.Time,
                engineGateway);
        }
        catch (Exception exception)
        {
            await sendLog(
                new workflowSimulationLog(
                    SimulationState.InitializationFailed,
                    $"Simulation initialization failed: {exception.Message}"));

            throw;
        }
    }

    private IParameterRegistry BuildParameterRegistry(
        ModelConfiguration modelConfiguration)
    {
        return new ParameterRegistry(
            modelConfiguration.ParametersLaws.Keys);
    }

    private Laws BuildLaws(
        ModelConfiguration modelConfiguration,
        IParameterRegistry parameterRegistry)
    {
        LawsBuilder lawsBuilder =
            new LawsBuilder(parameterRegistry);

        return lawsBuilder.Build(modelConfiguration);
    }

    private async Task<IGrid<PlanePosition>> BuildGridAsync(
    GridConfiguration gridConfiguration,
    Laws laws,
    IParameterRegistry parameterRegistry,
    CancellationToken cancellation,
    Func<workflowSimulationLog, Task> sendLog)
    {
        GridBuilder gridBuilder =
            new GridBuilder();

        using CancellationTokenSource initializationCancellation =
            CancellationTokenSource.CreateLinkedTokenSource(
                cancellation);

        Task<PlaneGrid> gridTask =
            Task.Run(
                () => gridBuilder.Build(
                    gridConfiguration,
                    laws.GetAll(),
                    initializationCancellation.Token));

        try
        {
            await MonitorGridConstructionAsync(
                gridBuilder,
                laws.Count,
                parameterRegistry,
                gridTask,
                initializationCancellation,
                sendLog);

            return await gridTask;
        }
        catch
        {
            initializationCancellation.Cancel();

            try
            {
                await gridTask;
            }
            catch (OperationCanceledException)
            {
            }

            throw;
        }
    }

    private async Task MonitorGridConstructionAsync(
    GridBuilder gridBuilder,
    int parameterCount,
    IParameterRegistry parameterRegistry,
    Task<PlaneGrid> gridTask,
    CancellationTokenSource initializationCancellation,
    Func<workflowSimulationLog, Task> sendLog)
    {
        int lastReportedCount = -1;

        ushort? monitoredParameterId = null;

        DateTime parameterStartTime = DateTime.UtcNow;

        while (!gridTask.IsCompleted)
        {
            initializationCancellation.Token.ThrowIfCancellationRequested();

            ushort currentParameterId =
                gridBuilder.CurrentParameterId;

            if (monitoredParameterId != currentParameterId)
            {
                monitoredParameterId =
                    currentParameterId;

                parameterStartTime =
                    DateTime.UtcNow;

                lastReportedCount = -1;
            }

            int initializedParameterCount =
                gridBuilder.InitializedParameterCount;

            if (initializedParameterCount != lastReportedCount)
            {
                lastReportedCount =
                    initializedParameterCount;

                await sendLog(
                    new workflowSimulationLog(
                        SimulationState.Initiating,
                        "Initializing simulation parameters.",
                        initializedParameterCount,
                        parameterCount));
            }

            if (DateTime.UtcNow - parameterStartTime >=
                ParameterInitializationTimeout)
            {
                string parameterName =
                    parameterRegistry.GetParameterName(
                        currentParameterId);

                initializationCancellation.Cancel();

                throw new TimeoutException(
                    $"Initialization of parameter '{parameterName}' " +
                    $"exceeded the maximum allowed time of " +
                    $"{ParameterInitializationTimeout.TotalMinutes} minutes.");
            }

            await Task.Delay(
                MonitoringInterval,
                initializationCancellation.Token);
        }
    }

    private SpatialRepository BuildSpatialRepository(
        Laws laws,
        int gridWidth)
    {
        SpatialRepositoryBuilder spatialRepositoryBuilder =
            new SpatialRepositoryBuilder(
                new SpatialPlanBuilder());

        return spatialRepositoryBuilder.Build(
            laws.GetAll(),
            gridWidth);
    }

    private TaskCoordinator BuildTaskCoordinator(
        EngineGateway engine,
        IGrid<PlanePosition> grid,
        SpatialRepository spatialRepository,
        UserMetricObserver observer,
        IParameterRegistry parameterRegistry,
        Laws laws,
        int cellCount,
        int parameterCount)
    {
        SequentialTraversal sequentialTraversal =
            new SequentialTraversal();

        ObservationDispatcher observationDispatcher =
            new ObservationDispatcher();

        observationDispatcher.Subscribe(observer);

        RequestControllerFactory requestControllerFactory =
            new RequestControllerFactory(
                spatialRepository,
                observationDispatcher,
                grid,
                parameterCount,
                engine.EntryBuffer,
                sequentialTraversal);

        ResultControllerFactory resultControllerFactory =
            new ResultControllerFactory(
                spatialRepository,
                grid,
                engine.ExitBuffer,
                sequentialTraversal);

        TaskCoordinator taskCoordinator =
            new TaskCoordinator(
                requestControllerFactory,
                resultControllerFactory,
                engine.EntryBuffer,
                engine.ExitBuffer,
                parameterRegistry,
                laws,
                cellCount);

        taskCoordinator.SetStartCalculation(
            engine.StartCalculation);

        engine.SetNotifyCalculationCompletedCallback(
            taskCoordinator.NotifyCalculusCompleted);

        return taskCoordinator;
    }

    private GridObserver<PlanePosition> BuildGridObserver(
        IGrid<PlanePosition> grid,
        IEnumerable<ushort> parameterIds,
        int parameterCount)
    {
        GridBufferPool gridBufferPool =
            new GridBufferPool(
                GridBinaryFormat.GetBufferSize(
                    grid.Size,
                    parameterCount));

        ushort[] ids =
            parameterIds.ToArray();

        IReadOnlyList<ushort> positions =
            new List<ushort>(ids);

        return new GridObserver<PlanePosition>(
            grid,
            positions,
            gridBufferPool);
    }
}