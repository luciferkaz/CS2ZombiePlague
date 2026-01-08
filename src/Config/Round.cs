namespace CS2ZombiePlague.Config;

public sealed class RoundConfig
{
    public InfectionConfig Infection { get; set; } = new();
    public PlagueConfig Plague { get; set; } = new();
    public NemesisConfig Nemesis { get; set; } = new();
}

public sealed class InfectionConfig : IRoundConfig
{
    public bool Enable { get; set; } = true;
    public bool ZombieRevived { get; set; } = true;
    public bool FirstZombieLeap { get; set; } = true;
    public float FirstZombieHealthRatio { get; set; } = 1.0f;
    public float ZombieSpawnTime { get; set; } = 5.0f;
}

public sealed class PlagueConfig : IRoundConfig
{
    public bool Enable { get; set; } = true;
    public bool ZombieRevived { get; set; } = true;
    public float ZombieSpawnRatio { get; set; } = 0.3f;
    public float ZombieSpawnTime { get; set; } = 5.0f;
}

public sealed class NemesisConfig : IRoundConfig
{
    public bool Enable { get; set; } = true;
    public bool NemesisLeap { get; set; } = true;
    public int NemesisBonusHealthPerPlayer { get; set; } = 1500;
}