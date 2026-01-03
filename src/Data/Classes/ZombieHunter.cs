using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public class ZombieHunter : ZombieClass
{
    public override string InternalName => "zombie_hunter";
    public override string DisplayName => "Zombie Hunter";
    public override string ZombieModel => "characters/models/s2ze/zombie_frozen/zombie_frozen.vmdl";
    public override string Description => "";
    public override int Health => 3200;
    public override float Speed => 300.0f;
    public override float Knockback => 0.9f;
    public override int Gravity => 700;
}