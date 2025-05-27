using System.Collections.Generic;
using System.Threading.Tasks;
using TelemetryData.Domain;

namespace TelemetryData.Domain.Interfaces
{
  public interface ITelemetryService
  {
    //async method to retrueve a collection of telemetry data
    Task<IEnumerable<TelemetryDataModel>> GetAllTelemetry();

    //async method to create a new telemetry data record
    Task<TelemetryDataModel> CreateTelemetry(TelemetryDataModel telemetry);
  }
}
