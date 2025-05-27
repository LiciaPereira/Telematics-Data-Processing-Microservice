using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TelemetryData.Api.Models;

namespace TelemetryData.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class TelemetryController : ControllerBase
  {
    [HttpGet]
    public IEnumerable<TelemetryDataModel> GetAllTelemetry()
    {
      return new List<TelemetryDataModel>();
    }
  }
}
