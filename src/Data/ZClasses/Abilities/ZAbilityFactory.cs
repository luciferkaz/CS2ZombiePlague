using CS2ZombiePlague.Config.Ability;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.ZClasses.Abilities;

public class ZAbilityFactory(ISwiftlyCore core, IOptions<AbilityConfig> config) : IZAbilityFactory
{
    public IZAbility Create<T>() where T : IZAbility
    {
        return typeof(T) switch
        {
            var t when t == typeof(Heal) => new Heal(core, config.Value.Heal),
            _ => throw new NotSupportedException("ZAbilityFactory: type T hasn't supported!")
        };
    }
}