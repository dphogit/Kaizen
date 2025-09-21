using Kaizen.API.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;
using Testcontainers.MsSql;

namespace Kaizen.API.FunctionalTests.Infrastructure;

public class ApiTestFixture : IAsyncLifetime
{
    private const string Image = "mcr.microsoft.com/mssql/server:2022-latest";
    private const string Database = "KaizenFunctionalTests";

    private readonly MsSqlContainer _msSql = new MsSqlBuilder()
        .WithImage(Image)
        .Build();

    private Respawner _respawner = null!;

    private string _connectionString = null!;

    private DbContextOptions<KaizenDbContext> _dbContextOptions = null!;

    public TestWebApplicationFactory Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        // Start container
        await _msSql.StartAsync();

        // Default connection uses the `master` DB.
        var masterConnectionString = _msSql.GetConnectionString();

        // Create the test DB from the `master` connection.
        await using (var masterConnection = new SqlConnection(masterConnectionString))
        {
            await masterConnection.OpenAsync();
            var createDbCommand = masterConnection.CreateCommand();
            createDbCommand.CommandText = $"CREATE DATABASE {Database}";
            await createDbCommand.ExecuteNonQueryAsync();
        }

        // Connection options for the test DB.
        var builder = new SqlConnectionStringBuilder(masterConnectionString) { InitialCatalog = Database };
        _connectionString = builder.ConnectionString;

        _dbContextOptions = new DbContextOptionsBuilder<KaizenDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        // Recreate the test DB with the schema defined by the EF model.
        await using (var context = new KaizenDbContext(_dbContextOptions))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        // Setup Respawn to provide the ability to reset/clean the DB within tests on demand.
        // This is more efficient than having to repeatedly deleting and creating the database.
        var respawnerOptions = new RespawnerOptions { TablesToIgnore = [new Table("__EFMigrationsHistory")] };
        _respawner = await Respawner.CreateAsync(_connectionString, respawnerOptions);

        // Create the webapp factory pointing to the setup test container DB
        Factory = new TestWebApplicationFactory(_connectionString);
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connectionString);
        await Reseed();
    }

    public async Task DisposeAsync()
    {
        await _msSql.DisposeAsync();
        await Factory.DisposeAsync();
    }

    private async Task Reseed()
    {
        await using (var context = new KaizenDbContext(_dbContextOptions))
        {
            // The DB should be cleared, but still exist - the schema is created, and seeding is performed.
            await context.Database.EnsureCreatedAsync();
        }
        
        await ReseedAdminUserAsync();
    }

    private async Task ReseedAdminUserAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var services = scope.ServiceProvider;
        
        await IdentitySeeder.SeedAdminUser(
            services,
            TestWebApplicationFactory.TestAdminEmail,
            TestWebApplicationFactory.TestAdminPassword);
    }
}