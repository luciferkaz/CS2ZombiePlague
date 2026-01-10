namespace CS2ZombiePlague.Data.Abilities;

/*public class Charge : IZAbility
{
    private readonly ISwiftlyCore _core;
    private IPlayer? _caster;
    private readonly ZombieManager _zombieManager;

    private bool IsActive = false;

    private const float MaxSpeed = 650f;
    private const float ChargeTime = 3.0f;
    private const float Delay = 0.1f;
    private const float Cooldown = 5.0f;

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

        var startSpeed = _zombieManager.GetZombie(_caster.PlayerID).GetZombieClass().Speed;
        var deltaSpeed = (MaxSpeed - startSpeed) / ChargeTime * Delay;

        var currentSpeed = startSpeed;

        var startTime = 0f;

        CancellationTokenSource token = null!;
        token = _core.Scheduler.RepeatBySeconds(Delay, () =>
        {
            if (_caster == null && !_caster.Controller.PawnIsAlive || !_caster.IsInfected())
            {
                IsActive = false;
                token.Cancel();
            }

            if (startTime >= ChargeTime)
            {
                _core.Scheduler.NextTick(() => { _caster.SetSpeed(startSpeed); });
                IsActive = false;
                token.Cancel();
            }

            currentSpeed += deltaSpeed;
            _caster.SetSpeed(currentSpeed);

            startTime += Delay;
        });
    }

    public Charge(ISwiftlyCore core, ZombieManager zombieManager)
    {
        _core = core;
        _zombieManager = zombieManager;

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

        if (key == KeyKind.F && isPressed)
        {
            Use();
        }
    }
}*/