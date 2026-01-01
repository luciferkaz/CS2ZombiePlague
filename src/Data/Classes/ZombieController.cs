using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public class ZombieController(ZombieClass zombieClass)
{
    public void InitializeZombiePlayer(IPlayer player)
    {
        player.SetHealth(zombieClass.Health);
        player.SetSpeed(zombieClass.Speed);
        player.SetGravity(zombieClass.Gravity);
        player.SetModel(zombieClass.ZombieModel);

        var itemServices = player.PlayerPawn?.ItemServices;
        if (itemServices != null)
        {
            itemServices.RemoveItems();
            itemServices.GiveItem("weapon_knife");
        }

        player.PlayerPawn.Render = new Color(255, 0, 0);

        player.SwitchTeam(Team.T);
    }

    public ZombieClass GetZombieClass()
    {
        return zombieClass;
    }

    public void Infect(IPlayer target)
    {
        CS2ZombiePlague.ZombieManager.CreateZombie(target);
    }
}