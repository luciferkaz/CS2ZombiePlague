using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace CS2ZombiePlague.Data;

public class Utils(ISwiftlyCore core)
{
    public IPlayer? ResolvePlayerFromHandle(CHandle<CEntityInstance> handle)
    {
        if (!handle.IsValid)
            return null;

        var entity = handle.Value;
        if (entity == null)
            return null;

        foreach (var player in core.PlayerManager.GetAllPlayers())
        {
            try
            {
                if (player.PlayerPawn?.Address == entity.Address ||
                    player.Controller?.Address == entity.Address)
                    return player;
            }
            catch (NullReferenceException)
            {
                continue;
            }
        }

        return null;
    }

    public IPlayer? FindPlayerByPawnAddress(nint address)
    {
        foreach (var player in core.PlayerManager.GetAllPlayers())
        {
            try
            {
                var pawn = player.PlayerPawn;
                if (pawn != null && pawn.Address == address)
                    return player;
            }
            catch (NullReferenceException)
            {
                continue;
            }
        }

        return null;
    }

    public void SortTeam()
    {
        int terroristCount = 0;
        int counterTerroristCount = 0;
        var allPlayers = core.PlayerManager.GetAllPlayers();

        foreach (var player in allPlayers)
        {
            if (player.Controller == null)
            {
                continue;
            }

            if (player.Controller.TeamNum == (int)Team.CT)
            {
                counterTerroristCount++;
            }
            else if (player.Controller.TeamNum == (int)Team.T)
            {
                terroristCount++;
            }
        }

        Team teamToSort;
        int playersToSort = Math.Abs(counterTerroristCount - terroristCount) / 2;

        if (playersToSort == 0)
        {
            return;
        }

        if (counterTerroristCount > terroristCount)
        {
            teamToSort = Team.CT;
        }
        else
        {
            teamToSort = Team.T;
        }

        foreach (var player in allPlayers)
        {
            if (player.Controller == null)
            {
                continue;
            }

            if (player.Controller.TeamNum == (int)teamToSort)
            {
                player.SwitchTeam(teamToSort == Team.CT ? Team.T : Team.CT);
                playersToSort--;
            }

            if (playersToSort == 0)
            {
                return;
            }
        }
    }
}