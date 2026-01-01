using CS2ZombiePlague.Data.Rounds;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Managers;

public class RoundManager(ISwiftlyCore core, IRoundFactory roundFactory)
{
    private readonly Dictionary<RoundType, IRound> _rounds = new();
    private IRound? _currentRound;

    private CancellationTokenSource? _token;
    private const int RoundStartTime = 15;

    private readonly Random _randomizer = new();

    public void RegisterRounds()
    {
        Register(RoundType.None, roundFactory.Create(RoundType.None, this));
        Register(RoundType.Infection, roundFactory.Create(RoundType.Infection, this));
        Register(RoundType.Plague, roundFactory.Create(RoundType.Plague, this));
        Register(RoundType.Nemesis, roundFactory.Create(RoundType.Nemesis, this));
    }

    private void Register(RoundType roundType, IRound round)
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
        var players = core.PlayerManager.GetAllPlayers();
        return players.Count() > 1;
    }

    public bool IsNoneRound()
    {
        return _currentRound == _rounds[RoundType.None];
    }

    public void Start()
    {
        int localTime = 0;
        _token = core.Scheduler.RepeatBySeconds(1, () =>
        {
            localTime += 1;
            core.PlayerManager.SendCenter("До заражения " + (RoundStartTime - localTime) + " секунд");

            if (localTime >= RoundStartTime)
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

    public void CancelToken()
    {
        if (_token != null)
        {
            _token.Cancel();
        }
    }

    private RoundType RandomRound()
    {
        var roundTypes = Enum.GetValues<RoundType>();
        return roundTypes[_randomizer.Next(1, roundTypes.Length)];
    }
}