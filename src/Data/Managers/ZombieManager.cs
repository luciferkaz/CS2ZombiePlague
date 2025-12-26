using CS2ZombiePlague.src.Data.Classes;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Managers
{
    public class ZombieManager(ISwiftlyCore _core)
    {
        private Dictionary<int, ZombiePlayer> _zombiePlayers = new()!;

        public ZombiePlayer CreateZombie(IPlayer player)
        {
            return _zombiePlayers[player.PlayerID] = new ZombiePlayer(new ZombieHunter(), player);
        }

        public void Remove(IPlayer player)
        {
            _zombiePlayers.Remove(player.PlayerID);
        }

        public void RemoveAll()
        {
            _zombiePlayers.Clear();
        }

        public ZombiePlayer GetZombie(int playerID)
        {
            return _zombiePlayers[playerID];
        }

        public Dictionary<int, ZombiePlayer> GetAllZombies()
        {
            return _zombiePlayers;
        }
    }
}