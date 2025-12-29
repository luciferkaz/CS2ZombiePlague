using CS2ZombiePlague.Data.Rounds;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Managers
{
    public class RoundManager(ISwiftlyCore _core)
    {
        private readonly Dictionary<RoundType, IRound> _rounds = new();
        private IRound? _currentRound;

        private CancellationTokenSource? _token = null;
        private const int ROUND_START_TIME = 10;

        private readonly Random _randomizer = new();

        public void Register(RoundType roundType, IRound round)
        {
            _rounds.Add(roundType, round);
        }

        public void SetRound(RoundType roundType)
        {
            _currentRound = _rounds[roundType];
        }
        
        public IRound? GetRound()
        {
            return _currentRound;
        }

        public bool RoundIsAvailable()
        {
            var players = _core.PlayerManager.GetAllPlayers();
            return players.Count() > 1;
        }

        public bool IsNoneRound()
        {
            return _currentRound == _rounds[RoundType.None];
        }

        public void Start()
        {
            int localTime = 0;
            _token = _core.Scheduler.RepeatBySeconds(1, () =>
            {
                localTime += 1;
                _core.PlayerManager.SendCenter("До заражения " + (ROUND_START_TIME - localTime) + " секунд");

                if (localTime >= ROUND_START_TIME)
                {
                    if (_currentRound == _rounds[RoundType.None])
                    {
                        SetRound(RandomRound());
                    }

                    _currentRound!.Start();
                    _token?.Cancel();
                }
            });
        }
        
        private RoundType RandomRound()
        {
            var roundTypes = Enum.GetValues<RoundType>();
            return roundTypes[_randomizer.Next(1, roundTypes.Length)];
        }
    }
}