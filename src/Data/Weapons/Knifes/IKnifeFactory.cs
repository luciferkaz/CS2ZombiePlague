namespace CS2ZombiePlague.Data.Weapons.Knifes;

public interface IKnifeFactory
{
    public IKnife Create<T>() where T : IKnife;
    
}