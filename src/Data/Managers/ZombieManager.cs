using CS2ZombiePlague.Data.Classes;
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
            return _zombiePlayers[player.PlayerID] =
                zombiePlayerFactory.Create(player, this, new ZombieHunter());
        }

        return null;
    }

    public void CreateZombie(IPlayer player, int attackerid, int victimid)
    {
        if (player is { IsValid: true })
        {
            core.GameEvent.Fire<EventPlayerDeath>((@event) =>
            {
                @event.UserId = victimid;
                @event.Attacker = attackerid;
                @event.Weapon = "knife";
                @event.Headshot = false;

                core.PlayerManager.GetPlayer(@event.Attacker).Controller.KillCount += 1;
                core.PlayerManager.GetPlayer(@event.Attacker).Controller.KillCountUpdated();
            });
            
            core.Scheduler.NextTick(() =>
            {
                _zombiePlayers[player.PlayerID] =
                    zombiePlayerFactory.Create(player, this, new ZombieHunter());
            });
        }
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