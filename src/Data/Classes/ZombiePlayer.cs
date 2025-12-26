using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace CS2ZombiePlague.src.Data.Classes
{
    public class ZombiePlayer
    {
        private readonly IZombieClass _zombieClass;
        private readonly IPlayer _player;
        private readonly InfectionController infectionController;

        public bool IsNemesis { get; }

        public ZombiePlayer(IZombieClass zombieClass, IPlayer player, bool isNemesis = false)
        {
            _zombieClass = zombieClass;
            _player = player;
            infectionController = new InfectionController();

            IsNemesis = isNemesis;

            Initialize();
        }

        private void Initialize()
        {
            if (_player.PlayerPawn == null)
            {
                return;
            }

            _player.SetHealth(_zombieClass.Health);
            _player.SetSpeed(_zombieClass.Speed);
            _player.SetGravity(_zombieClass.Gravity);
            _player.SetModel(_zombieClass.ZombieModel);

            var itemServices = _player.PlayerPawn?.ItemServices;
            if (itemServices != null)
            {
                itemServices.RemoveItems();
                itemServices.GiveItem("weapon_knife");
            }

            _player.SwitchTeam(Team.T);
        }

        public void Infect(IPlayer target)
        {
            infectionController.TryInfect(target);
        }

        public IZombieClass GetZombieClass()
        {
            return _zombieClass;
        }
    }
}