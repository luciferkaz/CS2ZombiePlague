using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public abstract class ZombieClass
{
    public abstract string InternalName { get; }
    public abstract string DisplayName { get; }
    public abstract string ZombieModel { get; }
    public abstract string Description { get; }
    public abstract int Health { get; }
    public abstract float Speed { get; }
    public abstract float Knockback { get; }
    public abstract int Gravity { get; }
}