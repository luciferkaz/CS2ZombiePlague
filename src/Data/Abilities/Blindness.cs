using Mono.Cecil.Cil;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.GameEvents;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;
using System;
using System.Collections.Generic;
using System.Text;
using static SwiftlyS2.Shared.Events.EventDelegates;

namespace CS2ZombiePlague.src.Data.Abilities
{
    public class Blindness : BaseAbility
    {
        public override string IternalName => "zm_ability_blindess";

        public override string DisplayName => "Blindess";

        public override string Description => "";

        public override float Cooldown => 15.0f;
        public override bool HasCooldown => false;

        private IPlayer _owner;
        public Blindness(IPlayer owner)
        {
            _owner = owner;
        }

       

    }
}
