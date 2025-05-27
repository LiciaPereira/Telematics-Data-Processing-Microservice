using Microsoft.AspNetCore.Mvc;
using TelemetryData.Domain;
using TelemetryData.Domain.Interfaces;
using TelemetryData.Infrastructure.Services;

namespace TelemetryData.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class TelemetryController : ControllerBase
  {
    //dependency injection. the controller depends on the ITelemetryService interface now, not on the concrete implementation.
    private readonly ITelemetryService _telemetryService;

    //constructor for dependency injection
    public TelemetryController(ITelemetryService telemetryService)
    {
      _telemetryService = telemetryService;
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
  }
}
