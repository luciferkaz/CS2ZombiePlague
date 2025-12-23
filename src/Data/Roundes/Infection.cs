using SwiftlyS2.Shared;

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
            var randomPlayer = players.First();
            CS2ZombiePlague.ZombieManager.CreateZombie(randomPlayer);

            _core.PlayerManager.SendCenter("Первый заражённый => +" + randomPlayer.Controller.PlayerName);
        }
    }
}