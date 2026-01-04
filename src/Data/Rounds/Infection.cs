using CS2ZombiePlague.Config;
using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Rounds;

public class Infection(
    ISwiftlyCore core,
    RoundManager roundManager,
    ZombieManager zombieManager,
    Utils utils,
    InfectionRoundConfig config) : IRound
{
    private Guid _playerDeathEvent = Guid.Empty;

    public void End()
    {
        core.Event.OnEntityTakeDamage -= TakeDamage;

        if (config.ZombieRespawn)
        {
            core.GameEvent.Unhook(_playerDeathEvent);
        }

        roundManager.SetRound(new None());

        core.PlayerManager.SendCenter("Раунд окончен");
    }

    public void Start()
    {
        core.Event.OnEntityTakeDamage += TakeDamage;

        if (config.ZombieRespawn)
        {
            _playerDeathEvent = core.GameEvent.HookPre<EventPlayerDeath>(EventPlayerDeath);
        }

        var players = core.PlayerManager.GetAllPlayers().ToList();
        var firstZombie = players[Random.Shared.Next(0, players.Count)];

        zombieManager.CreateZombie(firstZombie);

        firstZombie.SetHealth(firstZombie.PlayerPawn!.Health * config.FirstZombieHealthRatio);

        foreach (var player in players)
        {
            if (!player.IsInfected())
            {
                player.SwitchTeam(Team.CT);
            }
        }

        core.PlayerManager.SendCenter("Первый заражённый => " + firstZombie.Controller.PlayerName);
    }

    private void TakeDamage(IOnEntityTakeDamageEvent @event)
    {
        var attacker = utils.ResolvePlayerFromHandle(@event.Info.Attacker);
        var victim = utils.FindPlayerByPawnAddress(@event.Entity.Address);
        if (victim == null || !victim.IsValid || attacker == null)
            return;
        if (attacker.IsInfected())
        {
            var zombie = zombieManager.GetZombie(attacker.PlayerID);
            if (!zombie.Infect(victim))
            {
                if (!victim.IsLastHuman())
                {
                    victim.SetArmor(victim.PlayerPawn.ArmorValue - (int)@event.Info.Damage);
                    victim.SetHealth(victim.PlayerPawn.Health + (int)@event.Info.Damage);
                }
            }
        }
    }

    private HookResult EventPlayerDeath(EventPlayerDeath @event)
    {
        var player = @event.UserIdPlayer;
        core.Scheduler.DelayBySeconds(3, () =>
        {
            if (player != null && player.IsValid && player.IsInfected() && roundManager.GetRound() == this)
            {
                player.Controller.Respawn();

                var zombie = zombieManager.GetZombie(player.PlayerID);
                zombie.Initialize(player, zombie.GetZombieClass());
            }
        });

        return HookResult.Continue;
    }
}