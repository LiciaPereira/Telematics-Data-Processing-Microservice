using Confluent.Kafka;
using System;
using System.Text.Json;
using System.Threading;
using TelemetryData.Domain;

namespace TelemetryData.ConsumerService
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Kafka Consumer Service Started");

      var config = new ConsumerConfig {
        BootstrapServers = "localhost:9092",
        GroupId = "telemetry-processor-group", //a unique group ID for this consumer group
        AutoOffsetReset = AutoOffsetReset.Earliest, // start reading from the beginning if no offset is committed
      };

      //using var for the consumer builder
      //<ignore, string> means it ignore the key and expects string values
      using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build()) {
        consumer.Subscribe("telemetry-events-topic");//subscribe to the same topic as the producer

        CancellationTokenSource cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => {
          e.Cancel = true; //prevent the application from exiting immediatly
          cts.Cancel(); // signal to cancel the consumer loop
        };

        try {
          while (true) {
            try {
              //consume a message
              var consumeResult = consumer.Consume(cts.Token);

              TelemetryDataModel? receivedTelemetry = JsonSerializer.Deserialize<TelemetryDataModel>(consumeResult.Message.Value);

              if (receivedTelemetry != null) {
                Console.WriteLine($"Received TelemetryData: VehicleId={receivedTelemetry.VehicleId}, Speed={receivedTelemetry.Speed}, Engine Status={receivedTelemetry.EngineStatus}, on Partition: {consumeResult.Partition.Value}, Offset: {consumeResult.Offset.Value}");
              } else {
                Console.WriteLine($"Received null telemetry data from message: {consumeResult.Message.Value}");
              }
            }
            catch (ConsumeException e) {
              Console.WriteLine($"Error consuming message: {e.Error.Reason}");
            }
          }
        }
        catch (OperationCanceledException) {
          //consumer was cancelled
          Console.WriteLine("Kafka Consumer Service Shutting down..");
          consumer.Close();//commit final messages and leave the group
        }
      }
    }
  }
}