using CS2ZombiePlague.Config;
using CS2ZombiePlague.Data.Rounds;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Managers;

public class RoundManager(ISwiftlyCore core, IOptions<RoundConfig> roundConfig, IRoundFactory roundFactory)
    : IRoundManager
{
    private readonly List<IRound> _rounds = [];
    private IRound? _currentRound;

    private CancellationTokenSource? _token;
    private const int RoundStartTime = 15;

    public void RegisterRounds()
    {
        _rounds.Clear();

        var config = roundConfig.Value;
        var roundsToRegister = new IRoundConfig?[] { null, config.Infection, config.Plague, config.Nemesis }
            .Where(round => round == null || round.Enable);

        foreach (var round in roundsToRegister)
        {
            var instance = round == null ? roundFactory.Create(null, this) : roundFactory.Create(round, this);
            if (!_rounds.Contains(instance))
            {
                _rounds.Add(instance);
            }
        }
    }

    public void SetRound(IRound round)
    {
        _currentRound = round;
    }

    public IRound? GetRound()
    {
        return _currentRound;
    }

    public bool RoundIsAvailable()
    {
        var players = core.PlayerManager.GetAllPlayers();

        if (core.EntitySystem.GetGameRules()?.WarmupPeriod == true)
        {
            return false;
        }
        return players.Count() > 1;
    }

    public bool IsNoneRound()
    {
        return _currentRound is None;
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
                if (_currentRound is None)
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

    private IRound RandomRound()
    {
        var randomizer = new Random();
        return _rounds[randomizer.Next(1, _rounds.Count)];
    }
}