using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.src.Data.Classes
{
    public class ZombiePlayer
    {
        private readonly IZombieClass _zombieClass;
        private readonly IPlayer _player;
        private readonly InfectionController infectionController;

        public ZombiePlayer(IZombieClass zombieClass, IPlayer player)
        {
            _zombieClass = zombieClass;
            _player = player;
            infectionController = new InfectionController();

            ApplyZombieState();
        }

        public void ApplyZombieState()
        {
            if (_player.PlayerPawn == null)
            {
                return; 
            }

            _player.SetHealth(_zombieClass.Health);
            _player.SetSpeed(_zombieClass.Speed);
            _player.SetGravity(_zombieClass.Gravity);
            _player.SetModel("characters/models/tm_phoenix/tm_phoenix.vmdl");

            var itemServices = _player.PlayerPawn?.ItemServices;
            if (itemServices != null)
            {
                itemServices.RemoveItems();
                itemServices.GiveItem("weapon_knife");
            }

            _player.SwitchTeam(Team.T);
        }

        public void InfectTarget(IPlayer target)
        {
            infectionController.TryInfectTarget(target);
        }

        public IZombieClass GetZombieClass() { return _zombieClass; }
    }
}
