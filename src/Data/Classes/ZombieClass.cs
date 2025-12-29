using CS2ZombiePlague.src.Data.Classes;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public abstract class ZombieClass
{
    abstract public string InternalName { get; }
    abstract public string DisplayName { get; }
    abstract public string ZombieModel { get; }
    abstract public string Description { get; }
    abstract public int Health { get; }
    abstract public float Speed { get; }
    abstract public float Knockback { get; }
    abstract public int Gravity { get; }

    public abstract void Initialize(IPlayer player, ZombieController zombieController);
}