using LawsLaboratory.Application.Execution.EngineGateway;
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

        GatewayResult result = _gatewayExit.Results[index];

        if (!TryCreateValue(result.Value, out IValue value))
        {
            return false;
        }

        Id = result.Id;
        Value = value;

        return true;
    }


    private static bool TryCreateValue(
        ISerializedValue serializedValue,
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


