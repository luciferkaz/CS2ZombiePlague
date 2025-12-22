using CS2ZombiePlague.src.Data.Abilities;

namespace CS2ZombiePlague.src.Data.Classes
{
    public interface IZombieClass
    {
        public string InternalName { get; }
        public string DisplayName { get; }
        public string ZombieModel { get; }
        public string Description { get; }
        public int Health { get; }
        public float Speed { get; }
        public float Knockback { get; }
        public int Gravity { get; }

    }
}
