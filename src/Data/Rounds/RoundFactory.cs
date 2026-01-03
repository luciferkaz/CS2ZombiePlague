using CS2ZombiePlague.Config;
using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Rounds;

public class RoundFactory(ISwiftlyCore core, ZombieManager zombieManager, Utils utils) : IRoundFactory
{
    public IRound Create<T>(RoundManager roundManager) where T : IRound
    {
        return typeof(T) switch
        {
            Type t when t == typeof(None) => new None(),
            Type t when t == typeof(Infection) => new Infection(core, roundManager, zombieManager, utils),
            Type t when t == typeof(Plague) => new Plague(core, roundManager, zombieManager, utils),
            Type t when t == typeof(Nemesis) => new Nemesis(core, roundManager, zombieManager),
            _ => new None()
        };
    }

    public IRound Create(IRoundConfig config, RoundManager roundManager)
    {
        return config switch
        {
            InfectionRoundConfig => new Infection(core, roundManager, zombieManager, utils),
            NemesisRoundConfig => new Nemesis(core, roundManager, zombieManager),
            PlagueRoundConfig => new Plague(core, roundManager, zombieManager, utils),
            _ => new None()
        };
    }
}