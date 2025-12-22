using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS2ZombiePlague.src.Data.Classes
{
    public class ZombieManager(ISwiftlyCore _core)
    {
        private Dictionary<int, ZombiePlayer> ZombiePlayers = new()!;

        public ZombiePlayer CreateZombie(IPlayer player)
        {
            if(!ZombiePlayers.TryGetValue(player.PlayerID, out ZombiePlayer? zombiePlayer))
            {
                zombiePlayer = new ZombiePlayer(new ZombieHeavy(player), player);
                ZombiePlayers[player.PlayerID] = zombiePlayer;
            }

            return zombiePlayer;
        }

        public void Remove(IPlayer player)
        {
            if (ZombiePlayers.TryGetValue(player.PlayerID, out ZombiePlayer? zombiePlayer))
            {
                ZombiePlayers.Remove(player.PlayerID);
            }
        }

        public void RemoveAll()
        {
            ZombiePlayers.Clear();
        }

        public bool IsInfected(IPlayer player)
        {
            if(ZombiePlayers.TryGetValue(player.PlayerID, out ZombiePlayer? zombiePlayer))
            {
                return true;
            }
            return false;
        }
    }
}
