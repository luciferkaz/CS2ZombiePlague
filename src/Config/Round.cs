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
}

public class PlagueRoundConfig : IRoundConfig
{
    public bool Enable { get; set; } = true;
}

public class NemesisRoundConfig : IRoundConfig
{
    public bool Enable { get; set; } = true;
}