using UnityEngine;
public class BaseShooter
{
    protected WeaponData weaponInfo;
    protected float nextAllowedFireTime = 0.0f;

    protected EntityCam weaponCam = null;
    protected GameObject muzzleFlash = null;

    public virtual void Init(WeaponData weaponInfo, EntityCam wpCam)
    {
        this.weaponInfo = weaponInfo;
        nextAllowedFireTime = Time.time;
        weaponCam = wpCam;
    }

    protected virtual bool CanFire()
    {
        return Time.time > nextAllowedFireTime;
    }

    private void PlayFireSound()
    {
        AudioManager.Instance.PlayBattleAudio(weaponInfo.Sound, false);
    }

    protected virtual void SpawnProjectiles()
    {
        if (string.IsNullOrEmpty(weaponInfo.Projectilename))
        {
            Loger.Error("SpawnProjectiles error, weaponid:{0} Projectilename is null ", weaponInfo.Id);
            return;
        }

        ResManager.Instance.LoadPrefab(weaponInfo.Projectilename, (string asstName, object original) =>
        {
            GameObject go = null;
            go = vp_Utility.Instantiate((original as UnityEngine.Object), weaponCam.gameObject.transform.position, weaponCam.gameObject.transform.rotation) as GameObject;
            go.SetActive(true);
        });
    }

    protected virtual void EjectShell()
    {
        if (string.IsNullOrEmpty(weaponInfo.Projectileshellname))
        {
            Loger.Error("EjectShell error, weaponid:{0} Projectileshellname is null ", weaponInfo.Id);
            return;
        }

        ResManager.Instance.LoadPrefab(weaponInfo.Projectileshellname, (string asstName, object original) =>
        {
            Transform ejectSpawn = vp_Utility.GetTransformByNameInChildren(weaponCam.gameObject.transform.root, "shell");

            GameObject go = null;
            go = vp_Utility.Instantiate((original as UnityEngine.Object), ejectSpawn.position, ejectSpawn.rotation) as GameObject;
            go.SetActive(true);
            go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            vp_Layer.Set(go.gameObject, vp_Layer.Debris);

            Rigidbody rigidBody = go.GetComponent<Rigidbody>();
            if (rigidBody == null)
            {
                rigidBody = go.AddComponent<Rigidbody>();
            }

            Vector3 force = ejectSpawn.forward.normalized * 0.5f;
            rigidBody.AddForce(force, ForceMode.Impulse);
        });
    }

    protected virtual void ShowMuzzleFlash()
    {
        if (string.IsNullOrEmpty(weaponInfo.Muzzlename))
        {
            Loger.Error("ShowMuzzleFlash error, weaponid:{0} Muzzlename is null ", weaponInfo.Id);
            return;
        }

        if (muzzleFlash != null)
        {
            muzzleFlash.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
            return;

        }
        ResManager.Instance.LoadPrefab(weaponInfo.Muzzlename, (string asstName, object original) =>
        {
            Transform ejectSpawn = vp_Utility.GetTransformByNameInChildren(weaponCam.gameObject.transform.root, "Muzzle");

            muzzleFlash = vp_Utility.Instantiate((original as UnityEngine.Object), ejectSpawn.position, ejectSpawn.rotation) as GameObject;
            muzzleFlash.SetActive(true);
            muzzleFlash.transform.SetParent(ejectSpawn);

            muzzleFlash.transform.position = ejectSpawn.transform.position;
            muzzleFlash.transform.rotation = ejectSpawn.transform.rotation;

            muzzleFlash.SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
        });
    }

    public virtual void Fire()
    {
        if (CanFire() == false)
        {
            //Loger.Error("not CanFire");
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
