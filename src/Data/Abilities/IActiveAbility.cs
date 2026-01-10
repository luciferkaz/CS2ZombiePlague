using SwiftlyS2.Shared.Events;

namespace CS2ZombiePlague.Data.Abilities;

public interface IActiveAbility : IAbility
{
    public KeyKind? Key { get; }

    public void OnClientKeyStateChanged(IOnClientKeyStateChangedEvent @event);
}