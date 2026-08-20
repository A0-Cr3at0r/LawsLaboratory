// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Build / Factories
//
// RequestControllerFactory.cs
//
// Creates the RequestControllers used by the request stage of the simulation
// pipeline.
//
// The factory determines the number of controllers according to the number of
// cells and partitions the grid traversal range among them. Each controller
// receives its execution range together with the shared infrastructure required
// to produce execution requests.
//
// Controller construction is kept separate from TaskCoordinator so that the
// coordinator remains responsible for execution orchestration rather than the
// details of controller construction.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Execution.EngineGateway.Entry;
using LawsLaboratory.Application.Simulation.Observer;
using LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Application.Execution.ExecutionRequestStage;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;


namespace LawsLaboratory.Application.Simulation.Build.Factories;

internal sealed class RequestControllerFactory
{
    private readonly SpatialRepository _spatialRepository;
    private readonly ObservationDispatcher _observerDispatcher;
    private readonly IGrid<PlanePosition> _grid;
    private readonly int _maxVariableCount;
    private readonly GatewayEntryBuffer _gatewayBuffer;
    private readonly ITraversalStrategy<int> _traversalStrategy;

    public RequestControllerFactory(
        SpatialRepository spatialRepository,
        ObservationDispatcher observerDispatcher,
        IGrid<PlanePosition> grid,
        int maxVariableCount,
        GatewayEntryBuffer gatewayBuffer,
        ITraversalStrategy<int> traversalStrategy)
    {
        _spatialRepository = spatialRepository;
        _observerDispatcher = observerDispatcher;
        _grid = grid;
        _maxVariableCount = maxVariableCount;
        _gatewayBuffer = gatewayBuffer;
        _traversalStrategy = traversalStrategy;
    }

    public IReadOnlyList<RequestController> CreateControllers(int cellCount)
    {
        int controllerCount = ComputeControllerCount(cellCount);

        List<RequestController> controllers = new(controllerCount);

        int boxSize = cellCount / controllerCount;
        int remainder = cellCount % controllerCount;

        int beginAt = 0;

        for (int i = 0; i < controllerCount; i++)
        {
            int currentBox = boxSize;

            if (i == controllerCount - 1)
                currentBox += remainder;

            controllers.Add(
                new RequestController(
                    beginAt,
                    currentBox,
                    i,
                    _spatialRepository,
                    _observerDispatcher,
                    _grid,
                    _maxVariableCount,
                    _gatewayBuffer,
                    _traversalStrategy));

            beginAt += currentBox;
        }

        return controllers;
    }

    private static int ComputeControllerCount(int cellCount)
    {
        if (cellCount <= 10_000)
            return 1;

        if (cellCount >= 500_000)
            return Environment.ProcessorCount;

        double ratio =
            (double)(cellCount - 10_000) /
            (500_000 - 10_000);

        return 1 + (int)Math.Round(ratio * (Environment.ProcessorCount - 1));
    }
}