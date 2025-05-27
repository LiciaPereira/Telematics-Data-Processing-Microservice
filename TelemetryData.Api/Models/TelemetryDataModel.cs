using System.ComponentModel.DataAnnotations;

namespace TelemetryData.Api.Models
{
  public class TelemetryDataModel
  {
    [Key]
    public Guid VehicleId { get; set; }
    public DateTime DateCreated { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public float Speed { get; set; }
    public EngineStatus EngineStatus { get; set; }

    public TelemetryDataModel(
      Guid vehicleId,
      DateTime dateCreated,
      decimal latitude,
      decimal longitude,
      float speed,
      EngineStatus engineStatus
      )
    {
      VehicleId = vehicleId;
      DateCreated = dateCreated;
      Latitude = latitude;
      Longitude = longitude;
      Speed = speed;
      EngineStatus = engineStatus;
    }
  }
  public enum EngineStatus
  {
    Off = 0,
    Running = 1,
    Idle = 2
  }
}