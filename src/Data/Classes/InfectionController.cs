using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.src.Data.Classes
{
    public class InfectionController
    {
        public void TryInfect(IPlayer target)
        {
            if (target != null && !target.IsInfected() && !target.IsLastHuman())
            {
                InfectPlayer(target);
            }
        }

        private void InfectPlayer(IPlayer target)
        {
            CS2ZombiePlague.ZombieManager.CreateZombie(target);
        }
    }
}
