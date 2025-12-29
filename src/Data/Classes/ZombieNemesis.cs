using CS2ZombiePlague.src.Data.Classes;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public class ZombieNemesis : ZombieClass
{
    public override string InternalName => "zombie_hunter";
    public override string DisplayName => "Zombie Hunter";
    public override string ZombieModel => "characters/models/tm_phoenix/tm_phoenix.vmdl";
    public override string Description => "";
    public override int Health => 3000;
    public override float Speed => 320.0f;

    public override float Knockback => 1.0f;
    public override int Gravity => 500;

    public override void Initialize(IPlayer player, ZombieController zombieController)
    {
        zombieController.ApplyState(player);
    }
}