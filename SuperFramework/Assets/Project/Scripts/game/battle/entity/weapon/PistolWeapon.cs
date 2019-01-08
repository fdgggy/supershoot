
public class PistolWeapon : BaseWeapon
{
    public override void Init(WeaponData weaponInfo, EntityCam wpCam)
    {
        base.Init(weaponInfo, wpCam);
        weaponActionType = WeaponActionType.Firearm;
        weaponGrip = WeaponGrip.OneHanded;
    }

    public override void Fire()
    {
        shooter.Fire();
    }
}
