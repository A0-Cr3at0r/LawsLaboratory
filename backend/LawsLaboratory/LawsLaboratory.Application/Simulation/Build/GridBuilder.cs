// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / Build
//
// GridBuilder.cs
//
// Builds the initial simulation grid from the provided grid configuration and
// the complete set of laws.
//
// Each law is responsible for initializing one simulation parameter. The
// builder generates the initial parameter values according to the law's value
// distribution and, when configured, its spatial distribution and spatial
// domain.
//
// Spatial distributions produce positions relative to the simulation origin.
// The generated position is first validated against the spatial domain, if
// present, then translated to grid coordinates by applying the simulation
// origin before discretization. Positions outside the grid are ignored.
//
// When no spatial distribution is provided, initialization starts at the
// simulation origin and expands outward through the grid. When a spatial
// domain is provided without a spatial distribution, the same traversal is
// used and only positions belonging to the domain are initialized.
//
// A parameter is considered initialized only after its complete initialization
// succeeds. The builder performs its work synchronously and supports
// cooperative cancellation through the provided CancellationToken. Timeout
// policy and execution strategy belong to the caller.
//
// A GridBuilder instance is intended for a single grid construction.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Simulation.Configuration.RuntimeConfigurationPack;
using LawsLaboratory.Core.Laws;
using LawsLaboratory.Core.Mathematics.Discretizer;
using LawsLaboratory.Core.Mathematics.Distributions;
using LawsLaboratory.Core.Mathematics.Domain;
using LawsLaboratory.Core.SpatialModel.Grid;
using LawsLaboratory.Core.SpatialModel.Position;
using LawsLaboratory.Core.Value;
using System.Numerics;

namespace LawsLaboratory.Application.Simulation.Build;

internal sealed class GridBuilder
{
    private readonly Vector2RoundDiscretizer _discretizer =
        new Vector2RoundDiscretizer();

    public int InitializedParameterCount { get; private set; }

    public PlaneGrid Build(
        GridConfiguration gridConfiguration,
        Law[] laws,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(gridConfiguration);
        ArgumentNullException.ThrowIfNull(laws);

        ValidateGridConfiguration(gridConfiguration);

        PlaneGrid grid = new PlaneGrid(
            gridConfiguration.Width,
            gridConfiguration.Height,
            laws.Length);

        ValidateLaws(laws, grid.Size);

        Vector2 origin = new(
            gridConfiguration.Width / 2f,
            gridConfiguration.Height / 2f);

        foreach (Law law in laws)
        {
            cancellationToken.ThrowIfCancellationRequested();

            InitializeParameter(
                law,
                grid,
                origin,
                gridConfiguration.Width,
                gridConfiguration.Height,
                cancellationToken);

            InitializedParameterCount++;
        }

        return grid;
    }


    private  void InitializeParameter(
        Law law,
        PlaneGrid grid,
        Vector2 origin,
        int width,
        int height,
        CancellationToken cancellationToken)
    {
        IDistribution<double> valueDistribution =
            law.GetInitializationValueDistribution();

        IDistribution<Vector2>? spaceDistribution =
            law.GetInitializationSpaceDistribution();

        IDomain<Vector2>? spaceDomain =
            law.GetInitializationSpaceDomain();

        if (spaceDistribution is not null)
        {
            InitializeFromDistribution(
                law.TargetParameterId,
                law.GetTargetCellCount(),
                grid,
                valueDistribution,
                spaceDistribution,
                spaceDomain,
                origin,
                width,
                height,
                cancellationToken);

            return;
        }

        InitializeFromOrigin(
            law.TargetParameterId,
            law.GetTargetCellCount(),
            grid,
            valueDistribution,
            spaceDomain,
            origin,
            width,
            height,
            cancellationToken);
    }


    /// <summary>
    /// Traverses the grid in expanding square rings centered on the simulation
    /// origin. Each ring is visited once, starting with its top edge and then
    /// proceeding clockwise around the remaining edges.
    ///
    /// When a spatial domain is provided, every visited grid position is tested
    /// against that domain before its cell is initialized. The domain is
    /// evaluated using grid-relative coordinates.
    /// </summary>
    private static void InitializeFromOrigin(
        ushort parameterId,
        int targetCellCount,
        PlaneGrid grid,
        IDistribution<double> valueDistribution,
        IDomain<Vector2>? spaceDomain,
        Vector2 origin,
        int width,
        int height,
        CancellationToken cancellationToken)
    {
        int initializedCount = 0;

        int originX = (int)Math.Round(origin.X);
        int originY = (int)Math.Round(origin.Y);

        int maxRadius = Math.Max(
            Math.Max(originX, width - 1 - originX),
            Math.Max(originY, height - 1 - originY));

        for (int radius = 0;
             radius <= maxRadius && initializedCount < targetCellCount;
             radius++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            int minX = Math.Max(0, originX - radius);
            int maxX = Math.Min(width - 1, originX + radius);
            int minY = Math.Max(0, originY - radius);
            int maxY = Math.Min(height - 1, originY + radius);

            for (int x = minX;
                 x <= maxX && initializedCount < targetCellCount;
                 x++)
            {
                if (TryInitializeCell(
                        x,
                        minY,
                        parameterId,
                        grid,
                        valueDistribution,
                        spaceDomain,
                        width))
                {
                    initializedCount++;
                }
            }

            for (int y = minY + 1;
                 y <= maxY && initializedCount < targetCellCount;
                 y++)
            {
                if (TryInitializeCell(
                        maxX,
                        y,
                        parameterId,
                        grid,
                        valueDistribution,
                        spaceDomain,
                        width))
                {
                    initializedCount++;
                }
            }

            if (maxY > minY)
            {
                for (int x = maxX - 1;
                     x >= minX && initializedCount < targetCellCount;
                     x--)
                {
                    if (TryInitializeCell(
                            x,
                            maxY,
                            parameterId,
                            grid,
                            valueDistribution,
                            spaceDomain,
                            width))
                    {
                        initializedCount++;
                    }
                }
            }

            if (maxX > minX)
            {
                for (int y = maxY - 1;
                     y > minY && initializedCount < targetCellCount;
                     y--)
                {
                    if (TryInitializeCell(
                            minX,
                            y,
                            parameterId,
                            grid,
                            valueDistribution,
                            spaceDomain,
                            width))
                    {
                        initializedCount++;
                    }
                }
            }
        }

        EnsureTargetReached(
            initializedCount,
            targetCellCount,
            parameterId);
    }


    /// <summary>
    /// Repeatedly samples positions from the spatial distribution until the
    /// requested number of cells has been initialized.
    ///
    /// Each generated position is relative to the simulation origin. The
    /// position is first tested against the optional spatial domain in its
    /// relative coordinate system. Only after passing that test is the origin
    /// added and the resulting absolute position discretized to a grid cell.
    ///
    /// Samples outside the grid, or samples that map to an already initialized
    /// cell, are discarded and do not contribute to the target count.
    /// </summary>
    private  void InitializeFromDistribution(
        ushort parameterId,
        int targetCellCount,
        PlaneGrid grid,
        IDistribution<double> valueDistribution,
        IDistribution<Vector2> spaceDistribution,
        IDomain<Vector2>? spaceDomain,
        Vector2 origin,
        int width,
        int height,
        CancellationToken cancellationToken)
    {
        int initializedCount = 0;

        while (initializedCount < targetCellCount)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Vector2 sampledPosition =
                spaceDistribution.Generate();

            if (spaceDomain is not null &&
                !spaceDomain.Contains(sampledPosition))
            {
                continue;
            }

            Vector2 absolutePosition =
                sampledPosition + origin;

            PlanePosition position =
                _discretizer.Discretize(absolutePosition);

            if (!TryGetCellId(
                    position,
                    width,
                    height,
                    out int cellId))
            {
                continue;
            }

            if (TryInitializeCell(
                    cellId,
                    parameterId,
                    grid,
                    valueDistribution))
            {
                initializedCount++;
            }
        }
    }


    private static bool TryInitializeCell(
        int x,
        int y,
        ushort parameterId,
        PlaneGrid grid,
        IDistribution<double> valueDistribution,
        IDomain<Vector2>? spaceDomain,
        int width)
    {
        Vector2 position = new(x, y);

        if (spaceDomain is not null &&
            !spaceDomain.Contains(position))
        {
            return false;
        }

        int cellId = y * width + x;

        return TryInitializeCell(
            cellId,
            parameterId,
            grid,
            valueDistribution);
    }


    private static bool TryInitializeCell(
        int cellId,
        ushort parameterId,
        PlaneGrid grid,
        IDistribution<double> valueDistribution)
    {
        if (grid.GetParameterValue(
                cellId,
                parameterId) is not Dead)
        {
            return false;
        }

        grid.SetCellParameterValue(
            cellId,
            parameterId,
            valueDistribution.Generate());

        return true;
    }


    private static bool TryGetCellId(
        PlanePosition position,
        int width,
        int height,
        out int cellId)
    {
        if ((uint)position.X >= (uint)width ||
            (uint)position.Y >= (uint)height)
        {
            cellId = 0;
            return false;
        }

        cellId =
            position.Y * width +
            position.X;

        return true;
    }


    private static void EnsureTargetReached(
        int initializedCount,
        int targetCellCount,
        ushort parameterId)
    {
        if (initializedCount == targetCellCount)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Unable to initialize the requested {targetCellCount} cells " +
            $"for parameter {parameterId}. " +
            $"Only {initializedCount} cells could be initialized.");
    }


    private static void ValidateGridConfiguration(
        GridConfiguration configuration)
    {
        if (configuration.Width <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(configuration.Width));
        }

        if (configuration.Height <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(configuration.Height));
        }
    }


    private static void ValidateLaws(
        Law[] laws,
        int gridSize)
    {
        if (laws.Length == 0)
        {
            throw new ArgumentException(
                "At least one law must be provided.",
                nameof(laws));
        }

        bool[] initializedParameters =
            new bool[laws.Length];

        foreach (Law law in laws)
        {
            ushort parameterId = law.TargetParameterId;

            if (parameterId >= laws.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(laws),
                    $"Parameter ID {parameterId} does not correspond " +
                    $"to a registered parameter.");
            }

            if (initializedParameters[parameterId])
            {
                throw new InvalidOperationException(
                    $"Parameter {parameterId} is targeted by more than one law.");
            }

            initializedParameters[parameterId] = true;

            int targetCellCount = law.GetTargetCellCount();

            if (targetCellCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(laws),
                    $"Parameter {parameterId} has a negative target cell count.");
            }

            if (targetCellCount > gridSize)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(laws),
                    $"Parameter {parameterId} requests more cells " +
                    $"than the grid contains.");
            }
        }
    }
}
