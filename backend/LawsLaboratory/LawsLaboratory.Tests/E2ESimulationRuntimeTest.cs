// -----------------------------------------------------------------------------
// LawsLaboratory
// Tests / Application / Simulation / SimulationTools
//
// InitializerSimulationTests.cs
//
// Verifies the complete construction and execution of a simulation through the
// Initializer.
//
// The test uses five interacting parameters:
//
// A:
//     A <- A + 1
//     A[0,1] <- A
//     initialized everywhere to 0
//
// B:
//     B <- A + B
//     B[0,1] <- B
//     initialized everywhere to 1
//
// C:
//     C <- e
//     C[(1,0),(-1,0),(0,1),(0,-1)] <- C
//     initialized in four cells around the origin
//
// D:
//     D <- D
//     D[0,0] <- C[0,0]
//     initialized in four cells around the origin
//
// E:
//     E <- E
//     E[0,0] <- C[0,0]
//     initialized nowhere
//
//The simulation executes 100 cycles with a 10 ms delay between cycles.
//
// Final deterministic grid state:
//     A = 100 everywhere
//     B = 5051 everywhere
//     C = e everywhere
//     D = e everywhere
//     E = NaN everywhere
//
// Metric expectations:
//     A, B, C and D produce 100 valid temporal observations.
//     E produces no valid temporal observations because all of its
//     cells remain Dead / NaN.
//
// The test also verifies that the UserMetricObserver receives complete
// observations for every parameter.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.Configuration;
using LawsLaboratory.Application.Simulation.Configuration.ExecutionConfiguration;
using LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;
using LawsLaboratory.Application.Simulation.GridObservation;
using LawsLaboratory.Application.Simulation.Logs;
using LawsLaboratory.Application.Simulation.Observer.Observer.UserMetrics;
using LawsLaboratory.Application.Simulation.SimulationTools;
using System.Buffers.Binary;

namespace LawsLaboratory.Tests.Application.Simulation.SimulationTools;

public sealed class InitializerSimulationTests
{
    private const int Width = 100;
    private const int Height = 40;

    private const int CellCount = Width * Height;

    private const long CycleCount = 100;

    private const int DelayMs = 10;

    [Fact]
    public async Task LaunchInitializationAndSimulation_WithFiveParameterModel_ProducesExpectedFinalState()
    {
        Initializer initializer =
            new Initializer();

        SimulationConfiguration configuration =
            CreateConfiguration();

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

        await runtime.LaunchSimulation();

        Assert.Equal(
            SimulationState.Completed,
            runtime.SimulationState);

        AssertFinalGridState(
            runtime);

        AssertFinalMetrics(
            runtime);

        Assert.Contains(
            logs,
            log => log.State == SimulationState.Initiating);

        Assert.Contains(
            logs,
            log => log.State == SimulationState.Initiated);

        Assert.DoesNotContain(
            logs,
            log => log.State == SimulationState.InitializationFailed);
    }


    private static SimulationConfiguration CreateConfiguration()
    {
        return new SimulationConfiguration
        {
            Model =
                new ModelConfiguration
                {
                    ParametersLaws =
                        new Dictionary<string, LawConfiguration>
                        {
                            ["A"] =
                                new LawConfiguration
                                {
                                    VariationFormula =
                                        "A + 1",

                                    TransmissionFormula =
                                        "A",

                                    TransmissionDestinations =
                                    [
                                        new PlanePositionConfiguration
                                        {
                                            X = 0,
                                            Y = 1
                                        }
                                    ],

                                    InitializationConfiguration =
                                        new InitializationConfiguration
                                        {
                                            InitialValueDistribution =
                                                new ConstantDistributionConfiguration
                                                {
                                                    Value = 0
                                                },

                                            TargetCellCount =
                                                CellCount
                                        }
                                },

                            ["B"] =
                                new LawConfiguration
                                {
                                    VariationFormula =
                                        "A + B",

                                    TransmissionFormula =
                                        "B",

                                    TransmissionDestinations =
                                    [
                                        new PlanePositionConfiguration
                                        {
                                            X = 0,
                                            Y = 1
                                        }
                                    ],

                                    InitializationConfiguration =
                                        new InitializationConfiguration
                                        {
                                            InitialValueDistribution =
                                                new ConstantDistributionConfiguration
                                                {
                                                    Value = 1
                                                },

                                            TargetCellCount =
                                                CellCount
                                        }
                                },

                            ["C"] =
                                new LawConfiguration
                                {
                                    VariationFormula =
                                        "e",

                                    TransmissionFormula =
                                        "C",

                                    TransmissionDestinations =
                                    [
                                        new PlanePositionConfiguration
                                        {
                                            X = 1,
                                            Y = 0
                                        },

                                        new PlanePositionConfiguration
                                        {
                                            X = -1,
                                            Y = 0
                                        },

                                        new PlanePositionConfiguration
                                        {
                                            X = 0,
                                            Y = 1
                                        },

                                        new PlanePositionConfiguration
                                        {
                                            X = 0,
                                            Y = -1
                                        }
                                    ],

                                    InitializationConfiguration =
                                        new InitializationConfiguration
                                        {
                                            InitialValueDistribution =
                                                new ConstantDistributionConfiguration
                                                {
                                                    Value = 0
                                                },

                                            InitialSpatialDistribution =
                                                new IndependentAxisDistributionConfiguration
                                                {
                                                    X =
                                                        new UniformDistributionConfiguration
                                                        {
                                                            Minimum = -1,
                                                            Maximum = 1
                                                        },

                                                    Y =
                                                        new UniformDistributionConfiguration
                                                        {
                                                            Minimum = -1,
                                                            Maximum = 1
                                                        }
                                                },

                                            DomainConfiguration =
                                                new BoxDomainConfiguration
                                                {
                                                    MinimumX = -1,
                                                    MaximumX = 1,
                                                    MinimumY = -1,
                                                    MaximumY = 1
                                                },

                                            TargetCellCount = 4
                                        }
                                },

                            ["D"] =
                                new LawConfiguration
                                {
                                    VariationFormula =
                                        "D",

                                    TransmissionFormula =
                                        "C[0,0]",

                                    TransmissionDestinations =
                                    [
                                        new PlanePositionConfiguration
                                        {
                                            X = 1,
                                            Y = 0
                                        },

                                        new PlanePositionConfiguration
                                        {
                                            X = -1,
                                            Y = 0
                                        },

                                        new PlanePositionConfiguration
                                        {
                                            X = 0,
                                            Y = 1
                                        },

                                        new PlanePositionConfiguration
                                        {
                                            X = 0,
                                            Y = -1
                                        }
                                    ],

                                    InitializationConfiguration =
                                        new InitializationConfiguration
                                        {
                                            InitialValueDistribution =
                                                new ConstantDistributionConfiguration
                                                {
                                                    Value = 0
                                                },

                                            InitialSpatialDistribution =
                                                new IndependentAxisDistributionConfiguration
                                                {
                                                    X =
                                                        new UniformDistributionConfiguration
                                                        {
                                                            Minimum = -1,
                                                            Maximum = 1
                                                        },

                                                    Y =
                                                        new UniformDistributionConfiguration
                                                        {
                                                            Minimum = -1,
                                                            Maximum = 1
                                                        }
                                                },

                                            DomainConfiguration =
                                                new BoxDomainConfiguration
                                                {
                                                    MinimumX = -1,
                                                    MaximumX = 1,
                                                    MinimumY = -1,
                                                    MaximumY = 1
                                                },

                                            TargetCellCount = 4
                                        }
                                },

                            ["E"] =
                                new LawConfiguration
                                {
                                    VariationFormula =
                                        "E",

                                    TransmissionFormula =
                                        "C[0,0]",

                                    TransmissionDestinations =
                                    [
                                        new PlanePositionConfiguration
                                        {
                                            X = 0,
                                            Y = 0
                                        }
                                    ],

                                    InitializationConfiguration =
                                        new InitializationConfiguration
                                        {
                                            InitialValueDistribution =
                                                new ConstantDistributionConfiguration
                                                {
                                                    Value = 0
                                                },

                                            TargetCellCount = 0
                                        }
                                }
                        }
                },

            Runtime =
                new RuntimeConfiguration
                {
                    Grid =
                        new GridConfiguration
                        {
                            Width = Width,
                            Height = Height
                        },

                    Time =
                        new TimeConfiguration
                        {
                            MaxCycles = CycleCount,
                            DelayMsPerCycle = DelayMs
                        }
                }
        };
    }


    private static void AssertFinalGridState(
        SimulationRuntime runtime)
    {
        using GridBinaryView view =
            runtime.getGridView();

        ReadOnlySpan<byte> data =
            view.Data.Span;

        Assert.Equal(
            GridBinaryFormat.Magic,
            BinaryPrimitives.ReadUInt32LittleEndian(
                data[0..4]));

        Assert.Equal(
            GridBinaryFormat.Version,
            BinaryPrimitives.ReadUInt16LittleEndian(
                data[4..6]));

        Assert.Equal(
            Width,
            BinaryPrimitives.ReadInt32LittleEndian(
                data[6..10]));

        Assert.Equal(
            Height,
            BinaryPrimitives.ReadInt32LittleEndian(
                data[10..14]));

        ushort parameterCount =
            BinaryPrimitives.ReadUInt16LittleEndian(
                data[14..16]);

        Assert.Equal(
            5,
            parameterCount);

        ushort[] parameterIds =
            new ushort[parameterCount];

        int parameterIdOffset =
            GridBinaryFormat.HeaderSize;

        for (int i = 0; i < parameterCount; i++)
        {
            parameterIds[i] =
                BinaryPrimitives.ReadUInt16LittleEndian(
                    data[
            (parameterIdOffset + i * sizeof(ushort))..]);
        }

        Assert.Equal(
            new ushort[] { 0, 1, 2, 3, 4 },
            parameterIds);

        int valuesOffset =
            GridBinaryFormat.HeaderSize +
            parameterCount * sizeof(ushort);

        AssertParameterValues(
            data,
            valuesOffset,
            0,
            100.0);

        AssertParameterValues(
            data,
            valuesOffset,
            1,
            5051.0);


        AssertParameterValues(
            data,
            valuesOffset,
            2,
            Math.E);

        AssertParameterValues(
            data,
            valuesOffset,
            3,
            Math.E);

        AssertAllParameterValuesAreNaN(
            data,
            valuesOffset,
            4);
    }


    private static void AssertParameterValues(
        ReadOnlySpan<byte> data,
        int valuesOffset,
        int parameterIndex,
        double expectedValue)
    {
        int parameterOffset =
            valuesOffset +
            parameterIndex *
            CellCount *
            sizeof(double);

        ReadOnlySpan<byte> parameterData =
            data[
                parameterOffset..(parameterOffset +
                 CellCount * sizeof(double))];

        for (int cellId = 0; cellId < CellCount; cellId++)
        {
            double actualValue =
                BitConverter.Int64BitsToDouble(
                    BinaryPrimitives.ReadInt64LittleEndian(
                       parameterData[
                            (cellId * sizeof(double))..]));

            Assert.True(
                double.IsFinite(actualValue),
                $"Parameter {parameterIndex}, cell {cellId} is not finite.");

            Assert.Equal(
                expectedValue,
                actualValue,
                12);
        }
    }


    private static void AssertFinalMetrics(
        SimulationRuntime runtime)
    {
        UserMetricSnapshot snapshot =
            runtime.GetUserMetric();

        Assert.Equal(
            5,
            snapshot.Parameters.Count);

        AssertParameterMetric(
            snapshot,
            parameterId: 0,
            expectedMean: 100.0);

        AssertParameterMetric(
            snapshot,
            parameterId: 1,
            expectedMean: 5051.0);

        AssertParameterMetric(
            snapshot,
            parameterId: 2,
            expectedMean: Math.E);

        AssertParameterMetric(
            snapshot,
            parameterId: 3,
            expectedMean: Math.E);

        AssertParameterHasNoValidTemporalObservations(
            snapshot,
            parameterId: 4);
    }

    private static void AssertParameterHasNoValidTemporalObservations(
    UserMetricSnapshot snapshot,
    ushort parameterId)
    {
        Assert.True(
            snapshot.Parameters.ContainsKey(parameterId),
            $"Parameter {parameterId} is missing from the metric snapshot.");

        ParameterMetricSnapshot parameter =
            snapshot.Parameters[parameterId];

        Assert.Equal(
            0,
            parameter.Temporal.Count);
    }


    private static void AssertParameterMetric(
      UserMetricSnapshot snapshot,
      ushort parameterId,
      double expectedMean)
    {
        Assert.True(
            snapshot.Parameters.ContainsKey(parameterId),
            $"Parameter {parameterId} is missing from the metric snapshot.");

        ParameterMetricSnapshot parameter =
            snapshot.Parameters[parameterId];

        Assert.Equal(
            CycleCount,
            parameter.Temporal.Count);

        Assert.Equal(
            expectedMean,
            parameter.Temporal.Mean,
            12);

        Assert.Equal(
            expectedMean,
            parameter.Temporal.Minimum,
            12);

        Assert.Equal(
            expectedMean,
            parameter.Temporal.Maximum,
            12);
    }

    private static void AssertAllParameterValuesAreNaN(
    ReadOnlySpan<byte> data,
    int valuesOffset,
    int parameterIndex)
    {
        int parameterOffset =
            valuesOffset +
            parameterIndex *
            CellCount *
            sizeof(double);

        ReadOnlySpan<byte> parameterData =
            data[
                parameterOffset..(parameterOffset + CellCount * sizeof(double))];

        for (int cellId = 0; cellId < CellCount; cellId++)
        {
            double actualValue =
                BitConverter.Int64BitsToDouble(
                    BinaryPrimitives.ReadInt64LittleEndian(
                        parameterData[
                            (cellId * sizeof(double))..]));

            Assert.True(
                double.IsNaN(actualValue),
                $"Parameter {parameterIndex}, cell {cellId} " +
                $"was expected to be NaN but was {actualValue}.");
        }
    }
}