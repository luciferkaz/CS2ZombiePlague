namespace CS2ZombiePlague.Data.ZClasses.Abilities;

public interface ICooldownRestricted
{
    float Cooldown { get; }

    void StartCooldown();

    bool ShouldResetCooldown();

    void ResetCooldown();
}