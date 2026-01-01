using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Rounds;

public class RoundFactory(ISwiftlyCore core, ZombieManager zombieManager, Utils utils) : IRoundFactory
{
    public IRound Create(RoundType roundType, RoundManager roundManager)
    {
        return roundType switch
        {
            RoundType.None => new None(),
            RoundType.Infection => new Infection(core, roundManager, zombieManager, utils),
            RoundType.Plague => new Plague(core, roundManager, zombieManager, utils),
            RoundType.Nemesis => new Nemesis(core, roundManager, zombieManager),
            _ => new None()
        };
    }
}