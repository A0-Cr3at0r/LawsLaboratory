namespace LawsLaboratory.Application.Execution.EngineGateway;

public enum ValueKind : byte
{
    Scalar,
    Complex,
    Vector,
    Matrix,
    Tensor
}

public interface ISerializedValue
{
    ValueKind Kind { get; }

    ReadOnlySpan<int> Shape { get; }

    ReadOnlySpan<double> Data { get; }
}

public sealed class GatewayResult
{
    public int Id { get; init; }
    public required ISerializedValue Value { get; init; }
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