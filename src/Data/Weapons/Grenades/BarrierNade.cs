using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using Vector = SwiftlyS2.Shared.Natives.Vector;

namespace CS2ZombiePlague.Data.Weapons.Grenades;

public class BarrierNade(ISwiftlyCore core, RoundManager roundManager, Utils utils) : ICustomWeapon, IGrenade
{
    public string OriginalName => "decoy_grenade";
    public string IternalName => "barrier_nade";
    public string DisplayName => "BarrierNade";

    readonly float _explodeRadius = 125.0f;
    readonly float _liveTime = 15.0f;
    readonly float _knockbackDistance = 200.0f;

    public void Load()
    {
        core.GameEvent.HookPre<EventDecoyStarted>(PreEventGrenadeStarted);
    }

    public void Explode(int userid, Vector position)
    {
        var startTime = 0f;

        var particle = core.EntitySystem.CreateEntity<CBaseModelEntity>();
        particle.SetModel("characters/models/tm_phoenix/tm_phoenix.vmdl");
        particle.DispatchSpawn();
        particle.Teleport(position, null, null);

        CancellationTokenSource token = null!;
        var delay = 0.05f;

        token = core.Scheduler.RepeatBySeconds(delay, () =>
        {
            startTime += delay;
            var playersInRadius = utils.FindAllPlayersInSphere(_explodeRadius, position);

            foreach (var player in playersInRadius)
            {
                if (player.IsInfected())
                {
                    Knock(player, position);
                }
            }

            if (startTime >= _liveTime || roundManager.IsNoneRound())
            {
                if (particle != null)
                {
                    particle.Despawn();
                }

                token.Cancel();
            }
        });
    }

    private HookResult PreEventGrenadeStarted(EventDecoyStarted @event)
    {
        var grenade = core.EntitySystem.GetEntityByIndex<CEntityInstance>((uint)@event.EntityID);
        if (grenade != null && grenade.IsValid)
        {
            Explode(@event.UserId, new Vector(@event.X, @event.Y, @event.Z));
            grenade.Despawn();
        }

        return HookResult.Stop;
    }

    private void Knock(IPlayer player, Vector position)
    {
        var playerPawn = player.PlayerPawn;
        if (playerPawn == null)
        {
            return;
        }

        var directionVector = (playerPawn.AbsOrigin.Value - position).Normalized();

        bool onGround = playerPawn.GroundEntity.Value != null;
        var zBoost = onGround ? 150f : 25f;

        Vector newVelocity = new Vector(
            playerPawn.AbsVelocity.X + directionVector.X * _knockbackDistance,
            playerPawn.AbsVelocity.Y + directionVector.Y * _knockbackDistance,
            zBoost
        );

        playerPawn.GroundEntity.Value = null;

        playerPawn.Teleport(playerPawn.AbsOrigin.Value, playerPawn.EyeAngles, newVelocity);
    }
}