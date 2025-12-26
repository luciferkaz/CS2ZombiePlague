using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Rounds
{
    public class Infection : IRound
    {
        private ISwiftlyCore _core;

        public Infection(ISwiftlyCore core)
        {
            _core = core;
        }

        public void End()
        {
            _core.Event.OnEntityTakeDamage -= TakeDamage;
            CS2ZombiePlague.RoundManager.SelectRound();

            _core.PlayerManager.SendCenter("Раунд окончен");
        }

        public void Start()
        {
            _core.Event.OnEntityTakeDamage += TakeDamage;

            var players = _core.PlayerManager.GetAllPlayers().Shuffle();
            var firstZombie = players.First();

            CS2ZombiePlague.ZombieManager.CreateZombie(firstZombie);

            foreach (var player in players)
            {
                if (!player.IsInfected())
                {
                    player.SwitchTeam(Team.CT);
                }
            }

            _core.PlayerManager.SendCenter("Первый заражённый => +" + firstZombie.Controller.PlayerName);
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
                zombie.Infect(victim);
            }

            if (victim.IsInfected())
            {
                _core.Scheduler.NextTick(() =>
                {
                    var zombie = CS2ZombiePlague.ZombieManager.GetZombie(victim.PlayerID);
                    victim.SetSpeed(zombie.GetZombieClass().Speed);
                });
            }
        }
    }
}