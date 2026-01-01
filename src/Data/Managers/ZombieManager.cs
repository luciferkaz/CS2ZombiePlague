using CS2ZombiePlague.Data.Classes;
using CS2ZombiePlague.Data.Rounds;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Managers;

public class ZombieManager
{
    private Dictionary<int, ZombiePlayer> _zombiePlayers = new()!;

    public ZombiePlayer? CreateZombie(IPlayer player)
    {
        if (player != null && player.IsValid)
        {
            return _zombiePlayers[player.PlayerID] = new ZombiePlayer(new ZombieHunter(), player);
        }

        return null;
    }

    public ZombiePlayer? CreateNemesis(IPlayer player)
    {
        if (player != null && player.IsValid)
        {
            return _zombiePlayers[player.PlayerID] = new ZombiePlayer(new ZombieNemesis(), player);
        }

        return null;
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