// -----------------------------------------------------------------------------
// LawsLaboratory
// Tests / Application / Simulation / SimulationTools
//
// InitializerTests.cs
//
// Verifies the orchestration performed by Initializer during simulation
// construction.
//
// These tests focus on the initialization workflow as a whole rather than on
// the internal behavior of individual builders, distributions or domains.
// They verify successful runtime construction, invalid input handling,
// initialization failures and external cancellation.
//
// Timeout behavior is intentionally excluded for now because the production
// timeout is five minutes. It will be tested separately once the other
// initialization behaviors are validated.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.Configuration;
using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;
using LawsLaboratory.Application.Simulation.Logs;
using LawsLaboratory.Application.Simulation.SimulationTools;

namespace LawsLaboratory.Tests.Application.Simulation.SimulationTools;

public sealed class InitializerTests
{
    [Fact]
    public async Task LaunchInitializationAsync_WithValidConfiguration_BuildsCompleteSimulationRuntime()
    {
        Initializer initializer =
            new Initializer();

        SimulationConfiguration configuration =
            CreateConfiguration(
                width: 10,
                height: 10,
                parameterCount: 1,
                targetCellCount: 100);

        List<workflowSimulationLog> logs = [];

        SimulationRuntime runtime =
            await initializer.LaunchInitializationAsync(
                configuration,
                CancellationToken.None,
                log =>
                {
                    logs.Add(log);
                    return Task.CompletedTask;
                });

        Assert.NotNull(runtime);

        Assert.Contains(
            logs,
            log => log.State == SimulationState.Initiating);

        Assert.Contains(
            logs,
            log => log.State == SimulationState.Initiated);
    }


    [Fact]
    public async Task LaunchInitializationAsync_WithNullConfiguration_ThrowsArgumentNullException()
    {
        Initializer initializer =
            new Initializer();

        Func<workflowSimulationLog, Task> sendLog =
            _ => Task.CompletedTask;

        await Assert.ThrowsAsync<ArgumentNullException>(
            () =>
                initializer.LaunchInitializationAsync(
                    null!,
                    CancellationToken.None,
                    sendLog));
    }


    [Fact]
    public async Task LaunchInitializationAsync_WithNullLogCallback_ThrowsArgumentNullException()
    {
        Initializer initializer =
            new Initializer();

        SimulationConfiguration configuration =
            CreateConfiguration(
                width: 10,
                height: 10,
                parameterCount: 1,
                targetCellCount: 100);

        await Assert.ThrowsAsync<ArgumentNullException>(
            () =>
                initializer.LaunchInitializationAsync(
                    configuration,
                    CancellationToken.None,
                    null!));
    }


    [Fact]
    public async Task LaunchInitializationAsync_WhenInitializationFails_ReportsFailureAndRethrowsException()
    {
        Initializer initializer =
            new Initializer();

        SimulationConfiguration configuration =
            CreateInvalidDistributionConfiguration();

        List<workflowSimulationLog> logs = [];

        ArgumentOutOfRangeException exception =
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                () =>
                    initializer.LaunchInitializationAsync(
                        configuration,
                        CancellationToken.None,
                        log =>
                        {
                            logs.Add(log);
                            return Task.CompletedTask;
                        }));

        Assert.Contains(
            logs,
            log =>
                log.State == SimulationState.InitializationFailed &&
                log.Message.Contains(
                    exception.Message,
                    StringComparison.Ordinal));
    }


    [Fact]
    public async Task LaunchInitializationAsync_WhenExternallyCancelled_StopsInitialization()
    {
        Initializer initializer =
            new Initializer();

        SimulationConfiguration configuration =
            CreateConfiguration(
                width: 2000,
                height: 2000,
                parameterCount: 4,
                targetCellCount: 4_000_000);

        using CancellationTokenSource cancellation =
            new CancellationTokenSource();


        TaskCompletionSource<bool> gridConstructionStarted =
            new TaskCompletionSource<bool>(
                TaskCreationOptions.RunContinuationsAsynchronously);

        Task<SimulationRuntime> initializationTask =
            initializer.LaunchInitializationAsync(
                configuration,
                cancellation.Token,
                log =>
                {
                    if (log.Message == "Starting grid construction.")
                    {
                        gridConstructionStarted.TrySetResult(true);
                    }

                    return Task.CompletedTask;
                });

        await gridConstructionStarted.Task;

        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => initializationTask);
    }




    private static SimulationConfiguration CreateConfiguration(
        int width,
        int height,
        int parameterCount,
        int targetCellCount)
    {
        Dictionary<string, LawConfiguration> parametersLaws =
            new Dictionary<string, LawConfiguration>();

        for (int i = 0; i < parameterCount; i++)
        {
            string parameterName =
                $"parameter{i}";

            parametersLaws[parameterName] =
                new LawConfiguration
                {
                    VariationFormula =
                        $"{parameterName} + 1",

                    TransmissionFormula =
                        $"{parameterName}[0,0]",

                    InitializationConfiguration =
                        new InitializationConfiguration
                        {
                            InitialValueDistribution =
                                new ConstantDistributionConfiguration
                                {
                                    Value = 1
                                },

                            TargetCellCount =
                                targetCellCount
                        },

                    TransmissionDestinations =
                        []
                };
        }

        return new SimulationConfiguration
        {
            Model =
                new ModelConfiguration
                {
                    ParametersLaws =
                        parametersLaws
                },

            Runtime =
                new RuntimeConfiguration
                {
                    Grid =
                        new GridConfiguration
                        {
                            Width = width,
                            Height = height
                        },

                    Time =
                        new TimeConfiguration()
                }
        };
    }


    private static SimulationConfiguration CreateInvalidDistributionConfiguration()
    {
        return new SimulationConfiguration
        {
            Model =
                new ModelConfiguration
                {
                    ParametersLaws =
                        new Dictionary<string, LawConfiguration>
                        {
                            ["temperature"] =
                                new LawConfiguration
                                {
                                    VariationFormula =
                                        "temperature + 1",

                                    TransmissionFormula =
                                        "temperature[0,0]",

                                    InitializationConfiguration =
                                        new InitializationConfiguration
                                        {
                                            InitialValueDistribution =
                                                new NormalDistributionConfiguration
                                                {
                                                    Mean = 0,
                                                    StandardDeviation = -1
                                                },

                                            TargetCellCount = 100
                                        },

                                    TransmissionDestinations =
                                        []
                                }
                        }
                },

            Runtime =
                new RuntimeConfiguration
                {
                    Grid =
                        new GridConfiguration
                        {
                            Width = 10,
                            Height = 10
                        },

                    Time =
                        new TimeConfiguration()
                }
        };
    }

    // -----------------------------------------------------------------------------
    // Timeout test
    // -----------------------------------------------------------------------------
    //
    // This test is intentionally disabled for now because the production
    // initialization timeout is five minutes.
    //
    // The configuration deliberately requests more cells than the configured
    // spatial domain can contain. Initialization therefore cannot reach its
    // target cell count and eventually exceeds the per-parameter timeout.
    //
    //[Fact]
    //public async Task LaunchInitializationAsync_WhenParameterInitializationTimesOut_ReportsFailureAndRethrowsTimeoutException()
    //{
    //    Initializer initializer =
    //        new Initializer();

    //    SimulationConfiguration configuration =
    //        CreateTimeoutConfiguration();

    //    List<workflowSimulationLog> logs = [];

    //    TimeoutException exception =
    //        await Assert.ThrowsAsync<TimeoutException>(
    //            () =>
    //                initializer.LaunchInitializationAsync(
    //                    configuration,
    //                    CancellationToken.None,
    //                    log =>
    //                {
    //                    logs.Add(log);
    //                    return Task.CompletedTask;
    //                }));

    //    Assert.Contains(
    //        logs,
    //        log =>
    //            log.State == SimulationState.InitializationFailed &&
    //            log.Message.Contains(
    //                exception.Message,
    //                StringComparison.Ordinal));
    //}


    //private static SimulationConfiguration CreateTimeoutConfiguration()
    //{
    //    return new SimulationConfiguration
    //    {
    //        Model =
    //            new ModelConfiguration
    //            {
    //                ParametersLaws =
    //                    new Dictionary<string, LawConfiguration>
    //                    {
    //                        ["temperature"] =
    //                            new LawConfiguration
    //                            {
    //                                VariationFormula =
    //                                    "temperature + 1",

    //                                TransmissionFormula =
    //                                    "temperature[0,0]",

    //                                InitializationConfiguration =
    //                                    new InitializationConfiguration
    //                                    {
    //                                        InitialValueDistribution =
    //                                            new ConstantDistributionConfiguration
    //                                            {
    //                                                Value = 1
    //                                            },

    //                                        InitialSpatialDistribution =
    //                                            new IndependentAxisDistributionConfiguration
    //                                            {
    //                                                X =
    //                                                    new ConstantDistributionConfiguration
    //                                                    {
    //                                                        Value = 0
    //                                                    },

    //                                                Y =
    //                                                    new ConstantDistributionConfiguration
    //                                                    {
    //                                                        Value = 0
    //                                                    }
    //                                            },

    //                                        DomainConfiguration =
    //                                            new BoxDomainConfiguration
    //                                            {
    //                                                MinimumX = 0,
    //                                                MaximumX = 0,
    //                                                MinimumY = 0,
    //                                                MaximumY = 0
    //                                            },

    //                                        TargetCellCount = 2
    //                                    },

    //                                TransmissionDestinations =
    //                                    []
    //                            }
    //                    }
    //            },

    //        Runtime =
    //            new RuntimeConfiguration
    //            {
    //                Grid =
    //                    new GridConfiguration
    //                    {
    //                        Width = 10,
    //                        Height = 10
    //                    },

    //                Time =
    //                    new TimeConfiguration()
    //            }
    //    };
    //}
}