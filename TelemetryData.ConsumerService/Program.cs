using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json;
using System.Threading;
using TelemetryData.ConsumerService.Processing;
using TelemetryData.Domain;

namespace TelemetryData.ConsumerService
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      Console.WriteLine("Kafka Consumer Service Started");

      var host = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) => {
        config.AddEnvironmentVariables();//add env variables as a configuration source
      }).ConfigureServices((hostContext, services) => {
        services.AddScoped<ITelemetryProcessor, TelemetryProcessor>(); //register the processor for DI
      }).Build();

      var configuration = host.Services.GetRequiredService<IConfiguration>();
      var processor = host.Services.GetRequiredService<ITelemetryProcessor>();

      var config = new ConsumerConfig {
        //now read from "configuration" obj
        BootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers") ?? "localhost:9092",
        GroupId = configuration.GetValue<string>("Kafka:GroupId") ?? "telemetry-processor-group", //a unique group ID for this consumer group
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
                await processor.ProcessTelemetryAsync(receivedTelemetry);
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