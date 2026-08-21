// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / GridObservation
//
// GridObserver.cs
//
// Captures the current state of a simulation grid into a reusable binary
// representation suitable for network transmission.
// -----------------------------------------------------------------------------

using System.Buffers.Binary;
using System.Runtime.InteropServices;
using LawsLaboratory.Core.SpatialModel.Grid;

namespace LawsLaboratory.Application.Simulation.GridObservation;

internal sealed class GridObserver<TPosition>
    where TPosition : struct
{
    private readonly IGrid<TPosition> _grid;

    private readonly ushort[] _parameterIds;

    private readonly GridBufferPool _bufferPool;

    public GridObserver(
        IGrid<TPosition> grid,
        IReadOnlyList<ushort> parameterIds,
        GridBufferPool bufferPool)
    {
        ArgumentNullException.ThrowIfNull(grid);
        ArgumentNullException.ThrowIfNull(parameterIds);
        ArgumentNullException.ThrowIfNull(bufferPool);

        if (parameterIds.Count == 0)
        {
            throw new ArgumentException(
                "At least one parameter must be selected.",
                nameof(parameterIds));
        }

        _grid = grid;
        _parameterIds = parameterIds.ToArray();
        _bufferPool = bufferPool;
    }

    public GridBinaryView CaptureGrid()
    {
        int cellCount = _grid.Size;

        int parameterCount =
            _parameterIds.Length;

        int parameterIdsSize =
            checked(
                parameterCount *
                sizeof(ushort));

        int valueCount =
            checked(
                cellCount *
                parameterCount);

        int valuesSize =
            checked(
                valueCount *
                sizeof(double));

        int totalSize =
            checked(
                GridBinaryFormat.HeaderSize +
                parameterIdsSize +
                valuesSize);

        byte[] buffer = _bufferPool.Rent();


        try
        {
            WriteHeader(
                buffer,
                parameterCount);

            WriteParameterIds(
                buffer);

            WriteValues(
                buffer);

            return new GridBinaryView(
                _bufferPool,
                buffer,
                totalSize);
        }
        catch
        {
            _bufferPool.Return(buffer);
            throw;
        }
    }

    private void WriteHeader(
        byte[] buffer,
        int parameterCount)
    {
        Span<byte> header =
            buffer.AsSpan(
                0,
                GridBinaryFormat.HeaderSize);

        BinaryPrimitives.WriteUInt32LittleEndian(
            header[0..4],
            GridBinaryFormat.Magic);

        BinaryPrimitives.WriteUInt16LittleEndian(
            header[4..6],
            GridBinaryFormat.Version);

        BinaryPrimitives.WriteInt32LittleEndian(
            header[6..10],
            _grid.Width);

        BinaryPrimitives.WriteInt32LittleEndian(
            header[10..14],
            _grid.Height);

        BinaryPrimitives.WriteUInt16LittleEndian(
            header[14..16],
            checked((ushort)parameterCount));

        BinaryPrimitives.WriteUInt16LittleEndian(
            header[16..18],
            GridBinaryFormat.ElementSize);
    }

    private void WriteParameterIds(
        byte[] buffer)
    {
        int offset =
            GridBinaryFormat.HeaderSize;

        Span<byte> destination =
            buffer.AsSpan(
                offset,
                _parameterIds.Length *
                sizeof(ushort));

        for (int i = 0; i < _parameterIds.Length; i++)
        {
            BinaryPrimitives.WriteUInt16LittleEndian(
                destination[(i * sizeof(ushort))..],
                _parameterIds[i]);
        }
    }

    private void WriteValues(
        byte[] buffer)
    {
        int offset =
            GridBinaryFormat.HeaderSize +
            _parameterIds.Length *
            sizeof(ushort);

        Span<byte> destination =
            buffer.AsSpan(offset);

        int parameterOffset = 0;

        foreach (ushort parameterId in _parameterIds)
        {
            Span<double> parameterValues =
                MemoryMarshal.Cast<byte, double>(
                    destination[parameterOffset..]);

            _grid.CopyParameterValues(
                parameterId,
                parameterValues);

            parameterOffset +=
                _grid.Size *
                sizeof(double);
        }
    }
}