using System;
using System.Collections.Generic;
using System.Text;

namespace CS2ZombiePlague.src.Data.Abilities
{
    abstract public class BaseAbility
    {
        public abstract string IternalName { get; }
        public abstract string DisplayName { get; }
        public abstract string Description { get; }
        public abstract float Cooldown { get; }
        public abstract bool HasCooldown { get; }
    }
}
