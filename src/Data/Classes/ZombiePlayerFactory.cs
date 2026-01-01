using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public class ZombiePlayerFactory : IZombiePlayerFactory
{
    public ZombiePlayer Create(IPlayer player, ZombieManager zombieManager, ZombieClass zombieClass, bool isNemesis = false)
    {
        return zombieClass switch
        {
            ZombieHeavy zombieHeavy => new ZombiePlayer(player, zombieManager, zombieHeavy, isNemesis),
            ZombieHunter zombieHunter => new ZombiePlayer(player, zombieManager, zombieHunter, isNemesis),
            ZombieNemesis zombieNemesis => new ZombiePlayer(player, zombieManager, zombieNemesis, isNemesis),
            _ => new ZombiePlayer(player, zombieManager, new ZombieHunter(), isNemesis)
        };
    }
}