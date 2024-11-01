using TicketingSystem.DAL.Context;
using TicketingSystem.DAL.Repositories;

namespace TicketingSystem.API.Setup;

public static class DataSetup
{
    public static IServiceCollection ConfigureData(this IServiceCollection services, bool isDevelopment)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddScoped<IVenueRepository, VenueRepository>();

        return services
            .AddDbContextPool<TicketingDbContext>(options =>
            {
                options.EnableDetailedErrors(isDevelopment);
                options.EnableSensitiveDataLogging(isDevelopment);
            })
            .AddScoped<TicketingDbContext>(provider => provider.GetRequiredService<TicketingDbContext>());
    }
}