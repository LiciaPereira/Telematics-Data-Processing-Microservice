using Microsoft.EntityFrameworkCore;
using TelemetryData.Domain;
using TelemetryData.Domain.Interfaces;
using TelemetryData.Infrastructure.Data;

namespace TelemetryData.Infrastructure.Services
{
  //provides the concrete implementation of ITelemetryService
  public class TelemetryService : ITelemetryService
  {
    //dependency injection. TelemetryDbContext is injected here to perform db operations
    private readonly TelemetryDbContext _context;
    public TelemetryService(TelemetryDbContext context)
    {
      _context = context;
    }
    /// <summary>
    /// retrieves all telemetry data records from the db
    /// </summary>
    /// <returns>a collection of TelmetryDataModel objects</returns>
    public async Task<IEnumerable<TelemetryDataModel>> GetAllTelemetry()
    {
      return await _context.TelemetryData.ToListAsync();
    }

    /// <summary>
    /// adds a new telemetry data record to the db
    /// </summary>
    /// <param name="telemetry"></param>
    /// <returns>the created TelemetryDataModel object (with updated ID frorm DB)</returns>
    public async Task<TelemetryDataModel> CreateTelemetry(TelemetryDataModel telemetry)
    {
      _context.TelemetryData.Add(telemetry);
      await _context.SaveChangesAsync(); //commits changes to the db
      return telemetry;
    }
  }
}
