using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using TelemetryData.Domain.Interfaces;
using TelemetryData.Infrastructure.Data;
using TelemetryData.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

//add services to container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TelemetryDbContext>(
  options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
//configure and register the GrpcChannel as a singleton for efficiency
builder.Services.AddSingleton(services => {
  //I'll disable SSL certificate validation for now. will configure proper certificates in production
  var grpcChannelOptions = new GrpcChannelOptions {
    Credentials = Grpc.Core.ChannelCredentials.Insecure
  };
  //get gRPC service URL from configuration (like environment variable in Docker Compose)
  var grpcServiceUrl = builder.Configuration.GetValue<string>("GrpcServiceUrl") ?? "http://localhost:5000";//fallback for local dev
  return GrpcChannel.ForAddress(grpcServiceUrl, grpcChannelOptions);
});

//register services for DI
//when an ITelemetryService is requested, the DI container will provide an instance of TelemetryService
builder.Services.AddScoped<ITelemetryService, TelemetryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
