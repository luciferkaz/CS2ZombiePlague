namespace CS2ZombiePlague.Config;

public sealed class ZombiePlagueCoreConfig
{
    public bool DamageNotifyEnabled { get; set; } = true;
    public int DamageNotifyDuration { get; set; } = 1000;
    
    public bool KnockbackEnabled { get; set; } = true;
}