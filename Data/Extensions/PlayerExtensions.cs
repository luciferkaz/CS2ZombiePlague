using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace CS2ZombiePlague.Data.Extension
{
    public static class PlayerExtensions
    {
        public static void SetHealth(this CCSPlayerController controller, int health = 100)
        {
            var pawn = controller.PlayerPawn.Value;
            if (!controller.PawnIsAlive || pawn == null) return;

            pawn.Health = health;

            Utilities.SetStateChanged(pawn, "CBaseEntity", "m_iHealth");
        }
        public static void SetArmor(this CCSPlayerController controller, int armor = 100)
        {
            var pawn = controller.PlayerPawn.Value;
            if (!controller.PawnIsAlive || pawn == null) return;

            pawn.ArmorValue = armor;

            Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_ArmorValue");
        }
        // Стандартное значение скорости 250
        public static void SetSpeed(this CCSPlayerController controller, float speed = 250)
        {
            var pawn = controller.PlayerPawn.Value;
            if (!controller.PawnIsAlive || pawn == null) return;

            pawn.VelocityModifier = speed / 250;
            Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flVelocityModifier");
        }
        // Стандартное значение гравитации 800
        public static void SetGravity(this CCSPlayerController controller, float gravity = 800)
        {
            var pawn = controller.PlayerPawn.Value;
            if (!controller.PawnIsAlive || pawn == null) return;

            pawn.GravityScale = gravity / 800;
            pawn.ActualGravityScale = gravity / 800;
            Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flActualGravityScale");
            Utilities.SetStateChanged(pawn, "CCSPlayerPawn", "m_flGravityScale");
        }
    }
}
