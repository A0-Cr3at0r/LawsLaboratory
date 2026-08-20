// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Build / Factories
//
// ResultControllerFactory.cs
//
// Creates and partitions the ResultControllers used by the result stage of
// the simulation pipeline.
//
// The factory determines the number of controllers according to the number of
// cells and distributes the valid result range among them. Each controller
// receives a contiguous portion of the result range.
//
// ResultControllerFactory is an infrastructure component used by the
// TaskCoordinator during pipeline execution. It is not part of the declarative
// simulation configuration and is therefore not involved in configuration
// deserialization or persistence.
//
// Controller construction is kept here so that the TaskCoordinator remains
// responsible for orchestration rather than the details of controller setup.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Execution.EngineGateway.Exit;
using LawsLaboratory.Application.Execution.ExecutionResultStage;
using LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;


namespace LawsLaboratory.Application.Simulation.Build.Factories;

internal sealed class ResultControllerFactory
{
    private readonly SpatialRepository _spatialRepository;
    private readonly IGrid<PlanePosition> _grid;
    private readonly GatewayExitBuffer _gatewayExit;
    private readonly ITraversalStrategy<int> _traversalStrategy;

    public ResultControllerFactory(
        SpatialRepository spatialRepository,
        IGrid<PlanePosition> grid,
        GatewayExitBuffer gatewayExit,
        ITraversalStrategy<int> traversalStrategy)
    {
        _spatialRepository = spatialRepository;
        _grid = grid;
        _gatewayExit = gatewayExit;
        _traversalStrategy = traversalStrategy;
    }

    public IReadOnlyList<ResultController> CreateControllers(int cellCount)
    {
        int controllerCount = ComputeControllerCount(cellCount);

        List<ResultController> controllers = new(controllerCount);

        int boxSize = cellCount / controllerCount;
        int remainder = cellCount % controllerCount;

        int beginAt = 0;

        for (int i = 0; i < controllerCount; i++)
        {
            int currentBox = boxSize;

            if (i == controllerCount - 1)
                currentBox += remainder;

            controllers.Add(
                new ResultController(
                    beginAt,
                    currentBox,
                    _spatialRepository,
                    _grid,
                    _gatewayExit,
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