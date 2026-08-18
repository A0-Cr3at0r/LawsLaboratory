namespace LawsLaboratory.Application.Execution.EngineGateway;

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