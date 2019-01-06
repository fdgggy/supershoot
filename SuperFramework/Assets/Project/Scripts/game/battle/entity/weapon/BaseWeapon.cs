
public class BaseWeapon
{
    private WeaponData weaponInfo;
    protected BaseShooter shooter;

    public virtual void Init(WeaponData weaponInfo, vp_FPCamera wpCam)
    {
        this.weaponInfo = weaponInfo;
        if ((WeaponEnum)weaponInfo.Weapontype == WeaponEnum.Pistol)
        {
            this.shooter = new PistolShooter();
            this.shooter.Init(weaponInfo, wpCam);
        }
    }
    public virtual void Fire()
    {

    }
}
