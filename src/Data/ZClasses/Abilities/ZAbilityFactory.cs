using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.ZClasses.Abilities;

public class ZAbilityFactory(ISwiftlyCore core, ZombieManager zombieManager, Utils utils) : IZAbilityFactory
{
    public IZAbility Create<T>() where T : IZAbility
    {
        return typeof(T) switch
        {
            var t when t == typeof(Heal) => new Heal(core, utils),
            var t when t == typeof(Leap) => new Leap(core, utils),
            var t when t == typeof(Charge) => new Charge(core, zombieManager),
            _ => throw new NotSupportedException("ZAbilityFactory: type T hasn't supported!")
        };
    }
}