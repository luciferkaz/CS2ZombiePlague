using CS2ZombiePlague.Config.Ability;
using CS2ZombiePlague.Data.Abilities.Contracts;
using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Utils;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;

namespace CS2ZombiePlague.Data.Abilities;

public sealed class Blind : BasePassiveAbility
{
    private readonly ISwiftlyCore _core;
    private readonly BlindConfig _config;
    
    public override float Cooldown => _config.CooldownTime;

    public Blind(ISwiftlyCore core, BlindConfig config) : base(core)
    {
        _core = core;
        _config = config;
        
        core.GameEvent.HookPost<EventPlayerHurt>(OnPlayerHurtPost);
    }

    public override void Use()
    {
        if (Target == null)
        {
            return;
        }
        
        _core.NetMessage.SendCUserMessageFade(
            playerId: Target.PlayerID,
            duration: _config.DurationEffectAfterAbilityOnAttacker,
            holdTime: _config.HoldTimeEffectAfterAbilityOnAttacker,
            flags: NetMessageExt.FFadeOut,
            color: NetMessageExt.Rgba(
                r: _config.RedColorEffectAfterAbilityOnAttacker,
                g: _config.GreenColorEffectAfterAbilityOnAttacker,
                b: _config.BlueColorEffectAfterAbilityOnAttacker,
                a: _config.AlphaEffectAfterAbilityOnAttacker
            )
        );
        
        base.Use();
    }

    private HookResult OnPlayerHurtPost(EventPlayerHurt @event)
    {
        var attacker = @event.AttackerPlayer;
        var victim = @event.UserIdPlayer;
        
        if (!attacker.IsValid || !attacker.IsAlive || attacker.IsInfected())
        {
            return HookResult.Continue;
        }

        if (!victim.IsValid || !victim.IsAlive || !victim.IsInfected())
        {
            return HookResult.Continue;
        }
        
        if (!IsActive && victim.PlayerID == Caster.PlayerID)
        {
            Target = attacker;
            Use();
        }
        
        return HookResult.Continue;
    }
}