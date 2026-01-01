using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared.Natives;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public class ZombiePlayer
{
    private readonly ZombieManager _zombieManager;
    private readonly ZombieClass _zombieClass;
    private readonly IPlayer _player;
    public bool IsNemesis { get; }

    public ZombiePlayer(IPlayer player, ZombieManager zombieManager, ZombieClass zombieClass, bool isNemesis = false)
    {
        IsNemesis = isNemesis;
        _zombieManager = zombieManager;
        _zombieClass = zombieClass;
        _player = player;

        Initialize(player, zombieClass);
        
        player.SendAlert("Ваш класс => " + zombieClass.DisplayName);
    }

    public bool Infect(IPlayer target)
    {
        if (target != null && !target.IsInfected() && !target.IsLastHuman() && target.PlayerPawn.ArmorValue == 0)
        {
            _zombieManager.CreateZombie(target, _player.PlayerID, target.PlayerID);
            return true;
        }

        return false;
    }

    public ZombieClass GetZombieClass()
    {
        return _zombieClass;
    }

    private void Initialize(IPlayer player, ZombieClass zombieClass)
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
}