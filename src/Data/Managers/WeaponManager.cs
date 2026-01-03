using CS2ZombiePlague.Data.Managers;
using CS2ZombiePlague.Data.Weapons.Grenades;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Weapons;

public class WeaponManager(ISwiftlyCore core, RoundManager roundManager, Utils utils)
{
    private readonly Dictionary<string, ICustomWeapon> _customWeapons = new ();
    
    public void RegisterWeapons()
    {
        Register(new FrostNade(core, roundManager, utils));
        Register(new BarrierNade(core, roundManager, utils));
        
        LoadAll();
    }
    
    private void LoadAll()
    {
        foreach (var weapon in _customWeapons.Values)
        {
            weapon.Load();
        }
    }
    
    private void Register(ICustomWeapon weapon)
    {
        _customWeapons[weapon.OriginalName] = weapon;
    }
}