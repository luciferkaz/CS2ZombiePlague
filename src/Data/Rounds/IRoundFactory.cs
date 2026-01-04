using CS2ZombiePlague.Config;
using CS2ZombiePlague.Data.Managers;

namespace CS2ZombiePlague.Data.Rounds;

public interface IRoundFactory
{
    public IRound Create(IRoundConfig? config, RoundManager roundManager);
}