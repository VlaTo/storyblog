using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StoryBlog.Web.Services.Shared.Persistence
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext>
        where TContext : DbContext
    {
        private const string EnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";

        protected string ConnectionStringName
        {
            get;
        }

        protected string MigrationsAssemblyName
        {
            get;
        }

        protected DesignTimeDbContextFactoryBase(string connectionStringName, string migrationsAssemblyName)
        {
            ConnectionStringName = connectionStringName;
            MigrationsAssemblyName = migrationsAssemblyName;
        }

        public TContext CreateDbContext(string[] args)
        {
            return Create(
                Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariable(EnvironmentVariableName),
                ConnectionStringName,
                MigrationsAssemblyName
            );
        }

        public TContext CreateWithConnectionStringName(string connectionStringName, string migrationsAssemblyName)
        {
            var environmentName = Environment.GetEnvironmentVariable(EnvironmentVariableName);
            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName, connectionStringName, migrationsAssemblyName);
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

        private TContext Create(string basePath, string environmentName, string connectionStringName, string migrationsAssemblyName)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = configuration.GetConnectionString(connectionStringName);

            if (String.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find a connection string named 'default'.");
            }

            return CreateWithConnectionString(connectionString, migrationsAssemblyName);
        }

        private TContext CreateWithConnectionString(string connectionString, string migrationsAssemblyName)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));
            }

            var context = new DbContextOptionsBuilder<TContext>();

            context.UseSqlite(connectionString, options =>
            {
                options.MigrationsAssembly(migrationsAssemblyName);
            });

            return CreateNewInstance(context.Options);
        }
    }
}