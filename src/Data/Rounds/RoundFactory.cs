using CS2ZombiePlague.Config;
using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Rounds;

public class RoundFactory(ISwiftlyCore core, ZombieManager zombieManager, Utils utils) : IRoundFactory
{
    public IRound Create(IRoundConfig? config, RoundManager roundManager)
    {
        return config switch
        {
            InfectionConfig roundConfig => new Infection(core, roundManager, zombieManager, utils, roundConfig),
            NemesisConfig roundConfig => new Nemesis(core, roundManager, zombieManager, roundConfig),
            PlagueConfig roundConfig => new Plague(core, roundManager, zombieManager, utils, roundConfig),
            _ => new None()
        };
    }
}