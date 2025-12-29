using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Rounds
{
    public class Infection(ISwiftlyCore _core) : IRound
    {

        public void End()
        {
            _core.Event.OnEntityTakeDamage -= TakeDamage;
            _core.Event.OnClientDisconnected -= ClientDisconnected;
            _core.Event.OnClientPutInServer -= ClientPutInServer;
            
            CS2ZombiePlague.RoundManager.SetRound(RoundType.None);

            _core.PlayerManager.SendCenter("Раунд окончен");
        }

        public void Start()
        {
            _core.Event.OnEntityTakeDamage += TakeDamage;
            _core.Event.OnClientDisconnected += ClientDisconnected;
            _core.Event.OnClientPutInServer += ClientPutInServer;
                
            var players = _core.PlayerManager.GetAllPlayers().ToList();
            var firstZombie = players[Random.Shared.Next(0, players.Count)];

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
        private void ClientPutInServer(IOnClientPutInServerEvent @event)
        {
            var humansCount = CS2ZombiePlague.HumanManager.GetCountHumans();
            if (humansCount > 1)
            {
                var player = _core.PlayerManager.GetPlayer(@event.PlayerId);
                player.SwitchTeam(Team.T);
                player.Respawn();
                CS2ZombiePlague.ZombieManager.CreateZombie(player);
            }
        }
    }
}