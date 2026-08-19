namespace LawsLaboratory.Api.Gateway.ExternalCalculation.Transport
{
    internal interface IEngineTransport
    {
        Task<byte[]> SendAsync(byte[] request);
    }
}
