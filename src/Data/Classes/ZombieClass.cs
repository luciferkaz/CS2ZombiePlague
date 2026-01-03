namespace CS2ZombiePlague.Data.Classes;

/**
 * 1. Клерик (хил)
 * 2. Охотник (ставит ловушки)
 * 3. Ассасин (ускоряется)
 * 4. Толстый (ослепляет)
 * 5. Шаман (кричит)
 */
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