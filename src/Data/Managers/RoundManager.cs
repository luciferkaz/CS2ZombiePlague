using CS2ZombiePlague.src.Data.Roundes;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.src.Data.Managers
{
    public class RoundManager(ISwiftlyCore _core)
    {
        private readonly List<IRound> _rounds = new();
        private IRound? _currentRound;

        private CancellationTokenSource? token = null;
        private const int RoundStartTime = 10;

        public void Register(IRound round)
        {
            _rounds.Add(round);
            _currentRound = null;
        }

        public void SelectRound(int round)
        {
            _currentRound = _rounds[round];
            int localTime = 0;
            token = _core.Scheduler.RepeatBySeconds(1, () => {
                if(_currentRound == null) { token?.Cancel(); }

                localTime += 1;
                _core.PlayerManager.SendCenter("До заражения " + (RoundStartTime-localTime) + " секунд");
                if (_currentRound != null && localTime == RoundStartTime)
                {
                    _currentRound.Start();
                    token?.Cancel();
                }
            });
        }

        public bool GameIsAvailable()
        {
            var players = _core.PlayerManager.GetAllPlayers();
            if (players.Count() > 1)
            {
                return true;
            }
            return false;
        }

        public IRound GetRound()
        {
            return _currentRound;
        }
    }
}
