using CS2ZombiePlague.src.Data.Abilities;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.src.Data.Classes
{
    public class ZombieHeavy : IZombieClass
    {
        public string InternalName => "zombie_heavy";
        public string DisplayName => "Zombie Heavy";
        public string ZombieModel => "characters/players/zombie_models/zombie_heavy/zombie_heavy.vmdl";
        public string Description => "";
        public int Health => 4000;
        public float Speed => 240.0f;
        public float Knockback => 1.0f;
        public int Gravity => 900;

        public BaseAbility ability;

        public ZombieHeavy(IPlayer player)
        {
            ability = new Blindness(player);
        }
    }
}
