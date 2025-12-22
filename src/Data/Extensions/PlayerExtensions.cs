using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS2ZombiePlague.src.Data.Extensions
{
    public static class PlayerExtensions
    {
        public static void SetHealth(this IPlayer controller, int health = 100)
        {
            var pawn = controller.PlayerPawn;
            if (pawn == null || pawn.Health <= 0) return;

            pawn.Health = health;
            pawn.HealthUpdated();

        }
        public static void SetArmor(this IPlayer controller, int armor = 100)
        {
            var pawn = controller.PlayerPawn;
            if (pawn == null || pawn.Health <= 0) return;

            pawn.ArmorValue = armor;
            pawn.ArmorValueUpdated();
        }
        // Стандартное значение скорости 250
        public static void SetSpeed(this IPlayer controller, float speed = 250)
        {
            var pawn = controller.PlayerPawn;
            if (pawn == null || pawn.Health <= 0) return;

            pawn.VelocityModifier = speed / 250;
            pawn.VelocityModifierUpdated();
        }
        // Стандартное значение гравитации 800
        public static void SetGravity(this IPlayer controller, float gravity = 800)
        {
            var pawn = controller.PlayerPawn;
            if (pawn == null || pawn.Health <= 0) return;

            pawn.GravityScale = gravity / 800;
            pawn.ActualGravityScale = gravity / 800;
            pawn.GravityScaleUpdated();
        }

        public static void SetModel(this IPlayer controller, string modelPath)
        {
            var pawn = controller.PlayerPawn;
            if (pawn == null || pawn.Health <= 0) return;

            pawn.SetModel(modelPath);
        }
    }
}
