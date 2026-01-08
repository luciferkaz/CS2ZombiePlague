using CS2ZombiePlague.Config;
using CS2ZombiePlague.Data.Extensions;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Convars;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;

namespace CS2ZombiePlague.Data;

public class MoneySystem(ISwiftlyCore core, IOptions<ZombiePlagueCoreConfig> config)
{
    public void Start()
    {
        core.GameEvent.HookPost<EventPlayerHurt>(OnPlayerHurtPost);
        
        IConVar<int>? convar = core.ConVar.Find<int>("mp_maxmoney");
        convar.Value = config.Value.MaxMoney;
    }

    private HookResult OnPlayerHurtPost(EventPlayerHurt @event)
    {
        var player = @event.AttackerPlayer;
        var victim = @event.UserIdPlayer;

        if (player == null || !player.IsValid || @event.UserIdPlayer == null)
            return HookResult.Continue;

        if (player.IsInfected() || victim.PlayerPawn.Team == player.PlayerPawn.Team)
        {
            return HookResult.Continue;
        }

        var playerMoneyService = player.Controller.InGameMoneyServices;
        if (playerMoneyService == null)
        {
            return HookResult.Continue;
        }

        var currentMoney = playerMoneyService.Account;
        var additionalMoney = (int)@event.DmgHealth;

        playerMoneyService.Account = currentMoney + additionalMoney;
        playerMoneyService.AccountUpdated();

        return HookResult.Continue;
    }
}