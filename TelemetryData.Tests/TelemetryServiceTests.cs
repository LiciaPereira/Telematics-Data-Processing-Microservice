using Microsoft.EntityFrameworkCore;
using TelemetryData.Domain;
using TelemetryData.Infrastructure.Data;
using TelemetryData.Infrastructure.Services;

namespace TelemetryData.Tests;

public class TelemetryServiceTests
{
  [Fact]
  public async Task CreateTelemetryPersistsRecord()
  {
    var options = CreateOptions();
    await using var context = new TelemetryDbContext(options);
    var service = new TelemetryService(context);
    var telemetry = CreateTelemetry();

    var created = await service.CreateTelemetry(telemetry);
    var records = await context.TelemetryData.ToListAsync();

    Assert.Equal(telemetry.VehicleId, created.VehicleId);
    Assert.Single(records);
    Assert.Equal(telemetry.VehicleId, records[0].VehicleId);
  }

  [Fact]
  public async Task GetAllTelemetryReturnsStoredRecords()
  {
    var options = CreateOptions();
    await using var context = new TelemetryDbContext(options);
    var telemetry = CreateTelemetry();
    context.TelemetryData.Add(telemetry);
    await context.SaveChangesAsync();
    var service = new TelemetryService(context);

    var records = await service.GetAllTelemetry();

    Assert.Contains(records, item => item.VehicleId == telemetry.VehicleId);
  }

  private static DbContextOptions<TelemetryDbContext> CreateOptions()
  {
    return new DbContextOptionsBuilder<TelemetryDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;
  }

  private static TelemetryDataModel CreateTelemetry()
  {
    return new TelemetryDataModel(
      Guid.NewGuid(),
      DateTime.UtcNow,
      -23.1816390M,
      -46.884170M,
      75.0F,
      EngineStatus.Running);
  }
}
