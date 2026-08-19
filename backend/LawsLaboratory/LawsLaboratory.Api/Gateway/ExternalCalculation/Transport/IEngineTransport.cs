// -----------------------------------------------------------------------------
// LawsLaboratory
// Api / Gateway / ExternalCalculation / Transport
//
// IEngineTransport.cs
//
// Defines the communication boundary between the application and an external
// calculation engine.
//
// The transport is responsible only for sending a serialized request and
// receiving the corresponding serialized response. It does not know how the
// request or response is represented semantically.
//
// Serialization and transport are therefore deliberately separated:
//
//     Execution data
//          ↓
//     IEngineSerializer
//          ↓
//     byte[]
//          ↓
//     IEngineTransport
//          ↓
//     external engine
//          ↓
//     byte[]
//          ↓
//     IEngineSerializer
//
// This separation allows the communication mechanism to evolve independently
// from the serialization format and from the simulation execution pipeline.
// -----------------------------------------------------------------------------

namespace LawsLaboratory.Api.Gateway.ExternalCalculation.Transport
{
    internal interface IEngineTransport
    {
        Task<byte[]> SendAsync(byte[] request);
    }
}
