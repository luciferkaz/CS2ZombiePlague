using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public class ZombieController(ZombieClass zombieClass)
{
    public void ApplyState(IPlayer player)
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

        player.SwitchTeam(Team.T);        
    }
        
    public bool TryInfect(IPlayer target)
    {
        if (target != null && !target.IsInfected() && !target.IsLastHuman())
        {
            Infect(target);
            return true;
        }
        return false;
    }

    public ZombieClass GetZombieClass()
    {
        return zombieClass;
    }
        
    private void Infect(IPlayer target)
    {
        CS2ZombiePlague.ZombieManager.CreateZombie(target);
    }
        
        
}