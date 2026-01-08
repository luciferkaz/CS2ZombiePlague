namespace CS2ZombiePlague.Config.Ability;

public sealed class AbilityConfig
{
    public HealConfig Heal { get; set; } = new();
}

public sealed class HealConfig : IAbilityConfig
{
    public bool Enable { get; set; } = true;

    public float MaxHealDistance { get; set; } = 350f;

    public int HealAmount { get; set; } = 500;
    
    private float CooldownTime { get; set; } = 30f;

    public string ParticleEffectName { get; set; } = "particles/kolka/part2.vpcf";

    public string SoundEffectName { get; set; } = "";
} 