namespace LawsLaboratory.Application.Execution.EngineGateway.ExternalCalculation.Serializer
{
    internal interface IEngineSerializer
    {
        byte[] Serialize();

        void Deserialize();
    }
}
