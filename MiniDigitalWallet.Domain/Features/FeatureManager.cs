using MiniDigitalWallet.Domain.Features.ChangePin;
using MiniDigitalWallet.Domain.Features.Profile;
using MiniDigitalWallet.Domain.Features.Transfer;

namespace MiniDigitalWallet.Domain.Features;

public static class FeatureManager
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddScoped<RegisterService>();
        services.AddScoped<ProfileService>();
        services.AddScoped<ChangePinService>();
        services.AddScoped<TransferService>();
    }
}