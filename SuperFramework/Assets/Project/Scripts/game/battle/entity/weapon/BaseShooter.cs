using UnityEngine;
public class BaseShooter
{
    protected WeaponData weaponInfo;
    protected float nextAllowedFireTime = 0.0f;

    public virtual void Init(WeaponData weaponInfo)
    {
        this.weaponInfo = weaponInfo;
        nextAllowedFireTime = Time.time;
    }

    protected virtual bool CanFire()
    {
        return Time.time > nextAllowedFireTime;
    }

    private void PlayFireSound()
    {
        Loger.Info("PlayFireSound");
    }

    protected virtual void SpawnProjectiles()
    {
        Loger.Info("SpawnProjectiles");
    }

    protected virtual void EjectShell()
    {
        Loger.Info("EjectShell");
    }

    protected virtual void ShowMuzzleFlash()
    {
        Loger.Info("ShowMuzzleFlash");
    }

    protected virtual void Fire()
    {
        if (CanFire() == false)
        {
            Loger.Error("not CanFire");
            return;
        }
        nextAllowedFireTime = Time.time + weaponInfo.Firerate;

        if (weaponInfo.Soundfiredelay <= 0.0f)
        {
            PlayFireSound();
        }
        else
        {
            vp_Timer.In(weaponInfo.Soundfiredelay, PlayFireSound);
        }

        if (weaponInfo.Projectilespawndelay <= 0.0f)
        {
            SpawnProjectiles();
        }
        else
        {
            vp_Timer.In(weaponInfo.Projectilespawndelay, SpawnProjectiles);
        }

        if (weaponInfo.Shellejectdelay <= 0.0f)
        {
            EjectShell();
        }
        else
        {
            vp_Timer.In(weaponInfo.Shellejectdelay, EjectShell);
        }

        if (weaponInfo.Muzzleflashdelay <= 0.0f)
        {
            ShowMuzzleFlash();
        }
        else
        {
            vp_Timer.In(weaponInfo.Muzzleflashdelay, ShowMuzzleFlash);
        }
    }
}
