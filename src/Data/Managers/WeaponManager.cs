using CS2ZombiePlague.Data.Weapons;
using CS2ZombiePlague.Data.Weapons.Grenades;
using SwiftlyS2.Shared;

namespace CS2ZombiePlague.Data.Managers;

public class WeaponManager(ISwiftlyCore core, RoundManager roundManager, CommonUtils commonUtils)
{
    private readonly Dictionary<string, ICustomWeapon> _customWeapons = new();
    
    public void RegisterWeapons()
    {
        Register(new FrostNade(core, roundManager, commonUtils));
        Register(new BarrierNade(core, roundManager, commonUtils));
        
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