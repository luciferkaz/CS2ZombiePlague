using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS2ZombiePlague.src.Data.Classes
{
    public class InfectionController
    {
        public void TryInfectTarget(IPlayer target)
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
