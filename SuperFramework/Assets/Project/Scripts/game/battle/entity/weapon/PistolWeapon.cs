
public class PistolWeapon : BaseWeapon
{
    public override void Init(WeaponData weaponInfo, vp_FPCamera wpCam)
    {
        base.Init(weaponInfo, wpCam);
    }

    public override void Fire()
    {
        shooter.Fire();
    }
}
