using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Rounds;

public class Nemesis(ISwiftlyCore core, RoundManager roundManager, ZombieManager zombieManager) : IRound
{
    public void Start()
    {
        var players = core.PlayerManager.GetAllPlayers().ToList();
        var nemesis = players[Random.Shared.Next(0, players.Count)];

        zombieManager.CreateNemesis(nemesis);

        Initialize(nemesis);

        foreach (var player in players)
        {
            if (!player.IsInfected())
            {
                player.SwitchTeam(Team.CT);
            }
        }

        core.PlayerManager.SendCenter("[red]Немезида => " + nemesis.Controller.PlayerName);
    }

    public void End()
    {
        roundManager.SetRound(new None());

        core.PlayerManager.SendCenter("Раунд окончен");
    }

    private void Initialize(IPlayer nemesis)
    {
        var zombieNemesis = zombieManager.GetZombie(nemesis.PlayerID);
        var zombieClass = zombieNemesis.GetZombieClass();
        var countPlayers = core.PlayerManager.GetAllPlayers().Count();

        nemesis.SetHealth(zombieClass.Health * countPlayers);
    }
}