using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Natives;

namespace CS2ZombiePlague.Data;

public class Knockback(ISwiftlyCore _core)
{
    private class KnockbackInfo
    {
        public float recoil;
        public float pickDistance;

        public KnockbackInfo(float recoil, float pickDistance)
        {
            this.recoil = recoil;
            this.pickDistance = pickDistance;
        }
    }

    private readonly Dictionary<string, KnockbackInfo> _weaponKnockback = new Dictionary<string, KnockbackInfo>()
    {
        { "weapon_glock", new KnockbackInfo(150.0f, 100.0f) },
        { "weapon_usp_silencer", new KnockbackInfo(160.0f, 100.0f) },
        { "weapon_hkp2000", new KnockbackInfo(200.0f, 100.0f) },
        { "weapon_elite", new KnockbackInfo(225.0f, 100.0f) },
        { "weapon_p250", new KnockbackInfo(225.0f, 100.0f) },
        { "weapon_fiveseven", new KnockbackInfo(225.0f, 100.0f) },
        { "weapon_cz75a", new KnockbackInfo(240.0f, 100.0f) },
        { "weapon_deagle", new KnockbackInfo(900.0f, 100.0f) },
        { "weapon_revolver", new KnockbackInfo(900.0f, 125.0f) },
        { "weapon_nova", new KnockbackInfo(250.0f, 75.0f) },
        { "weapon_xm1014", new KnockbackInfo(250.0f, 75.0f) },
        { "weapon_sawedoff", new KnockbackInfo(250.0f, 75.0f) },
        { "weapon_mag7", new KnockbackInfo(300.0f, 75.0f) },
        { "weapon_m249", new KnockbackInfo(300.0f, 75.0f) },
        { "weapon_negev", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_mac10", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_mp7", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_mp9", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_mp5sd", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_ump45", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_p90", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_bizon", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_galilar", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_famas", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_ak47", new KnockbackInfo(400.0f, 250.0f) },
        { "weapon_m4a4", new KnockbackInfo(400.0f, 250.0f) },
        { "weapon_m4a1_silencer", new KnockbackInfo(350.0f, 250.0f) },
        { "weapon_ssg08", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_sg556", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_aug", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_awp", new KnockbackInfo(1200.0f, 400.0f) },
        { "weapon_g3sg1", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_scar20", new KnockbackInfo(300.0f, 150.0f) },
        { "weapon_knife", new KnockbackInfo(1200.0f, 25.0f) },
    };

    public void Start()
    {
        _core.GameEvent.HookPost<EventPlayerHurt>(OnPlayerHurt);
    }

    private HookResult OnPlayerHurt(EventPlayerHurt @event)
    {
        var victim = _core.PlayerManager.GetPlayer(@event.UserId);
        var attacker = _core.PlayerManager.GetPlayer(@event.Attacker);
        if (victim == null || attacker == null)
        {
            return HookResult.Continue;
        }

        if (!victim.IsInfected() || attacker.IsInfected())
        {
            return HookResult.Continue;
        }

        var weaponName = $"weapon_{@event.Weapon}";

        var victimOrigin = victim.Pawn!.AbsOrigin.Value;
        var attackerOrigin = attacker.Pawn!.AbsOrigin.Value;

        var directionVector = (victimOrigin - attackerOrigin)!.Normalized();
        var distance = GetDistance(victimOrigin, attackerOrigin);

        float recoil = GetShotgunRecoil(distance, _weaponKnockback[weaponName].recoil,
            _weaponKnockback[weaponName].pickDistance);

        var zombie = CS2ZombiePlague.ZombieManager.GetZombie(victim.PlayerID);
        var zombieKnockback = zombie.GetZombieClass().Knockback;

        Vector newVelocity = new Vector(
            victim.PlayerPawn.AbsVelocity.X + directionVector.X * recoil * zombieKnockback,
            victim.PlayerPawn.AbsVelocity.Y + directionVector.Y * recoil * zombieKnockback,
            0
        );

        if (victim.Pawn.GroundEntity.Value != null)
        {
            victim.Teleport(new Vector(victimOrigin.X, victimOrigin.Y, victimOrigin.Z + 20f),
                victim.PlayerPawn.EyeAngles,
                new Vector(0, 0, 0));
        }

        victim.PlayerPawn.AbsVelocity.X = newVelocity.X;
        victim.PlayerPawn.AbsVelocity.Y = newVelocity.Y;

        return HookResult.Continue;
    }

    private float GetDistance(Vector vector1, Vector vector2)
    {
        return (float)Math.Sqrt(Math.Pow(vector1.X - vector2.X, 2) + Math.Pow(vector1.Y - vector2.Y, 2) +
                                Math.Pow(vector1.Z - vector2.Z, 2));
    }

    private float GetShotgunRecoil(float distance, float recoilMax, float peakDistance, float k = -0.002f)
    {
        if (distance <= peakDistance)
            return recoilMax;
        // Console.WriteLine("recoil:" + (float) (recoilMax * Math.Exp(k * (distance - peakDistance))) + "\n");
        return (float)(recoilMax * Math.Exp(k * (distance - peakDistance)));
    }
}