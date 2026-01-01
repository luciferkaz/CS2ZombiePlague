using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace CS2ZombiePlague.Data.Rounds;

public class Nemesis(ISwiftlyCore _core) : IRound
{
    public void Start()
    {
        var players = _core.PlayerManager.GetAllPlayers().ToList();
        var nemesis = players[Random.Shared.Next(0, players.Count)];

        CS2ZombiePlague.ZombieManager.CreateNemesis(nemesis);

        Initialize(nemesis);

        foreach (var player in players)
        {
            if (!player.IsInfected())
            {
                player.SwitchTeam(Team.CT);
            }
        }

        _core.PlayerManager.SendCenter("[red]Немезида => " + nemesis.Controller.PlayerName);
    }

    public void End()
    {
        CS2ZombiePlague.RoundManager.SetRound(RoundType.None);

        _core.PlayerManager.SendCenter("Раунд окончен");
    }

    private void Initialize(IPlayer nemesis)
    {
        var zombieNemesis = CS2ZombiePlague.ZombieManager.GetZombie(nemesis.PlayerID);
        var zombieClass = zombieNemesis.GetZombieClass();
        var countPlayers = _core.PlayerManager.GetAllPlayers().Count();

        nemesis.SetHealth(zombieClass.Health * countPlayers);
    }
}