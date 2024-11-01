namespace TicketingSystem.API.Setup;

public static class ApiSetup
{
    public static IServiceCollection ConfigureApi(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        return services
            .AddResponseCaching();
    }
}