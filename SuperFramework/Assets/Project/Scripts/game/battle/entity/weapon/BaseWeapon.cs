
public class BaseWeapon
{
    private WeaponData weaponInfo;
    private BaseShooter shooter;

    public virtual void Init(WeaponData weaponInfo)
    {
        this.weaponInfo = weaponInfo;
        if ((WeaponEnum)weaponInfo.Weapontype == WeaponEnum.Pistol)
        {
            this.shooter = new PistolShooter();
            this.shooter.Init(weaponInfo);
        }
    }
    public virtual void Fire()
    {

    }
}
