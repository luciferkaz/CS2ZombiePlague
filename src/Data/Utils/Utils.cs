using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace CS2ZombiePlague.Data.Utils;

public class Utils(ISwiftlyCore _core)
{
    public IPlayer? ResolvePlayerFromHandle(CHandle<CEntityInstance> handle)
    {
        if (!handle.IsValid)
            return null;

        var entity = handle.Value;
        if (entity == null)
            return null;

        foreach (var player in _core.PlayerManager.GetAllPlayers())
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
        foreach (var player in _core.PlayerManager.GetAllPlayers())
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
}