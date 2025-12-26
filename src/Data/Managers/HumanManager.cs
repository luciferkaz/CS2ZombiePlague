using CS2ZombiePlague.src.Data.Extensions;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.src.Data.Managers
{
    public class HumanManager(ISwiftlyCore _core)
    {
        public List<IPlayer> GetAllHumans()
        {
            List<IPlayer> humans = new();

            var allPlayers = _core.PlayerManager.GetAllPlayers();
            foreach (var player in allPlayers)
            {
                if (player != null && !player.IsInfected() && player.Controller.PawnIsAlive)
                {
                    humans.Add(player);
                }
            }
            return humans;
        }

        public int GetCountHumans()
        {
            var humans = GetAllHumans();
            return humans.Count;
        }
    }
}
