using CS2ZombiePlague.Config.Ability;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Abilities;

public class AbilityFactory(ISwiftlyCore core, IOptions<AbilityConfig> config) : IAbilityFactory
{
    public IAbility Create<T>() where T : IAbility
    {
        return typeof(T) switch
        {
            var t when t == typeof(Heal) => new Heal(core, config.Value.Heal),
            _ => throw new NotSupportedException("ZAbilityFactory: type T hasn't supported!")
        };
    }
}