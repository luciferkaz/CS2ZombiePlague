namespace CS2ZombiePlague.Data.Abilities;

/*public class Leap : IZAbility
{
    private readonly ISwiftlyCore _core;
    private IPlayer? _caster;
    private readonly Utils _utils;
    
    
    private bool IsActive = false;
    private const float LeapDistance = 750f;
    private const float Cooldown = 5.0f;
    private const float zBoost = 300.0f;
    
    public void SetCaster(IPlayer caster)
    {
        _caster = caster;
    }

    public void UnHookAbility()
    {
        _core.Event.OnClientKeyStateChanged -= OnClientKeyStateChanged;
    }

    public void Use()
    {
        IsActive = true;
        
        var casterPawn = _caster.RequiredPlayerPawn;
        var viewAngles = casterPawn.EyeAngles;
        var forward = ForwardFromAngles(viewAngles);
        
        var leapVelocity = forward * LeapDistance;
        
        forward.Z = zBoost;
        
        _caster.Teleport(casterPawn.AbsOrigin, _caster.PlayerPawn.EyeAngles, leapVelocity);
        
        _core.Scheduler.DelayBySeconds(Cooldown, () =>
        {
            IsActive = false;
        });
    }

    public Leap(ISwiftlyCore core, Utils utils)
    {
        _core = core;
        _utils = utils;
        
        core.Event.OnClientKeyStateChanged += OnClientKeyStateChanged;
    }
    
    private void OnClientKeyStateChanged(IOnClientKeyStateChangedEvent @event)
    {
        
        if (IsActive)
        {
            return;
        }
        
        var player = _core.PlayerManager.GetPlayer(@event.PlayerId);

        if (player is not { IsValid: true } || !player.Controller.PawnIsAlive || !player.Equals(_caster))
        {
            return;
        }

        var isPressed = @event.Pressed;
        var key = @event.Key;

        if (key == KeyKind.Ctrl && isPressed && _caster.RequiredPawn.GroundEntity.Value == null)
        {
            Use();
        }
    }
    
    private static Vector ForwardFromAngles(QAngle angles)
    {
        const float deg2Rad = MathF.PI / 180f;

        var pitch = angles.Pitch * deg2Rad;
        var yaw = angles.Yaw * deg2Rad;

        var cosPitch = MathF.Cos(pitch);

        return new Vector(
            cosPitch * MathF.Cos(yaw),
            cosPitch * MathF.Sin(yaw),
            -MathF.Sin(pitch)
        );
    }
}*/