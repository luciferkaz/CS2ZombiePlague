using CS2ZombiePlague.Data.Managers;
using CS2ZombiePlague.src.Data.Extensions;
using CS2ZombiePlague.src.Data.Managers;
using CS2ZombiePlague.Data.Rounds;
using CS2ZombiePlague.Data.Utils;
using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.Plugins;

namespace CS2ZombiePlague
{
    [PluginMetadata(Id = "CS2ZombiePlague", Version = "1.0.0", Name = "CS2ZombiePlague", Author = "illusion & fdrinv",
        Description = "Zombie Plague mode for CS2")]
    public partial class CS2ZombiePlague : BasePlugin
    {
        private ServiceProvider? _provider;

        public static ZombieManager ZombieManager = null!;
        public static RoundManager RoundManager = null!;
        public static HumanManager HumanManager = null!;
        public static Utils Utils = null!;

        public CS2ZombiePlague(ISwiftlyCore core) : base(core)
        {
        }

        public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
        {
        }

        public override void UseSharedInterface(IInterfaceManager interfaceManager)
        {
        }

        public override void Load(bool hotReload)
        {
            if (hotReload)
            {
                _provider?.Dispose();
            }

            ServiceCollection services = new();
            services
                .AddSwiftly(Core)
                .AddSingleton<ZombieManager>()
                .AddSingleton<RoundManager>()
                .AddSingleton<HumanManager>()
                .AddSingleton<Utils>();

            _provider = services.BuildServiceProvider();

            ZombieManager = _provider.GetRequiredService<ZombieManager>();
            RoundManager = _provider.GetRequiredService<RoundManager>();
            HumanManager = _provider.GetRequiredService<HumanManager>();
            Utils = _provider.GetRequiredService<Utils>();

            RegisterRounds();

            Core.GameEvent.HookPost<EventRoundStart>(OnRoundStart);
            Core.GameEvent.HookPost<EventRoundEnd>(OnRoundEnd);
            Core.GameEvent.HookPre<EventPlayerHurt>(OnPlayerHurt);
        }

        private void RegisterRounds()
        {
            RoundManager.Register(new Infection(Core), RoundType.Infection);
        }

        public override void Unload()
        {
        }

        public HookResult OnRoundStart(EventRoundStart @event)
        {
            ZombieManager.RemoveAll();

            if (RoundManager.GameIsAvailable())
            {
                RoundManager.Start();
            }

            return HookResult.Continue;
        }
        
        public HookResult OnPlayerHurt(EventPlayerHurt @event)
        {
            if (RoundManager.GetRound() == null)
            {
                IPlayer victimPlayer = @event.UserIdPlayer;
                if(victimPlayer.IsValid)
                    victimPlayer.SetHealth(victimPlayer.Pawn.Health+@event.DmgHealth);
            }

            return HookResult.Continue;
        }

        public HookResult OnRoundEnd(EventRoundEnd @event)
        {
            if (RoundManager.GetRound() != null)
            {
                RoundManager.GetRound().End();
            }
            
            return HookResult.Continue;
        }

        [EventListener<EventDelegates.OnWeaponServicesCanUseHook>]
        public void OnItemServicesCanAcquireHook(IOnWeaponServicesCanUseHookEvent @event)
        {
            var player = Core.PlayerManager.GetPlayer((int)@event.WeaponServices.Pawn.Controller.EntityIndex - 1);
            if (player.IsValid)
            {
                if (player.IsInfected() && @event.Weapon.DesignerName != "weapon_knife")
                {
                    @event.SetResult(false);
                }
            }
        }
    }
}