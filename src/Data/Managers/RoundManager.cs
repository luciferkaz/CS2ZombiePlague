using CS2ZombiePlague.Data.Rounds;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Managers
{
    public class RoundManager(ISwiftlyCore _core)
    {
        private readonly Dictionary<RoundType, IRound> _rounds = new();
        private IRound? _currentRound;

        private CancellationTokenSource? _token = null;
        private const int RoundStartTime = 10;

        private readonly Random _randomizer = new();

        public void Register(IRound round, RoundType roundType)
        {
            _rounds.Add(roundType, round);
        }

        public void SelectRound(RoundType roundType)
        {
            _currentRound = _rounds[roundType];
        }

        public void SelectRound()
        {
            _currentRound = null;
        }
        
        public IRound? GetRound()
        {
            return _currentRound;
        }
        
        private RoundType RandomRound()
        {
            var roundTypes = Enum.GetValues<RoundType>();
            return roundTypes[_randomizer.Next(1, roundTypes.Length - 1)];
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

        public void Start()
        {
            int localTime = 0;
            _token = _core.Scheduler.RepeatBySeconds(1, () =>
            {
                localTime += 1;
                _core.PlayerManager.SendCenter("До заражения " + (RoundStartTime - localTime) + " секунд");

                if (localTime == RoundStartTime)
                {
                    if (_currentRound == null)
                    {
                        SelectRound(RandomRound());
                    }

                    _currentRound!.Start();
                    _token?.Cancel();
                }
            });
        }
    }
}