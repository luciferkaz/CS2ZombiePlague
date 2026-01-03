using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Rounds;

public class Infection(ISwiftlyCore core, RoundManager roundManager, ZombieManager zombieManager, Utils utils) : IRound
{
    public void End()
    {
        core.Event.OnEntityTakeDamage -= TakeDamage;
        core.Event.OnClientDisconnected -= ClientDisconnected;

        roundManager.SetRound(new None());

        core.PlayerManager.SendCenter("Раунд окончен");
    }

    public void Start()
    {
        core.Event.OnEntityTakeDamage += TakeDamage;
        core.Event.OnClientDisconnected += ClientDisconnected;

        var players = core.PlayerManager.GetAllPlayers().ToList();
        var firstZombie = players[Random.Shared.Next(0, players.Count)];

        zombieManager.CreateZombie(firstZombie);

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
                victim.SetArmor(victim.PlayerPawn.ArmorValue - (int)@event.Info.Damage);

                if (!victim.IsLastHuman())
                {
                    victim.PlayerPawn?.Health = victim.PlayerPawn.Health + (int)@event.Info.Damage;
                }
            }
        }
    }

    private void ClientDisconnected(IOnClientDisconnectedEvent @event)
    {
        var player = core.PlayerManager.GetPlayer(@event.PlayerId);
        if (player.IsInfected() && zombieManager.GetAllZombies().Count == 1)
        {
            var players = core.PlayerManager.GetAllPlayers().ToList();
            var newZombie = players[Random.Shared.Next(0, players.Count)];

            zombieManager.CreateZombie(newZombie);
        }
    }
}