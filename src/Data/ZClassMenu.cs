using CS2ZombiePlague.Config.Zombie;
using CS2ZombiePlague.Data.ZClasses;
using CS2ZombiePlague.Di;
using Microsoft.Extensions.Options;
using SwiftlyS2.Core.Menus.OptionsBase;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Menus;
using SwiftlyS2.Shared.Misc;

namespace CS2ZombiePlague.Data;

public class ZClassMenu
{
    private readonly Dictionary<int, IZClassConfig> _playersZClass = new();
    private IMenuAPI _menu;

    private ISwiftlyCore _core;
    private readonly ZClassConfig _config;

    public ZClassMenu(ISwiftlyCore core, IOptions<ZClassConfig> config)
    {
        _core = core;
        _config = config.Value;
    }

    public void RegisterHooks()
    {
        _menu = CreateMenu();
        _core.GameEvent.HookPost<EventPlayerChat>(PlayerChatEvent);
    }

    public IZombieClass GetPlayerZombieClass(int playerId)
    {
        if (_playersZClass.TryGetValue(playerId, out var zombieClassConfig))
        {
            return zombieClassConfig switch
            {
                ZombieCleric => DependencyManager.GetService<ZCleric>(),
                ZombieAssassin => DependencyManager.GetService<ZAssassin>(),
                ZombieHeavy => DependencyManager.GetService<ZHeavy>(),
                ZombieHunter => DependencyManager.GetService<ZHunter>(),
                ZombieShaman => DependencyManager.GetService<ZShaman>(),
            };
        }

        return DependencyManager.GetService<ZCleric>();
    }

    private IMenuAPI CreateMenu()
    {
        var builder = _core.MenusAPI.CreateBuilder()
            .Design.SetMenuTitle("Зомби-классы")
            .EnableSound();

        void AddZClassOption(IZClassConfig config)
        {
            var button = new ButtonMenuOption($"{config.DisplayName} {config.Description}");

            button.Click += (_, args) =>
            {
                _playersZClass[@args.Player.PlayerID] = config;
                _core.MenusAPI.CloseMenuForPlayer(@args.Player, _menu);
                _core.PlayerManager.SendCenterAsync($"{config.DisplayName} успешно выбран!");
                return ValueTask.CompletedTask;
            };
            builder.AddOption(button);
        }

        AddZClassOption(_config.Cleric);
        AddZClassOption(_config.Assassin);
        AddZClassOption(_config.Heavy);
        AddZClassOption(_config.Hunter);
        AddZClassOption(_config.Shaman);

        return builder.Build();
    }

    private HookResult PlayerChatEvent(EventPlayerChat @event)
    {
        if (@event.Text == "!class")
        {
            _core.MenusAPI.OpenMenuForPlayer(@event.UserIdPlayer, _menu);
        }

        return HookResult.Continue;
    }
}