using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Linq;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public static readonly LoggerFactory MyConsoleloggerFactory =
            new LoggerFactory(new[] {
                new ConsoleLoggerProvider((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information, true) });


        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Qoute> Qoutes{ get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging(true)
                .UseLoggerFactory(MyConsoleloggerFactory)
                .UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = SamuraiAppData; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new { s.SamuraiId, s.BattleId });
            //modelBuilder.Entity<Samurai>().Property<DateTime>("Created");
            //modelBuilder.Entity<Samurai>().Property<DateTime>("LastModified");
            foreach(var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("Created");
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModified");
            }

            modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(b => b.GivenName).HasColumnName("GivenName");
            modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(b => b.SurName).HasColumnName("SurName");
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var timeStamp = DateTime.Now;

            foreach(var entry in ChangeTracker.Entries()
                .Where(e=>(e.State == EntityState.Added || e.State == EntityState.Modified)
                            && !e.Metadata.IsOwned()))
            {
                entry.Property("LastModified").CurrentValue = timeStamp;

                if(entry.State == EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = timeStamp;
                }
            }
            return base.SaveChanges();
        }
    }
}
