using CS2ZombiePlague.Data.Extensions;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Classes;

public class ZombiePlayer
{
    private readonly IPlayer _player;
    private readonly ZombieController zombieController;
    public bool IsNemesis { get; }

    public ZombiePlayer(ZombieClass zombieClass, IPlayer player, bool isNemesis = false)
    {
        _player = player;
        IsNemesis = isNemesis;
        zombieController = new ZombieController(zombieClass);

        zombieClass.Initialize(_player, zombieController);
        player.SendAlert("Ваш класс => " + zombieClass.DisplayName);
    }

    public bool Infect(IPlayer target)
    {
        if (target != null && !target.IsInfected() && !target.IsLastHuman() && target.PlayerPawn.ArmorValue == 0)
        {
            zombieController.Infect(target);
            return true;
        }

        return false;
    }

    public ZombieClass GetZombieClass()
    {
        return zombieController.GetZombieClass();
    }
}