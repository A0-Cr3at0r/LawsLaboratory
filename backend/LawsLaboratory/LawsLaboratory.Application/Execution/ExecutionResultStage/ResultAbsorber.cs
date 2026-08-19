// -----------------------------------------------------------------------------
// LawsLaboratory
// Application / Execution / ExecutionResultStage
//
// ResultAbsorber.cs
//
// Reads one serialized result from the GatewayExitBuffer and converts it into
// the corresponding Core value representation.
//
// The absorber validates both the requested result position and the presence
// of a result before attempting deserialization. This provides a defensive
// boundary even though the execution gateway normally guarantees that consumed
// result slots have been populated.
//
// The current implementation accepts scalar results and converts them into
// ScalarValue instances.
//
// Unsupported or invalid serialized values are rejected and represented as
// Dead.Instance.
//
// The absorber performs no spatial write; it only interprets the result
// received from the execution gateway.
// -----------------------------------------------------------------------------

using LawsLaboratory.Application.Execution.EngineGateway.Exit;
using LawsLaboratory.Core.Value;

namespace LawsLaboratory.Application.Execution.ExecutionResultStage;

internal sealed class ResultAbsorber
{
    private readonly GatewayExitBuffer _gatewayExit;

    public IValue Value { get; private set; }

    public int Id { get; private set; }


    public ResultAbsorber(
        GatewayExitBuffer gatewayExit)
    {
        _gatewayExit = gatewayExit;

        Value = Dead.Instance;
        Id = -1;
    }


    public bool TryAbsorb(int index)
    {
        if (index < 0 || index >= _gatewayExit.Results.Length)
        {
            return false;
        }

        GatewayResult? result = _gatewayExit.Results[index];

        if (result is null)
        {
            return false;
        }

        if (!TryCreateValue(result.Value, out IValue value))
        {
            return false;
        }

        Id = result.Id;
        Value = value;

        return true;
    }


    private static bool TryCreateValue(
        SerializedValue serializedValue,
        out IValue value)
    {
        switch (serializedValue.Kind)
        {
            case ValueKind.Scalar:

                if (serializedValue.Data.Length == 0)
                {
                    value = Dead.Instance;
                    return false;
                }

                value = new ScalarValue(serializedValue.Data[0]);
                return true;


            default:

                value = Dead.Instance;
                return false;
        }
    }

}


