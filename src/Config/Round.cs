namespace CS2ZombiePlague.Config;

public sealed class ZombiePlagueRoundConfig
{
    public InfectionRoundConfig Infection { get; set; } = new();
    public PlagueRoundConfig Plague { get; set; } = new();
    public NemesisRoundConfig Nemesis { get; set; } = new();
}

public class InfectionRoundConfig : IRoundConfig
{
    public bool Enable { get; set; } = true;
    public bool ZombieRevived { get; set; } = true;
    public bool FirstZombieLeap { get; set; } = true;
    public float FirstZombieHealthRatio { get; set; } = 1.0f;
    public float ZombieSpawnTime { get; set; } = 5.0f;
}

public class PlagueRoundConfig : IRoundConfig
{
    public bool Enable { get; set; } = true;
    public bool ZombieRevived { get; set; } = true;
    public float ZombieSpawnRatio { get; set; } = 0.3f;
    public float ZombieSpawnTime { get; set; } = 5.0f;
}

public class NemesisRoundConfig : IRoundConfig
{
    public bool Enable { get; set; } = true;
    public bool NemesisLeap { get; set; } = true;
    public int NemesisBonusHealthPerPlayer { get; set; } = 1500;
}