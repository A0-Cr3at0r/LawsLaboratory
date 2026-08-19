using LawsLaboratory.Application.Execution.EngineGateway.Exit;
using LawsLaboratory.Application.Execution.EngineGateway.Entry;
using LawsLaboratory.Application.Execution.ExecutionRequestStage;
using LawsLaboratory.Application.Simulation.Build.Factories;
using LawsLaboratory.Application.Simulation.Observer;
using LawsLaboratory.Application.Simulation.SpatialManagement.Traversal;
using LawsLaboratory.Application.Simulation.TaskCoordinator;
using LawsLaboratory.Core.Formula;
using LawsLaboratory.Core.Formula.Element;
using LawsLaboratory.Core.Laws;
using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;
using LawsLaboratory.Application.Simulation.Build.SpatialBuild;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.LawsRepository;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Parameter;
using LawsLaboratory.Application.Simulation.EnvironnementRepository.Spatial;


namespace LawsLaboratory.Tests.TaskCoordinatorTest;

public class TaskCoordinatorTest
{
    [Fact]
    public async Task Variation_ExecutesCompletePipeline()
    {
        // Arrange

        const int width = 4;
        const int height = 4;
        const int cellCount = width * height;

        PlaneGrid grid =
            new PlaneGrid(
                width,
                height,
                1);

        for (int cellId = 0; cellId < cellCount; cellId++)
        {
            grid.SetCellParameterValue(
                cellId,
                0,
                0.0);
        }

        ParameterRegistry parameterRegistry =
            CreateParameterRegistry();

        Laws laws =
            CreateLaws();

        SpatialRepository spatialRepository =
            CreateSpatialRepository(
                laws,
                width);

        ObservationDispatcher observerDispatcher =
            new ObservationDispatcher();

        CompiledExpression expression =
            laws
                .GetLaw(0)
                .GetVariationExpression();

        GatewayEntryBuffer gatewayEntryBuffer =
            new GatewayEntryBuffer(
                maxPackets: cellCount,
                maxValueCount: 1,
                maxBoxUsable: Environment.ProcessorCount,
                program: expression.Program);

        GatewayExitBuffer gatewayExitBuffer =
            new GatewayExitBuffer(cellCount);

        SequentialTraversal traversal =
            new SequentialTraversal();


        RequestControllerFactory requestControllerFactory =
            new RequestControllerFactory(
                spatialRepository,
                observerDispatcher,
                grid,
                maxVariableCount: 1,
                gatewayEntryBuffer,
                traversal);


        ResultControllerFactory resultControllerFactory =
            new ResultControllerFactory(
                spatialRepository,
                grid,
                gatewayExitBuffer,
                traversal);


        TaskCoordinator taskCoordinator =
            new TaskCoordinator(
                requestControllerFactory,
                resultControllerFactory,
                gatewayEntryBuffer,
                gatewayExitBuffer,
                parameterRegistry,
                laws,
                cellCount);


        // Act

        // Faux Gateway
        List<TaskCoordinatorState> states = [];

        taskCoordinator.SetStartCalculation(
            () =>
            {
                states.Add(taskCoordinator.State);

                ExecuteVariationGateway(
                    gatewayEntryBuffer,
                    gatewayExitBuffer,
                    taskCoordinator,
                    cellCount);
            });

        await taskCoordinator.StartVariation();


        // Assert

        // TaskCoordinator
        Assert.Equal(
            TaskCoordinatorState.WaitingVariationCalculation,
            states[0]);

        Assert.Equal(
            TaskCoordinatorState.Completed,
            taskCoordinator.State);


        // GatewayExit
        Assert.Equal(
            cellCount,
            gatewayExitBuffer.ResultReceived);


        // Grid
        for (int cellId = 0; cellId < cellCount; cellId++)
        {
            ScalarValue value =
                Assert.IsType<ScalarValue>(
                    grid.GetParameterValue(cellId, 0));

            Assert.Equal(
                1.0,
                value.Get());
        }
    }

    





    [Fact]
    public async Task Transmission_ExecutesCompletePipeline()
    {
        // Arrange

        const int width = 137;
        const int height = 73;
        const int cellCount = width * height;

        PlaneGrid grid =
            new PlaneGrid(
                width,
                height,
                1);

        for (int cellId = 0; cellId < cellCount/2; cellId++)
        {
            grid.SetCellParameterValue(
                cellId,
                0,
                3.0);
        }

        for (int cellId = cellCount/2; cellId < cellCount; cellId++)
        {
            grid.SetCellParameterValue(
                cellId,
                0,
                4.0);
        }



        ParameterRegistry parameterRegistry =
            CreateParameterRegistry();

        Laws laws =
            CreateLaws();

        SpatialRepository spatialRepository =
            CreateSpatialRepository(
                laws,
                width);

        ObservationDispatcher observerDispatcher =
            new ObservationDispatcher();

        CompiledExpression expression =
            laws
                .GetLaw(0)
                .GetTransmissionExpression();

        GatewayEntryBuffer gatewayEntryBuffer =
            new GatewayEntryBuffer(
                maxPackets: cellCount,
                maxValueCount: 1,
                maxBoxUsable: Environment.ProcessorCount,
                program: expression.Program);

        GatewayExitBuffer gatewayExitBuffer =
            new GatewayExitBuffer(cellCount);

        SequentialTraversal traversal =
            new SequentialTraversal();


        RequestControllerFactory requestControllerFactory =
            new RequestControllerFactory(
                spatialRepository,
                observerDispatcher,
                grid,
                maxVariableCount: 1,
                gatewayEntryBuffer,
                traversal);


        ResultControllerFactory resultControllerFactory =
            new ResultControllerFactory(
                spatialRepository,
                grid,
                gatewayExitBuffer,
                traversal);


        TaskCoordinator taskCoordinator =
            new TaskCoordinator(
                requestControllerFactory,
                resultControllerFactory,
                gatewayEntryBuffer,
                gatewayExitBuffer,
                parameterRegistry,
                laws,
                cellCount);


        // Act
        List<TaskCoordinatorState> states = [];

        taskCoordinator.SetStartCalculation(
            () =>
            {
                states.Add(taskCoordinator.State);

                ExecuteTransmissionGateway(
                    gatewayEntryBuffer,
                    gatewayExitBuffer,
                    taskCoordinator,
                    cellCount);
            });

        await taskCoordinator.StartTransmission();


        // Assert

        // TaskCoordinator
        Assert.Equal(
            [TaskCoordinatorState.WaitingTransmissionCalculation],
            states);

        Assert.Equal(
            TaskCoordinatorState.Completed,
            taskCoordinator.State);


        // GatewayExit
        Assert.Equal(
            cellCount,
            gatewayExitBuffer.ResultReceived);


        //Grid
        for (int cellId = 0; cellId < cellCount; cellId++)
        {
            double expected =
                cellId == 0 || cellId > cellCount / 2
                    ? 8.0
                    : 6.0;

            ScalarValue value =
                Assert.IsType<ScalarValue>(
                    grid.GetParameterValue(cellId, 0));

            Assert.Equal(
                expected,
                value.Get());
        }

    }





    private static ParameterRegistry CreateParameterRegistry()
    {
        return new ParameterRegistry(
            ["p"]);
    }









    private static Laws CreateLaws()
    {
        CompiledExpression variationExpression =
            new CompiledExpression(
            [
                new VariableElement(
                    0,
                    new PlanePosition(0, 0)),

                new ConstantElement(1),

                new OperatorElement(
                    OperatorType.Add)
            ]);


        CompiledExpression transmissionExpression =
            new CompiledExpression(
            [
                new ConstantElement(2),

                new VariableElement(
                    0,
                    new PlanePosition(0, 0)),

                new OperatorElement(
                    OperatorType.Multiply)
            ]);


        VariationRule variationRule =
            new VariationRule(
                "p + 1",
                variationExpression);


        TransmissionRule transmissionRule =
            new TransmissionRule(
                "p[1,0] <= 2 * p",
                transmissionExpression,
                [
                    new PlanePosition(1, 0)
                ]);


        InitializationRule initializationRule =
            new InitializationRule(
                (IDistribution<double>)null!);


        Law law =
            new Law(
                targetParameterId: 0,
                variationRule,
                transmissionRule,
                initializationRule);


        return new Laws(
        [
            law
        ]);
    }








    private static SpatialRepository CreateSpatialRepository(
        Laws laws,
        int gridWidth)
    {
        SpatialPlanBuilder planBuilder =
            new SpatialPlanBuilder();


        SpatialRepositoryBuilder repositoryBuilder =
            new SpatialRepositoryBuilder(
                planBuilder);


        return repositoryBuilder.Build(
            laws.GetAll(),
            gridWidth);
    }







    private static void ExecuteTransmissionGateway(
    GatewayEntryBuffer entry,
    GatewayExitBuffer exit,
    TaskCoordinator coordinator,
    int cellCount)
    {
        int controllerCount =
            ComputeControllerCount(entry.Packets.Length);

        int boxSize =
            entry.Packets.Length / controllerCount;

        int remainder =
            entry.Packets.Length % controllerCount;

        int resultCount = 0;
        int boxBegin = 0;

        for (int box = 0; box < controllerCount; box++)
        {
            int currentBoxSize = boxSize;

            if (box == controllerCount - 1)
                currentBoxSize += remainder;

            int limit = entry.BoxLimite[box];

            for (int i = 0; i < limit; i++)
            {
                RequestPacket packet =
                    entry.Packets[boxBegin + i];


                double value =
                    packet.Values[0]!.Value;

                exit.Results[resultCount++] =
                    new GatewayResult
                    {
                        Id = packet.CellId,
                        Value = new SerializedValue(
                            ValueKind.Scalar,
                            [1, 1],
                            [2 * value])
                    };
            }

            boxBegin += currentBoxSize;
        }

        exit.ResultReceived = resultCount;

        coordinator.NotifyCalculusCompleted();
    }




    private static void ExecuteVariationGateway(
    GatewayEntryBuffer entry,
    GatewayExitBuffer exit,
    TaskCoordinator coordinator,
    int cellCount)
    {
        int controllerCount =
            ComputeControllerCount(cellCount);

        int boxSize =
            cellCount / controllerCount;

        int remainder =
            cellCount % controllerCount;

        int resultCount = 0;
        int boxBegin = 0;

        for (int box = 0; box < controllerCount; box++)
        {
            int currentBoxSize = boxSize;

            if (box == controllerCount - 1)
                currentBoxSize += remainder;

            int limit = entry.BoxLimite[box];

            for (int i = 0; i < limit; i++)
            {
                RequestPacket packet =
                    entry.Packets[boxBegin + i];

                double value =
                    packet.Values[0]!.Value;

                exit.Results[resultCount++] =
                    new GatewayResult
                    {
                        Id = packet.CellId,
                        Value = new SerializedValue(
                            ValueKind.Scalar,
                            [1, 1],
                            [value + 1])
                    };
            }

            boxBegin += currentBoxSize;
        }

        exit.ResultReceived = resultCount;

        coordinator.NotifyCalculusCompleted();
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