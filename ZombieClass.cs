using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2ZombiePlague
{
    abstract public class ZombieClass
    {
        public abstract string DisplayName { get; }
        public abstract string IternalName { get; }
        public abstract int Health {  get; set; }
        public abstract float Speed { get; set; }
        public abstract float  Gravity { get; set; }
    }
}
