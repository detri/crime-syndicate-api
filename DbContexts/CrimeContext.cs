using CrimeSyndicate.ModelConfigs;
using CrimeSyndicate.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimeSyndicate.DbContexts;

public class CrimeContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public CrimeContext(DbContextOptions<CrimeContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserConfig().Configure(modelBuilder.Entity<User>());
    }
}
