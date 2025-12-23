namespace CS2ZombiePlague.src.Data.Classes
{
    public class ZombieHunter : IZombieClass
    {
        public string InternalName => "zombie_hunter";
        public string DisplayName => "Zombie Hunter";
        public string ZombieModel => "characters/players/zombie_models/zombie_hunter/zombie_hunter.vmdl";
        public string Description => "";
        public int Health => 3200;
        public float Speed => 280.0f;
        public float Knockback => 1.0f;
        public int Gravity => 700;
    }
}