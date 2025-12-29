using CS2ZombiePlague.Data.Classes;
using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace CS2ZombiePlague.src.Data.Classes;

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
            
        zombieClass.Initialize(_player,  zombieController);
        player.SendAlert("Ваш класс => " + zombieClass.DisplayName);
    }

    public void Infect(IPlayer target)
    {
        zombieController.TryInfect(target);
    }

    public ZombieClass GetZombieClass()
    {
        return zombieController.GetZombieClass();
    }
}