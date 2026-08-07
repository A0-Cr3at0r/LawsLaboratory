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

public sealed class GatewayExit
{
    public IReadOnlyList<GatewayResult> Results { get; init; }
        = Array.Empty<GatewayResult>();
}