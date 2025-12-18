namespace CS2ZombiePlague.Data.Classes
{
    public class ZombieHeavy : ZombieClass
    {
        public override string IternalName => "zombie_heavy";
        public override string DisplayName => "Zombie Heavy";
        public override string ZombieModel => "characters/players/zombie_models/zombie_heavy/zombie_heavy.vmdl";
        public override string ClawsModel => "characters/players/zombie_models/zombie_heavy/zombie_heavy_claws.vmdl";
        public override int Health => 4000;
        public override float Speed => 240.0f;
        public override float Knockback => 1.0f;
        public override int Gravity => 900;
    }

}
