using System.Numerics;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using Vector = SwiftlyS2.Shared.Natives.Vector;

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
    
    /// <summary>
    /// Сбрасывает цвет визуализации (Render color) у всех валидных игроков
    /// до стандартного белого значения (255, 255, 255).
    /// </summary>
    public void AllResetRenderColor()
    {
        var players = core.PlayerManager.GetAllPlayers();
        
        foreach (var player in players)
        {
            if (player != null && player.IsValid)
            {
                player.PlayerPawn.Render = new Color(255, 255, 255);
            }
        }
    }

    public void MoveAllPlayersToTeam(Team team)
    {
        var players = core.PlayerManager.GetAllPlayers();
        
        foreach (var player in players)
        {
            if (player != null && player.IsValid)
            {
                player.SwitchTeam(team);
            }
        }
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
    
    public List<IPlayer> FindAllPlayersInSphere(float radius, Vector position)
    {
        var allPlayers = core.PlayerManager.GetAllPlayers();
        List<IPlayer> findedPlayers = new();
        
        foreach (IPlayer player in allPlayers)
        {
            if (player != null && player.IsValid && player.Controller.PawnIsAlive)
            {
                var playerPosition = player.Pawn.AbsOrigin.Value;
                if (Math.Sqrt(Math.Pow(playerPosition.X - position.X, 2) + Math.Pow(playerPosition.Y - position.Y, 2) +
                              Math.Pow(playerPosition.Z - position.Z, 2)) <= radius)
                {
                    findedPlayers.Add(player);
                }
            }
        }
        return findedPlayers;
    }
}