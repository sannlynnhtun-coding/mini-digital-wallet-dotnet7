namespace MiniDigitalWallet.Domain.Features;

public static class FeatureManager
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<RegisterService>();
    }
}