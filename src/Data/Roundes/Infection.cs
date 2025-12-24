using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.src.Data.Roundes
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
            _core.PlayerManager.SendCenter("Раунд окончен");
        }

        public void Start()
        {
            var players = _core.PlayerManager.GetAllPlayers().Shuffle();
            var firstZombie = players.First();

            CS2ZombiePlague.ZombieManager.CreateZombie(firstZombie);

            foreach (var player in players)
            {
                if(!player.IsInfected())
                {
                    player.SwitchTeam(Team.CT);
                }
            }

            _core.PlayerManager.SendCenter("Первый заражённый => +" + firstZombie.Controller.PlayerName);
        }
    }
}
