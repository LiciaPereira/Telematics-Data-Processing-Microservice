using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelemetryData.Domain;

namespace TelemetryData.ConsumerService.Processing
{
  internal interface ITelemetryProcessor
  {
    Task ProcessTelemetryAsync(TelemetryDataModel telemetry);
  }
}
