using System.Data;
using CS2ZombiePlague.Config;
using CS2ZombiePlague.Data;
using CS2ZombiePlague.Data.Classes;
using CS2ZombiePlague.Data.Managers;
using CS2ZombiePlague.Data.Rounds;
using Microsoft.Extensions.Configuration;
using CS2ZombiePlague.Data.Weapons;
using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Di;

public static class DependencyManager
{
    private static IServiceCollection? _services;
    private static ServiceProvider? _provider;
    
    private const string CoreConfigName = "zp_core.json";
    private const string CoreConfigSectionName = "ZombiePlagueCoreConfig";
    private const string RoundConfigName = "zp_round.json";
    private const string RoundConfigSectionName = "ZombiePlagueRoundConfig";

    public static void Load(ISwiftlyCore core)
    {
        core.Configuration
            .InitializeJsonWithModel<ZombiePlagueRoundConfig>(RoundConfigName, RoundConfigSectionName)
            .Configure(builder => { builder.AddJsonFile(RoundConfigName, optional: false, reloadOnChange: true); });
        
        _services = new ServiceCollection();

        _services
            .AddSwiftly(core)
            .AddSingleton<IRoundFactory, RoundFactory>()
            .AddSingleton<IZombiePlayerFactory, ZombiePlayerFactory>()
            .AddSingleton<ZombieManager>()
            .AddSingleton<RoundManager>()
            .AddSingleton<HumanManager>()
            .AddSingleton<Knockback>()
            .AddSingleton<WeaponManager>()
            .AddSingleton<Utils>();

        _services
            .AddOptionsWithValidateOnStart<ZombiePlagueRoundConfig>()
            .BindConfiguration(RoundConfigSectionName);

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