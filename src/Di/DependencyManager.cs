using System.Data;
using CS2ZombiePlague.Data;
using CS2ZombiePlague.Data.Classes;
using CS2ZombiePlague.Data.Managers;
using CS2ZombiePlague.Data.Rounds;
using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Di;

public static class DependencyManager
{
    private static IServiceCollection? _services;
    private static ServiceProvider? _provider;

    public static void Load(ISwiftlyCore core)
    {
        _services = new ServiceCollection();

        _services
            .AddSwiftly(core)
            .AddSingleton<IRoundFactory, RoundFactory>()
            .AddSingleton<IZombiePlayerFactory, ZombiePlayerFactory>()
            .AddSingleton<ZombieManager>()
            .AddSingleton<RoundManager>()
            .AddSingleton<HumanManager>()
            .AddSingleton<Knockback>()
            .AddSingleton<Utils>();

        _provider = _services.BuildServiceProvider();
    }

    public static void Dispose()
    {
        _provider?.Dispose();
        _services = null;
    }

    public static T GetService<T>() where T : notnull
    {
        return _provider == null
            ? throw new NoNullAllowedException(Tag + " _provider is null!")
            : _provider.GetRequiredService<T>();
    }

    private const string Tag = "DependencyManager";
}