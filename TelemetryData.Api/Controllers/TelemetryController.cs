using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TelemetryData.Api.Models;

namespace TelemetryData.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class TelemetryController : ControllerBase
  {
    //static list for in memory storage
    private static List<TelemetryDataModel> _telemetryData = new List<TelemetryDataModel>();

    [HttpGet]
    public IEnumerable<TelemetryDataModel> GetAllTelemetry()
    {
      return _telemetryData; //return the actual list
    }
    [HttpPost]
    ActionResult createNewTelemetryDatamodel([FromBody] TelemetryDataModel telemetry)
    {
      _telemetryData.Add(telemetry);

      //return 201 created and the new item
      return CreatedAtAction(nameof(GetAllTelemetry), new { id = telemetry.VehicleId }, telemetry);
    }
  }
}
