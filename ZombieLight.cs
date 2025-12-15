using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2ZombiePlague
{
    public class ZombieLight : ZombieClass
    {
        public override string DisplayName => "Лёгкий зомби";
        public override string IternalName => "zombie_light";
        public override int Health { get; set; }
        public override float Speed { get; set; }
        public override float Gravity { get; set; }

        private CCSPlayerController _player;
        private CCSPlayerPawn _pawn;

        public ZombieLight(CCSPlayerController playerController)
        {
            _player = playerController;
            if (_player.PlayerPawn.Value != null)
            {
                _pawn = _player.PlayerPawn.Value;
            }
        }
    }
}
