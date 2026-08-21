// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Simulation / GridObservation
//
// GridBinaryFormat.cs
//
// Defines the binary representation used to expose a simulation grid.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Simulation.GridObservation;

internal static class GridBinaryFormat
{
    // ASCII: "LGRD"
    public const uint Magic = 0x4C475244;

    public const ushort Version = 1;

    public const ushort ElementSize = sizeof(double);

    /*
     * Header layout:
     *
     * Offset  Size
     * 0       4       Magic
     * 4       2       Version
     * 6       4       Width
     * 10      4       Height
     * 14      2       ParameterCount
     * 16      2       ElementSize
     *
     * Total: 18 bytes
     */
    public const int HeaderSize = 18;
}