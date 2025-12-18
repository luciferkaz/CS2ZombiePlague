using CounterStrikeSharp.API.Core;
using CS2ZombiePlague.Data.Extension;

namespace CS2ZombiePlague.Data.Classes
{
    public class ZombiePlayer
    {
        private ZombieClass _zombieClass;
        private CCSPlayerController _player;

        public ZombiePlayer(ZombieClass zombieClass, CCSPlayerController player)
        {
            _zombieClass = zombieClass;
            _player = player;

            Intstalize();
        }

        private void Intstalize()
        {
            _player.SetHealth(_zombieClass.Health);
            _player.SetSpeed(_zombieClass.Speed);
            _player.SetGravity(_zombieClass.Gravity);
        }

        public CCSPlayerController GetPlayerController()
        {
            return _player;
        }
    }
}
