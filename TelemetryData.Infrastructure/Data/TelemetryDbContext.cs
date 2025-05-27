using Microsoft.EntityFrameworkCore;
using TelemetryData.Domain;

namespace TelemetryData.Infrastructure.Data
{
  public class TelemetryDbContext : DbContext
  {
    //constructor that takse DbContextOptions and passes them to the base DbContext constructor
    public TelemetryDbContext(DbContextOptions<TelemetryDbContext> options) : base(options)
    {
    }
    //the =null! is to tell the compiler this property will be initialized by EFCore and wont be null at runtime
    public DbSet<TelemetryDataModel> TelemetryData { get; set; } = null!;
  }
}
