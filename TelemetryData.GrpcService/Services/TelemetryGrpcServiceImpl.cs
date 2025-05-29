using Grpc.Core;
using TelemetryData.GrpcService;

namespace TelemetryData.GrpcService.Services
{
  //the concrete implementation of the TelemetryGrpcService defined in telemetry.proto
  //it inherits frorm the generated base class, which provides the contract methods that must be overriden
  public class TelemetryGrpcServiceImpl : TelemetryGrpcService.TelemetryGrpcServiceBase
  {
    //implememnt the method from the .proto contract. it processes the GetTelemetryStatusRequest and return a TelemetryStatusReply
    public override Task<TelemetryStatusReply> GetTelemetryStatus(
      GetTelemetryStatusRequest request, //the input message frorm the gRPC client
      ServerCallContext context)
    {
      //hardcoded for now
      return Task.FromResult(new TelemetryStatusReply {
        StatusMessage = $"Status for Vehicle ID: {request.VehicleId}",
        IsOnline = true
      });
    }
  }
}