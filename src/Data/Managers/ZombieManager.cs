using CS2ZombiePlague.Data.Classes;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Managers;

public class ZombieManager(IZombiePlayerFactory zombiePlayerFactory)
{
    private readonly Dictionary<int, ZombiePlayer> _zombiePlayers = new();

    public ZombiePlayer? CreateZombie(IPlayer player)
    {
        if (player is { IsValid: true })
        {
            return _zombiePlayers[player.PlayerID] =
                zombiePlayerFactory.Create(player, this, new ZombieHunter());
        }

        return null;
    }

    public ZombiePlayer? CreateNemesis(IPlayer player)
    {
        if (player is { IsValid: true })
        {
            return _zombiePlayers[player.PlayerID] =
                zombiePlayerFactory.Create(player, this, new ZombieNemesis(), true);
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