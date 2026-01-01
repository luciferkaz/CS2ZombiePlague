using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public class ZombieHeavy : ZombieClass
{
    public override string InternalName => "zombie_heavy";
    public override string DisplayName => "Zombie Heavy";
    public override string ZombieModel => "characters/models/tm_phoenix/tm_phoenix.vmdl";
    public override string Description => "";
    public override int Health => 4000;
    public override float Speed => 240.0f;
    public override float Knockback => 0.3f;
    public override int Gravity => 900;
}