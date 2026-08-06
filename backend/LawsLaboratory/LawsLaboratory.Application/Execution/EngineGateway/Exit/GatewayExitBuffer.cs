using LawsLaboratory.Application.Execution.ExecutionResultStage;

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

public sealed class GatewayExitBuffer
{
    private readonly ResultPacket<ISerializedValue>[] _results;

    public int Count => _results.Length;

    public GatewayExitBuffer(int maxResultCount)
    {
        _results = new ResultPacket<ISerializedValue>[maxResultCount];

        for (int i = 0; i < maxResultCount; i++)
        {
            _results[i] = new ResultPacket<ISerializedValue>();
        }
    }

    public IResult Get(int index)
    {
        return _results[index];
    }

    public void Set(int index, GatewayResult result)
    {
        _results[index].Set(
            result.Id,
            result.Value);
    }

    public void Clear()
    {
        for (int i = 0; i < _results.Length; i++)
        {
            _results[i].Clear();
        }
    }
}