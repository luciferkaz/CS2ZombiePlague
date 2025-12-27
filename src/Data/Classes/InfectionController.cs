using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.src.Data.Classes
{
    public class InfectionController
    {
        public bool TryInfect(IPlayer target)
        {
            if (target != null && !target.IsInfected() && !target.IsLastHuman())
            {
                Infect(target);
                return true;
            }
            return false;
        }

        private void Infect(IPlayer target)
        {
            CS2ZombiePlague.ZombieManager.CreateZombie(target);
        }
    }
}