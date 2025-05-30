using Confluent.Kafka;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using TelemetryData.Domain;
using TelemetryData.Domain.Interfaces;
using TelemetryData.GrpcService;

namespace TelemetryData.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class TelemetryController : ControllerBase
  {
    //dependency injection. the controller depends on the ITelemetryService interface now, not on the concrete implementation.
    private readonly ITelemetryService _telemetryService;
    //configure gRPC
    private readonly TelemetryGrpcService.TelemetryGrpcServiceClient _grpcClient;



    //constructor for dependency injection
    public TelemetryController(ITelemetryService telemetryService, GrpcChannel channel)
    {
      _telemetryService = telemetryService;
      _grpcClient = new TelemetryGrpcService.TelemetryGrpcServiceClient(channel);
    }

    /// <summary>
    /// Retrieves all telemetry data records
    /// </summary>
    /// <returns>A collection of TelemetryDataModel objects</returns>
    [HttpGet]
    public async Task<IEnumerable<TelemetryDataModel>> GetAllTelemetry()
    {
      return await _telemetryService.GetAllTelemetry();
    }

    /// <summary>
    /// Creates a new telemetry data record
    /// </summary>
    /// <param name="telemetry"></param>
    /// <returns>an ActionResult representing the status of the operation (201 when created)</returns>
    [HttpPost]
    public async Task<IActionResult> createNewTelemetryDatamodel([FromBody] TelemetryDataModel telemetry)
    {
      var createdTelemetry = await _telemetryService.CreateTelemetry(telemetry);

      //return 201 created and the new item
      return CreatedAtAction(nameof(GetAllTelemetry), new { id = createdTelemetry.VehicleId }, createdTelemetry);
    }

    /// <summary>
    /// Retrieves the real time status of a specific vehicle from the gRPC service.
    /// </summary>
    /// <param name="vehicleId"></param>
    /// <returns>the status message and the online status</returns>
    [HttpGet("status/{vehicleId}")]
    public async Task<IActionResult> GetVehicleStatus(string vehicleId)
    {
      Guid vehicleGuid;
      if (!Guid.TryParse(vehicleId, out vehicleGuid)) {
        return BadRequest("Invalid Vehicle ID format.");
      }

      //created a gRPC request message as defined in telemetry.proto
      var request = new GetTelemetryStatusRequest { VehicleId = vehicleGuid.ToString() };

      //call the gRPC service async
      var reply = await _grpcClient.GetTelemetryStatusAsync(request);

      //return the reply from the gRPC service (200 OK)
      return Ok(new { reply.StatusMessage, reply.IsOnline });
    }

    [HttpPost("event")] // endpoint for kafka topic
    public async Task<IActionResult> PostTelemetryEvent([FromBody] TelemetryDataModel telemetry)
    {
      //define placeholder variables at first
      var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
      using var producer = new ProducerBuilder<Null, string>(config).Build();

      //attempt to produce a message (just a placeholder)
      await producer.ProduceAsync("telemetry-events-topic", new Message<Null, string> { Value = "Test message" });

      return Ok("Telemetry event published.");
    }
  }
}
