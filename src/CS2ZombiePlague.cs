using CS2ZombiePlague.src.Data.Classes;
using CS2ZombiePlague.src.Data.Roundes;
using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Plugins;

namespace CS2ZombiePlague
{
    [PluginMetadata(Id = "CS2ZombiePlague", Version = "1.0.0", Name = "CS2ZombiePlague", Author = "illusion&fdrinv", Description = "No description.")]
    public partial class CS2ZombiePlague : BasePlugin
    {
        private ServiceProvider? _provider;

        public static ZombieManager ZombieManagement = null!;
        public static RoundManager RoundManager = null!;
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
              .AddSingleton<RoundManager>();

            _provider = services.BuildServiceProvider();

            ZombieManagement = _provider.GetRequiredService<ZombieManager>();
            RoundManager = _provider.GetRequiredService<RoundManager>();

            RegisterRounds();

            Core.GameEvent.HookPost<EventRoundStart>(OnRoundStart);
            Core.GameEvent.HookPost<EventRoundEnd>(OnRoundEnd);
        }

        private void RegisterRounds()
        {
            RoundManager.Register(new Infection(Core));
        }

        public override void Unload()
        {

        }

        public HookResult OnRoundStart(EventRoundStart @event)
        {
            ZombieManagement.RemoveAll();
            if (RoundManager.GameIsAvailable())
            {
                RoundManager.SelectRound(0);
            }

            return HookResult.Continue;
        }

        public HookResult OnRoundEnd(EventRoundEnd @event)
        {
            if (RoundManager.GameIsAvailable() && RoundManager.GetRound() != null)
            {
                RoundManager.GetRound().End();
            }

            return HookResult.Continue;
        }

        [EventListener<EventDelegates.OnWeaponServicesCanUseHook>]
        public void OnItemServicesCanAcquireHook(IOnWeaponServicesCanUseHookEvent @event)
        {
            if (@event.WeaponServices.Pawn != null)
            {
                if (@event.WeaponServices.Pawn.Health > 1000 && @event.Weapon.DesignerName != "weapon_knife")
                {
                    @event.SetResult(false);
                }
            }
        }


    }
}