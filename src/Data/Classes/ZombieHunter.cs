namespace CS2ZombiePlague.src.Data.Classes
{
    public class ZombieHunter : IZombieClass
    {
        public string InternalName => "zombie_hunter";
        public string DisplayName => "Zombie Hunter";
        public string ZombieModel => "characters/models/tm_phoenix/tm_phoenix.vmdl";
        public string Description => "";
        public int Health => 3200;
        public float Speed => 380.0f;
        public float Knockback => 1.0f;
        public int Gravity => 700;
    }
}