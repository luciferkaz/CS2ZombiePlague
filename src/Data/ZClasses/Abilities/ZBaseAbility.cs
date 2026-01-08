using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.SchemaDefinitions;

namespace CS2ZombiePlague.Data.ZClasses.Abilities;

public abstract class ZBaseAbility(ISwiftlyCore core) : IZAbility, ICooldownRestricted
{
    protected IPlayer Caster { get; private set; } = null!;

    private bool IsActive { get; set; }

    protected abstract bool IsPassive { get; }

    protected abstract KeyKind Key { get; }

    protected CParticleSystem? Particle { get; set; }

    public virtual float Cooldown => 0;
    private CancellationTokenSource? _cooldownToken;
    private float _cooldownElapsedTime;

    private const float TickInterval = 0.5f;
    private bool _isHooked;

    public void SetCaster(IPlayer caster)
    {
        Caster = caster;
        Hook();
    }

    /// <summary>
    /// Реальная логика способности. Гарантирует, что сюда мы попадём только когда можно.
    /// </summary>
    protected abstract void Use();

    protected abstract void OnClientClickButtonHandler(int playerId, KeyKind key, bool pressed);

    /// <summary>
    /// Доп. визуал/сайд-эффекты на применение.
    /// </summary>
    protected virtual void CreateParticle(IPlayer player)
    {
    }

    /// <summary>
    /// Единая точка: попробовать применить способность.
    /// </summary>
    protected bool TryUse()
    {
        if (IsActive) return false;

        if (Caster is not { IsValid: true, Controller.PawnIsAlive: true }) return false;

        if (!CanUse(Caster)) return false;

        Use();
        return true;
    }
    
    /// <summary>
    /// Хук для специфичных ограничений способности (например, "только заражённым").
    /// </summary>
    protected virtual bool CanUse(IPlayer player) => true;
    
    public void UnHook()
    {
        if (IsPassive)
        {
            return;
        }

        if (!_isHooked)
        {
            return;
        }

        core.Event.OnClientKeyStateChanged -= OnClientKeyStateChanged;
        _isHooked = false;
    }

    public void Hook()
    {
        if (IsPassive)
        {
            return;
        }

        if (_isHooked)
        {
            return;
        }

        core.Event.OnClientKeyStateChanged += OnClientKeyStateChanged;
        _isHooked = true;
    }

    private void OnClientKeyStateChanged(IOnClientKeyStateChangedEvent @event)
    {
        if (!IsActive && Caster is { IsValid: true, Controller.PawnIsAlive: true } && Caster.PlayerID == @event.PlayerId)
        {
            OnClientClickButtonHandler(@event.PlayerId, @event.Key, @event.Pressed);
        }
    }

    /// <summary>
    /// Перевести способность в "active" и запустить кулдаун (если задан).
    /// Вызывай это из Use(), когда способность действительно успешно сработала.
    /// </summary>
    public void StartCooldown()
    {
        if (Cooldown <= 0f)
        {
            return;
        }

        IsActive = true;
        StopCooldownTimerInternal();
        _cooldownElapsedTime = 0f;

        _cooldownToken = core.Scheduler.RepeatBySeconds(TickInterval, () =>
        {
            _cooldownElapsedTime += TickInterval;

            if (ShouldResetCooldown())
            {
                ResetCooldown();
            }
        });
    }

    public virtual bool ShouldResetCooldown()
    {
        return _cooldownElapsedTime >= Cooldown;
    }

    public virtual void ResetCooldown()
    {
        IsActive = false;
        _cooldownToken?.Cancel();
        _cooldownElapsedTime = 0f;
    }
    
    private void StopCooldownTimerInternal()
    {
        try
        {
            _cooldownToken?.Cancel();
        }
        catch
        {
            // игнорируем, чтобы не падать при гонках scheduler'а
        }
        finally
        {
            _cooldownToken = null;
        }
    }
    
    protected void DestroyParticle()
    {
        try
        {
            Particle?.Despawn();
        }
        catch
        {
            // не даём визуалу ломать геймплей
        }
        finally
        {
            Particle = null;
        }
    }
}