using Microsoft.EntityFrameworkCore;
using SecretStore.DataAccess.Context;

namespace SecretStore.Extensions;

public static class WebApplicationExtensions
{
    public static async Task MigrateDatabase(this WebApplication app)
    {
        var scope = app.Services.CreateAsyncScope();
        await scope.ServiceProvider
            .GetRequiredService<StoreDbContext>()
            .Database
            .MigrateAsync();
        await scope.DisposeAsync();
    }
}