using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Data.Managers;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.ZClasses;

public class ZombiePlayer
{
    private readonly ZombieManager _zombieManager;
    private readonly IZombieClass _zombieClass;
    private readonly IPlayer _player;
    public bool IsNemesis { get; }

    public ZombiePlayer(IPlayer player, ZombieManager zombieManager, IZombieClass zombieClass, bool isNemesis = false)
    {
        IsNemesis = isNemesis;
        _zombieManager = zombieManager;
        _zombieClass = zombieClass;
        _player = player;

        Initialize();
        
        player.SendAlert("Ваш класс => " + zombieClass.DisplayName);
    }

    public bool Infect(IPlayer target)
    {
        if (target != null && !target.IsInfected() && !target.IsLastHuman() && target.PlayerPawn.ArmorValue == 0 && !_player.IsNemesis())
        {
            _zombieManager.CreateZombie(target, _player.PlayerID, target.PlayerID);
            return true;
        }

        return false;
    }

    public IZombieClass GetZombieClass()
    {
        return _zombieClass;
    }

    public void Initialize()
    {
        _player.SetHealth(_zombieClass.Health);
        _player.SetSpeed(_zombieClass.Speed);
        _player.SetGravity(_zombieClass.Gravity);
        _player.SetModel(_zombieClass.Model);
        
        _zombieClass.Abilities.ForEach(zClass=>zClass.SetCaster(_player));
        
        _player.SwitchTeam(Team.T);
        
        var itemServices = _player.PlayerPawn?.ItemServices;
        if (itemServices != null)
        {
            itemServices.RemoveItems();
            itemServices.GiveItem("weapon_knife_t");
        }
    }

    public void UnHookAbilities()
    {
        _zombieClass.Abilities.ForEach(zClass=>zClass.UnHook());
    }
}