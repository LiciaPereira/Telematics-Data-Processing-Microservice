using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryData.Domain;

namespace TelemetryData.ConsumerService.Processing
{
  internal class TelemetryProcessor : ITelemetryProcessor
  {
    private const float SPEED_ANOMALY_THRESHOLD = 100.0F;

    public Task ProcessTelemetryAsync(TelemetryDataModel telemetry)
    {
      Console.WriteLine($"Processing TelemetryData: VehicleId={telemetry.VehicleId}, Speed={telemetry.Speed}, EngineStatus={telemetry.EngineStatus}, Latitude={telemetry.Latitude}, Longiture={telemetry.Longitude}");

      if (telemetry.Speed > SPEED_ANOMALY_THRESHOLD) {
        Console.WriteLine($"!!! SPEED ANOMALY DETECTED for Vehicle ID: {telemetry.VehicleId} - Speed: {telemetry.Speed}");
      }

      return Task.CompletedTask;
    }
  }
}
