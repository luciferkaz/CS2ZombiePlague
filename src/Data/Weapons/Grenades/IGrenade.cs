using SwiftlyS2.Shared.Natives;

namespace CS2ZombiePlague.Data.Weapons.Grenades;

public interface IGrenade
{
    public void Explode(int userid, Vector position);
}