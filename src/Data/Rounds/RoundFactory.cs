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
            InfectionRoundConfig roundConfig => new Infection(core, roundManager, zombieManager, utils, roundConfig),
            NemesisRoundConfig roundConfig => new Nemesis(core, roundManager, zombieManager, roundConfig),
            PlagueRoundConfig roundConfig => new Plague(core, roundManager, zombieManager, utils, roundConfig),
            _ => new None()
        };
    }
}