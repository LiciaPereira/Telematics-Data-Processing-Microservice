using TelemetryData.Domain;

namespace TelemetryData.Tests
{

  public class TelemetryDataModelTests
  {
    [Fact]
    public void ConstructorShouldInitializeAllPropertiesCorrectly()
    {
      //arrange
      var testVehicleId = Guid.NewGuid();
      var testDateCreated = DateTime.UtcNow;
      var testLatitude = -23.1816390M;
      var testLongitude = -46.884170M;
      var testSpeed = 75.0F;
      var testEngine = EngineStatus.Idle;

      //act
      var telemetry = new TelemetryDataModel(
        testVehicleId,
        testDateCreated,
        testLatitude,
        testLongitude,
        testSpeed,
        testEngine);

      //assert
      Assert.Equal(testVehicleId, telemetry.VehicleId);
      Assert.Equal(testDateCreated, telemetry.DateCreated);
      Assert.Equal(testLatitude, telemetry.Latitude);
      Assert.Equal(testLongitude, telemetry.Longitude);
      Assert.Equal(testSpeed, telemetry.Speed, 0.0001F);
      Assert.Equal(testEngine, telemetry.EngineStatus);
    }
  }
}