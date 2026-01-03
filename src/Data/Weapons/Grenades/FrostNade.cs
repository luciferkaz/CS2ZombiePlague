using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using Vector = SwiftlyS2.Shared.Natives.Vector;

namespace CS2ZombiePlague.Data.Weapons.Grenades;

public class FrostNade(ISwiftlyCore core, RoundManager roundManager, Utils utils) : ICustomWeapon, IGrenade
{
    public string OriginalName => "hegrenade_projectile";
    public string IternalName => "weapon_frost_nade";
    public string DisplayName => "FrostNade";

    private readonly Dictionary<int, Color> _oldRender = new();
    private readonly Dictionary<int, IPlayer> _frozenPlayers = new();

    private const float ExplodeRadius = 200.0f;
    private const float FreezeTime = 5.0f;

    public void Load()
    {
        core.GameEvent.HookPre<EventHegrenadeDetonate>(PreEventGrenadeDetonate);
        core.Event.OnEntityTakeDamage += TakeDamage;
    }

    public void Explode(int userid, Vector position)
    {
        var playersInRadius = utils.FindAllPlayersInSphere(ExplodeRadius, position);

        foreach (var player in playersInRadius)
        {
            if (player.IsInfected() && !player.IsNemesis() && !player.IsFrozen())
            {
                _oldRender[player.PlayerID] = player.PlayerPawn.Render;
                _frozenPlayers[player.PlayerID] = player;

                player.PlayerPawn.Render = new Color(127, 127, 255);
                player.PlayerPawn.RenderUpdated();
                Freeze(player);

                CreateStateHandler(player);
            }
        }
    }

    private void TakeDamage(IOnEntityTakeDamageEvent @event)
    {
        if (@event.Info.Inflictor.Value != null && @event.Info.Inflictor.Value.DesignerName == OriginalName)
        {
            @event.Info.Damage = 0;
        }
    }

    private HookResult PreEventGrenadeDetonate(EventHegrenadeDetonate @event)
    {
        var grenade = core.EntitySystem.GetEntityByIndex<CEntityInstance>((uint)@event.EntityID);
        if (grenade != null && grenade.IsValid)
        {
            Explode(@event.UserId, new Vector(@event.X, @event.Y, @event.Z));
            grenade.Despawn();
        }

        return HookResult.Stop;
    }

    private void CreateStateHandler(IPlayer player)
    {
        var startTime = 0f;

        CancellationTokenSource token = null!;
        token = core.Scheduler.RepeatBySeconds(0.1f, () =>
        {
            startTime += 0.1f;

            if (player == null && !player.Controller.PawnIsAlive)
            {
                token.Cancel();
            }

            if (startTime >= FreezeTime || !player.IsInfected() || roundManager.IsNoneRound() ||
                !player.Controller.PawnIsAlive)
            {
                _frozenPlayers[player.PlayerID] = null;
                player.PlayerPawn.Render = _oldRender[player.PlayerID];

                UnFreeze(player);
                player.PlayerPawn.RenderUpdated();
                token.Cancel();
            }
        });
    }

    private void Freeze(IPlayer player)
    {
        player.PlayerPawn.MoveType = MoveType_t.MOVETYPE_FLY;
        player.PlayerPawn.ActualMoveType = MoveType_t.MOVETYPE_FLY;
        player.PlayerPawn.MoveTypeUpdated();
        player.PlayerPawn.AbsVelocity = new Vector(0, 0, 0);
    }

    private void UnFreeze(IPlayer player)
    {
        player.PlayerPawn.MoveType = MoveType_t.MOVETYPE_WALK;
        player.PlayerPawn.ActualMoveType = MoveType_t.MOVETYPE_WALK;
        player.PlayerPawn.MoveTypeUpdated();
    }
}