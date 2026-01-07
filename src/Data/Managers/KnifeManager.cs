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
        _menuApi = CreateMenu();
    }

    public IKnife GetPlayerKnife(int playerId) => _playerKnifes[playerId];

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

        if (!_playerKnifes.ContainsKey(player.PlayerID))
            _playerKnifes[player.PlayerID] = factory.Create<SpeedKnifeWeapon>();

        var weapons = pawn.WeaponServices?.MyWeapons;
        if (weapons == null)
            return HookResult.Continue;

        foreach (var weapon in weapons)
        {
            if (weapon == null || !weapon.IsValid || !weapon.Value.DesignerName.Contains(@event.Item))
                continue;

            var knife = GetPlayerKnife(player.PlayerID);
            weapon.Value.SetModel(knife.Model);

            if (pawn.WeaponServices.ActiveWeapon.Value.Address == weapon.Value.Address)
                SetKnifeProperties(knife, player);

            break;
        }

        return HookResult.Continue;
    }

    private void ClientPutInServerEvent(IOnClientPutInServerEvent @event)
    {
        _playerKnifes[@event.PlayerId] = factory.Create<GravityKnifeWeapon>();
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
                _playerKnifes[args.Player.PlayerID] = factory.Create<T>();
                args.Player.SendChatAsync("Нож успешно выбран!");
                GiveKnife(args.Player);
            };
            builder.AddOption(button);
        }

        AddKnifeOption<KnockbackKnifeWeapon>(config.Value.Knockback);
        AddKnifeOption<SpeedKnifeWeapon>(config.Value.Speed);
        AddKnifeOption<GravityKnifeWeapon>(config.Value.Gravity);

        return builder.Build();
    }

    private void GiveKnife(IPlayer player)
    {
        core.Scheduler.NextTickAsync(() =>
        {
            var pawn = player.PlayerPawn;
            if (pawn == null || !player.Controller.PawnIsAlive)
            {
                return;
            }

            pawn.WeaponServices.RemoveWeaponByDesignerName("weapon_knife");
            pawn.ItemServices.GiveItem("weapon_knife");

            foreach (var weapon in pawn.WeaponServices.MyWeapons)
            {
                if (!weapon.Value.DesignerName.Contains("knife"))
                    continue;

                weapon.Value.SetModel(GetPlayerKnife(player.PlayerID).Model);
                pawn.WeaponServices.SelectWeapon(weapon.Value);
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