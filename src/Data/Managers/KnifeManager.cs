using CS2ZombiePlague.Config.Weapon;
using CS2ZombiePlague.Data.Extensions;
using CS2ZombiePlague.Data.Weapons.Knifes;
using Microsoft.Extensions.Options;
using SwiftlyS2.Core.Menus.OptionsBase;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.Menus;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;

namespace CS2ZombiePlague.Data.Managers;

public class KnifeManager(
    ISwiftlyCore core,
    RoundManager roundManager,
    IKnifeFactory factory,
    IOptions<KnifeConfig> config)
{
    private readonly Dictionary<int, IKnife> _playerKnifes = new();
    private IMenuAPI _menuApi = null!;

    private const float DefaultSpeed = 250f;
    private const float DefaultGravity = 800f;

    public void RegisterHooks()
    {
        core.Event.OnClientPutInServer += ClientPutInServerEvent;
        core.GameEvent.HookPost<EventItemEquip>(PlayerEquipEvent);
        core.GameEvent.HookPost<EventPlayerChat>(PlayerChatEvent);
        core.GameEvent.HookPost<EventPlayerSpawn>(PlayerSpawnEvent);
        _menuApi = CreateMenu();
    }

    public IKnife GetPlayerKnife(int playerId) => _playerKnifes[playerId];

    private void SetDefaultKnife(int playerId) => _playerKnifes[playerId] = factory.Create<GravityKnifeWeapon>();

    private HookResult PlayerSpawnEvent(EventPlayerSpawn @event)
    {
        if (@event.UserIdPlayer == null)
        {
            return HookResult.Continue;
        }

        GiveKnifeAsync(@event.UserIdPlayer);

        return HookResult.Continue;
    }

    private HookResult PlayerEquipEvent(EventItemEquip @event)
    {
        var player = @event.UserIdPlayer;
        var pawn = player?.PlayerPawn;

        if (player == null ||
            pawn == null && !player.Controller.PawnIsAlive ||
            (!roundManager.IsNoneRound() && player.Controller.Team == Team.T))
            return HookResult.Continue;

        if (@event.Item != "knife")
        {
            SetDefaultProperties(player);
            return HookResult.Continue;
        }

        var playerId = player.PlayerID;

        if (!_playerKnifes.ContainsKey(playerId))
            SetDefaultKnife(playerId);

        var weapons = pawn.WeaponServices?.MyValidWeapons;
        var eventKnifeName = @event.Item;
        foreach (var weapon in weapons)
        {
            if (!weapon.DesignerName.Contains(eventKnifeName))
                continue;

            var knife = GetPlayerKnife(playerId);
            weapon.SetModel(knife.Model);
            break;
        }

        if (pawn.WeaponServices.ActiveWeapon.Value.DesignerName.Contains(eventKnifeName))
        {
            var knife = GetPlayerKnife(playerId);
            SetKnifeProperties(knife, player);
        }

        return HookResult.Continue;
    }

    private void ClientPutInServerEvent(IOnClientPutInServerEvent @event)
    {
        SetDefaultKnife(@event.PlayerId);
    }

    private void SetKnifeProperties(IKnife knife, IPlayer player)
    {
        player.SetSpeed(knife.Speed);
        player.SetGravity(knife.Gravity);
    }

    private void SetDefaultProperties(IPlayer player)
    {
        player.SetSpeed(DefaultSpeed);
        player.SetGravity(DefaultGravity);
    }

    private IMenuAPI CreateMenu()
    {
        var builder = core.MenusAPI.CreateBuilder()
            .Design.SetMenuTitle("Выбери нож")
            .EnableSound();

        void AddKnifeOption<T>(IKnifeConfig cfg) where T : IKnife
        {
            var button = new ButtonMenuOption($"{cfg.DisplayName} {cfg.Description}");
            button.Click += async (_, args) =>
            {
                if (@args.Player == null || @args.Player.IsInfected())
                {
                    return;
                }

                _playerKnifes[args.Player.PlayerID] = factory.Create<T>();
                GiveKnifeAsync(args.Player);
            };
            builder.AddOption(button);
        }

        AddKnifeOption<KnockbackKnifeWeapon>(config.Value.Knockback);
        AddKnifeOption<SpeedKnifeWeapon>(config.Value.Speed);
        AddKnifeOption<GravityKnifeWeapon>(config.Value.Gravity);

        return builder.Build();
    }

    private void GiveKnifeAsync(IPlayer player)
    {
        core.Scheduler.NextWorldUpdateAsync(() =>
        {
            var pawn = player.PlayerPawn;
            if (pawn == null || !player.Controller.PawnIsAlive)
            {
                return;
            }

            if (player.IsInfected())
            {
                return;
            }

            pawn.WeaponServices.RemoveWeaponByDesignerName("weapon_knife");
            pawn.ItemServices.GiveItem("weapon_knife_t");
            foreach (var weapon in pawn.WeaponServices.MyValidWeapons)
            {
                if (!weapon.DesignerName.Contains("knife"))
                    continue;

                weapon.SetModel(GetPlayerKnife(player.PlayerID).Model);
                pawn.WeaponServices.SelectWeapon(weapon);

                player.SendChat("Нож успешно выбран!");
                break;
            }
        });
    }

    private HookResult PlayerChatEvent(EventPlayerChat @event)
    {
        var player = @event.UserIdPlayer;
        if (player != null && @event.Text == "!knife" && !player.IsInfected())
        {
            core.MenusAPI.OpenMenuForPlayer(player, _menuApi);
        }

        return HookResult.Continue;
    }
}