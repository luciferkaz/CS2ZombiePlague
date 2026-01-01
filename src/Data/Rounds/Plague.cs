using CS2ZombiePlague.Data.Extensions;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Rounds
{
    public class Plague(ISwiftlyCore _core) : IRound
    {
        public void End()
        {
            _core.Event.OnEntityTakeDamage -= TakeDamage;
            _core.Event.OnClientDisconnected -= ClientDisconnected;

            CS2ZombiePlague.RoundManager.SetRound(RoundType.None);

            _core.PlayerManager.SendCenter("Раунд окончен");
        }

        public void Start()
        {
            _core.Event.OnEntityTakeDamage += TakeDamage;
            _core.Event.OnClientDisconnected += ClientDisconnected;

            var players = _core.PlayerManager.GetAllPlayers().ToList();
            var countZombies = Math.Ceiling(players.Count * 0.3);

            foreach (var player in players)
            {
                if (player != null && player.IsValid)
                {
                    CS2ZombiePlague.ZombieManager.CreateZombie(player);
                    countZombies--;
                }

                if (countZombies == 0)
                {
                    break;
                }
            }

            foreach (var player in players)
            {
                if (!player.IsInfected())
                {
                    player.SwitchTeam(Team.CT);
                }
            }

            _core.PlayerManager.SendCenter("Массовое заражение!");
        }

        private void TakeDamage(IOnEntityTakeDamageEvent @event)
        {
            var attacker = CS2ZombiePlague.Utils.ResolvePlayerFromHandle(@event.Info.Attacker);
            var victim = CS2ZombiePlague.Utils.FindPlayerByPawnAddress(@event.Entity.Address);
            if (victim == null || !victim.IsValid || attacker == null)
                return;
            if (attacker.IsInfected())
            {
                var zombie = CS2ZombiePlague.ZombieManager.GetZombie(attacker.PlayerID);
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
            var player = _core.PlayerManager.GetPlayer(@event.PlayerId);
            if (player.IsInfected() && CS2ZombiePlague.ZombieManager.GetAllZombies().Count == 1)
            {
                var players = _core.PlayerManager.GetAllPlayers().ToList();
                var newZombie = players[Random.Shared.Next(0, players.Count)];

                CS2ZombiePlague.ZombieManager.CreateZombie(newZombie);
            }
        }
    }
}