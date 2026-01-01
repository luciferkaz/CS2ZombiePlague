using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public class ZombieNemesis : ZombieClass
{
    public override string InternalName => "zombie_nemesis";
    public override string DisplayName => "Nemesis";
    public override string ZombieModel => "characters/models/tm_phoenix/tm_phoenix.vmdl";
    public override string Description => "";
    public override int Health => 3000;
    public override float Speed => 290.0f;

    public override float Knockback => 1.2f;
    public override int Gravity => 500;

    public override void Initialize(IPlayer player, ZombieController zombieController)
    {
        zombieController.InitializeZombiePlayer(player);
    }
}