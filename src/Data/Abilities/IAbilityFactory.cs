namespace CS2ZombiePlague.Data.Abilities;

public interface IAbilityFactory
{
    public IAbility Create<T>() where T : IAbility;
}