using MassTransit;
using MassTransit.RetryPolicies;
using OutboxExample.Contracts.Persistence;

namespace OutboxExampleAPI
{
    public class DatabaseMigratorHostedService : IHostedService
    {
        private readonly ILogger<DatabaseMigratorHostedService> logger;
        private readonly IServiceScopeFactory scopeFactory;

        public DatabaseMigratorHostedService(ILogger<DatabaseMigratorHostedService> logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Applying migrations for {DbContext}", nameof(AppDbContext));
            await Retry.Interval(5, TimeSpan.FromSeconds(5)).Retry(async () =>
            {
                var scope = scopeFactory.CreateScope();
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    await dbContext.Database.EnsureDeletedAsync(cancellationToken);
                    await dbContext.Database.EnsureCreatedAsync(cancellationToken);
                    logger.LogInformation("Migrations completed for {DbContext}", nameof(AppDbContext));
                }
                finally
                {
                    if (scope is IAsyncDisposable disposable)
                    {
                        await disposable.DisposeAsync();
                    }
                    else
                    {
                        scope.Dispose();
                    }
                }
            },cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
