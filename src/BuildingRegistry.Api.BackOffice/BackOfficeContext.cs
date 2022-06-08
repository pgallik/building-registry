namespace BuildingRegistry.Api.BackOffice
{
    using System;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using BuildingRegistry.Infrastructure;

    public class BackOfficeContext : DbContext
    {
        public BackOfficeContext() { }

        public BackOfficeContext(DbContextOptions<BackOfficeContext> options)
            : base(options) { }

        public DbSet<BuildingUnitBuildingRelationship> BuildingUnitBuildingRelationship { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuildingUnitBuildingRelationship>()
                .ToTable("BuildingUnitBuildingRelationship", Schema.BackOffice)
                .HasKey(x => x.BuildingUnitPersistentLocalId)
                .IsClustered();

            modelBuilder.Entity<BuildingUnitBuildingRelationship>()
                .Property(x => x.BuildingUnitPersistentLocalId)
                .ValueGeneratedNever();

            modelBuilder.Entity<BuildingUnitBuildingRelationship>()
                .Property(x => x.BuildingPersistentLocalId);
        }
    }

    public class BuildingUnitBuildingRelationship
    {
        public int BuildingUnitPersistentLocalId { get; set; }
        public int BuildingPersistentLocalId { get; set; }

        private BuildingUnitBuildingRelationship()
        { }

        public BuildingUnitBuildingRelationship(int buildingUnitPersistentLocalId, int buildingPersistentLocalId)
        {
            BuildingUnitPersistentLocalId = buildingUnitPersistentLocalId;
            BuildingPersistentLocalId = buildingPersistentLocalId;
        }
    }

    public class ConfigBasedSequenceContextFactory : IDesignTimeDbContextFactory<BackOfficeContext>
    {
        public BackOfficeContext CreateDbContext(string[] args)
        {
            var migrationConnectionStringName = "BackOffice";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<BackOfficeContext>();

            var connectionString = configuration.GetConnectionString(migrationConnectionStringName);
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException(
                    $"Could not find a connection string with name '{migrationConnectionStringName}'");

            builder
                .UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure();
                    sqlServerOptions.MigrationsHistoryTable(MigrationTables.BackOffice, Schema.BackOffice);
                });

            return new BackOfficeContext(builder.Options);
        }
    }
}
