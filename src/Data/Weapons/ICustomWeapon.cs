namespace CS2ZombiePlague.Data.Weapons;

public interface ICustomWeapon
{
    public string OriginalName { get; }
    public string IternalName { get; }
    public string DisplayName { get; }

    public void Load();
}