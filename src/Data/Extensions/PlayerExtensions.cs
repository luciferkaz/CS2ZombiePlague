using CS2ZombiePlague.src.Data.Classes;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.ProtobufDefinitions;
using SwiftlyS2.Shared.SchemaDefinitions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace CS2ZombiePlague.src.Data.Extensions
{
    public static class PlayerExtensions
    {
        public static void SetHealth(this IPlayer player, int health)
        {
            var playerPawn = player.PlayerPawn;
            if (playerPawn == null || playerPawn.Health <= 0) return;

            playerPawn.Health = health;
            playerPawn.HealthUpdated();

        }
        public static void SetArmor(this IPlayer player, int armor)
        {
            var playerPawn = player.PlayerPawn;
            if (playerPawn == null || !player.Controller.PawnIsAlive) return;

            playerPawn.ArmorValue = armor;
            playerPawn.ArmorValueUpdated();
        }
        // Стандартное значение скорости 250
        public static void SetSpeed(this IPlayer player, float speed)
        {
            var playerPawn = player.PlayerPawn;
            if (playerPawn == null || !player.Controller.PawnIsAlive) return;

            playerPawn.VelocityModifier = speed / 250;
            playerPawn.VelocityModifierUpdated();
        }
        // Стандартное значение гравитации 800
        public static void SetGravity(this IPlayer player, float gravity)
        {
            var pawn = player.Pawn;
            if (pawn == null || !player.Controller.PawnIsAlive) return;

            pawn.GravityScale = gravity / 800;
            pawn.ActualGravityScale = gravity / 800;
            pawn.GravityScaleUpdated();
        }

        public static void SetModel(this IPlayer player, string modelPath)
        {
            var pawn = player.Pawn;
            if (pawn == null || !player.Controller.PawnIsAlive) return;

            pawn.SetModel(modelPath);
        }

        public static bool IsInfected(this IPlayer player)
        {
            var allZombies = CS2ZombiePlague.ZombieManager.GetAllZombies();
            return allZombies.ContainsKey(player.PlayerID);
        }

        public static bool IsLastHuman(this IPlayer player)
        {
            return !player.IsInfected() && CS2ZombiePlague.HumanManager.GetCountHumans() == 1;
        }
    }
}
