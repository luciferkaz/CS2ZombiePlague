using CS2ZombiePlague.Config.Ability;
using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Data.Managers;
using CS2ZombiePlague.Di;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace CS2ZombiePlague.Data.ZClasses.Abilities;

public class Heal(ISwiftlyCore core, HealConfig config) : ZBaseAbility(core)
{
    private const float EyePositionZ = 64f;

    public override float Cooldown => 30f;

    protected override bool IsPassive => false;
    protected override KeyKind Key => KeyKind.E;

    private readonly Utils _utils = DependencyManager.GetService<Utils>();
    private readonly ZombieManager _zombieManager = DependencyManager.GetService<ZombieManager>();
    private readonly ISwiftlyCore _core = core;

    protected override void Use()
    {
        var casterPawn = Caster.RequiredPlayerPawn;

        var origin = casterPawn.CBodyComponent?.SceneNode?.AbsOrigin;
        if (origin is null)
            return;

        var start = origin.Value + new Vector(0f, 0f, EyePositionZ);

        var forward = ForwardFromAngles(casterPawn.EyeAngles);
        var end = start + forward * config.MaxHealDistance;

        if (!TryFindHealTarget(casterPawn, start, end, out var target))
            return;

        if (!target.IsInfected())
            return;

        var targetPawn = target.RequiredPlayerPawn;
        
        core.PlayerManager.SendChat(Caster.Controller.PlayerName + " хилит " + target.Controller.PlayerName);

        ApplyHeal(target, targetPawn);

        CreateParticle(target);

        StartCooldown();
    }

    protected override void OnClientClickButtonHandler(int playerId, KeyKind key, bool pressed)
    {
        if (key == Key && pressed)
        {
            TryUse();
        }
    }
    
    protected override bool CanUse(IPlayer player)
    {
        return player is { IsValid: true, Controller.PawnIsAlive: true } && player.IsInfected();
    }
    
    protected override void CreateParticle(IPlayer target)
    {
        DestroyParticle();

        var pawn = target.RequiredPlayerPawn;
        var particle = _core.EntitySystem.CreateEntity<CParticleSystem>();
        
        particle.EffectName = config.ParticleEffectName;
        particle.StartActive = true;
        particle.DispatchSpawn();

        particle.Teleport(pawn.AbsOrigin, null, null);
        particle.AcceptInput("SetParent", "!activator", pawn, particle);
        particle.AcceptInput("SetParentAttachment", "knife", pawn);

        Particle = particle;

        core.Scheduler.DelayBySeconds(2.0f, DestroyParticle);
    }
    

    public override bool ShouldResetCooldown()
    {
        if (!Caster.Controller.PawnIsAlive)
        {
            return true;
        }

        if (!Caster.IsInfected())
        {
            return true;
        }
        
        return base.ShouldResetCooldown();
    }
    
    private bool TryFindHealTarget(CBasePlayerPawn casterPawn, Vector start, Vector end, out IPlayer target)
    {
        target = null!;

        var trace = new CGameTrace();

        _core.Trace.SimpleTrace(
            start,
            end,
            RayType_t.RAY_TYPE_LINE,
            RnQueryObjectSet.AllGameEntities | RnQueryObjectSet.Static,
            MaskTrace.Solid | MaskTrace.Player,
            MaskTrace.Empty,
            MaskTrace.Empty,
            CollisionGroup.Player,
            ref trace,
            casterPawn
        );

        var entity = trace.Entity;
        if (entity is null)
            return false;

        var found = _utils.FindPlayerByPawnAddress(entity.Address);
        if (found is null || !found.IsValid || !found.Controller.PawnIsAlive)
            return false;

        target = found;
        return true;
    }
    
    private void ApplyHeal(IPlayer target, CBasePlayerPawn targetPawn)
    {
        var currentHp = targetPawn.Health;
        var newHp = currentHp + config.HealAmount;
        var maxTargetHp = _zombieManager.GetZombie(target.PlayerID).GetZombieClass().Health;

        if (newHp >= maxTargetHp)
        {
            newHp = maxTargetHp;
        }

        if (newHp < 1)
        {
            newHp = 1;
        }

        target.SetHealth(newHp);
    }

    private static Vector ForwardFromAngles(QAngle angles)
    {
        const float deg2Rad = MathF.PI / 180f;

        var pitch = angles.Pitch * deg2Rad;
        var yaw = angles.Yaw * deg2Rad;

        var cosPitch = MathF.Cos(pitch);

        return new Vector(
            cosPitch * MathF.Cos(yaw),
            cosPitch * MathF.Sin(yaw),
            -MathF.Sin(pitch)
        );
    }
}