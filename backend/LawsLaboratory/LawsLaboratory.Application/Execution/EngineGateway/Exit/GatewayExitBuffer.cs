// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Execution / EngineGateway / Exit
//
// GatewayExitBuffer.cs
//
// Defines the serialized representation of values crossing the execution
// gateway boundary.
//
// ValueKind identifies the mathematical structure represented by a serialized
// value (scalar, complex, vector, matrix, or tensor).
//
// SerializedValue stores the value kind, its shape, and its numerical data.
// GatewayResult associates such a serialized value with the identifier of
// the request from which the result originated.
//
// These types form the application-side result representation exchanged with
// the execution engine. They are intentionally independent from the concrete
// Core IValue implementations; deserialization into Core values is performed
// by ResultAbsorber.
//
// The current internal execution pipeline only materializes scalar results,
// while the serialized representation is designed to accommodate richer
// mathematical value kinds for the gateway boundary.
//
// The GatewayExitBuffer is a preallocated output buffer receiving results produced by the execution
// engine.
//
// The buffer stores GatewayResult instances in the same request-oriented
// indexing space used by the execution pipeline. ResultReceived records the
// number of results made available for consumption.
//
// It forms the application-side boundary between engine execution and the
// result reception stage.
//
// The buffer is intentionally reusable across executions to limit allocations.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Application.Execution.EngineGateway.Exit;

public enum ValueKind : byte
{
    Scalar,
    Complex,
    Vector,
    Matrix,
    Tensor
}

public sealed record SerializedValue
{
    public ValueKind Kind { get; }

    public int[] Shape;

    public double[] Data;

    public SerializedValue(
        ValueKind kind,
        int[] shape,
        double[] data)
    {
        Kind = kind;
        Shape = shape;
        Data = data;
    }
}


public sealed class GatewayResult
{
    public int Id { get; init; }
    public required SerializedValue Value { get; init; }
}

internal sealed class GatewayExitBuffer
{
    private readonly GatewayResult[] _results;

    public int ResultReceived { get; internal set; }

    public Span<GatewayResult> Results =>
        _results.AsSpan();

    public GatewayExitBuffer(int maxResults)
    {
        _results = new GatewayResult[maxResults];
    }
}