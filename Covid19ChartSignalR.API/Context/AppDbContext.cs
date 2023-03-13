using Covid19ChartSignalR.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Covid19ChartSignalR.API.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Covid> Covids { get; set; }


}