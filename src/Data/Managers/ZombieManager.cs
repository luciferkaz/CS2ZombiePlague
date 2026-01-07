using CS2ZombiePlague.Data.ZClasses;
using CS2ZombiePlague.Di;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Managers;

public class ZombieManager(IZombiePlayerFactory zombiePlayerFactory, ISwiftlyCore core)
{
    private readonly Dictionary<int, ZombiePlayer> _zombiePlayers = new();

    public ZombiePlayer? CreateZombie(IPlayer player)
    {
        if (player is { IsValid: true })
        {
            var zClass = DependencyManager.GetService<ZCleric>();
            return _zombiePlayers[player.PlayerID] =
                zombiePlayerFactory.Create(player, this, zClass);
        }

        return null;
    }

    public ZombiePlayer? CreateZombie(IPlayer player, int attackerId, int victimId)
    {
        if (player is { IsValid: true })
        {
            core.GameEvent.Fire<EventPlayerDeath>((@event) =>
            {
                @event.UserId = victimId;
                @event.Attacker = attackerId;
                @event.Weapon = "knife";
                @event.Headshot = false;
                @event.Assister = -1;

                core.PlayerManager.GetPlayer(@event.Attacker).Controller.Score++;
                core.PlayerManager.GetPlayer(@event.Attacker).Controller.ScoreUpdated();
            });
            var zClass = DependencyManager.GetService<ZCleric>();
            return _zombiePlayers[player.PlayerID] = zombiePlayerFactory.Create(player, this, zClass);
        }

        return null;
    }

    public ZombiePlayer? CreateNemesis(IPlayer player)
    {
        if (player is { IsValid: true })
        {
            var nemesis = DependencyManager.GetService<ZNemesis>();
            return _zombiePlayers[player.PlayerID] =
                zombiePlayerFactory.Create(player, this, nemesis, true);
        }

        return null;
    }

    public void Remove(IPlayer player)
    {
        _zombiePlayers[player.PlayerID].UnHookAbilities();
        _zombiePlayers.Remove(player.PlayerID);
    }

    public void RemoveAll()
    {
        foreach (var zPlayer in _zombiePlayers.Values)
        {
            zPlayer.UnHookAbilities();
        }

        _zombiePlayers.Clear();
    }

    public ZombiePlayer GetZombie(int playerId)
    {
        return _zombiePlayers[playerId];
    }

    public Dictionary<int, ZombiePlayer> GetAllZombies()
    {
        return _zombiePlayers;
    }
}