using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public interface IZombiePlayerFactory
{
    public ZombiePlayer Create(IPlayer player, ZombieManager zombieManager, ZombieClass zombieClass, bool isNemesis = false);
}