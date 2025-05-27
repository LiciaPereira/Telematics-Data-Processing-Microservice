using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelemetryData.Api.Data;
using TelemetryData.Api.Models;

namespace TelemetryData.Api.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class TelemetryController : ControllerBase
  {
    private readonly TelemetryDbContext _context;

    //constructor for dependency injection
    public TelemetryController(TelemetryDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<TelemetryDataModel>> GetAllTelemetry()
    {
      return await _context.TelemetryData.ToListAsync();
    }
    [HttpPost]
    public async Task<IActionResult> createNewTelemetryDatamodel([FromBody] TelemetryDataModel telemetry)
    {
      _context.TelemetryData.Add(telemetry);
      await _context.SaveChangesAsync(); // commit changes to db

      //return 201 created and the new item
      return CreatedAtAction(nameof(GetAllTelemetry), new { id = telemetry.VehicleId }, telemetry);
    }
  }
}
