namespace CS2ZombiePlague.Config;

public sealed class ZombiePlagueCoreConfig
{
    public bool DamageNotifyEnabled { get; set; } = true;
    public int DamageNotifyDuration { get; set; } = 1000;

    public bool KnockbackEnabled { get; set; } = true;
    
    public bool MoneySystemEnabled { get; set; } = true;
    public int MaxMoney { get; set; } = 64000;

    public int PreStartDelay { get; set; } = 15;
    public int ZombieSpawnDelay { get; set; } = 5;
}