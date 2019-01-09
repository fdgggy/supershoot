using UnityEngine;
public class BaseWeapon
{
    private WeaponData weaponInfo;
    public WeaponActionType weaponActionType { get; set; }
    public WeaponGrip weaponGrip { get; set; }

    protected BaseShooter shooter;
    public GameObject weapon3rdPersonModel = null;
    protected Renderer weapon3rdPersonModelRenderer = null;
    public virtual void Init(WeaponData weaponInfo, EntityCam wpCam)
    {
        this.weaponInfo = weaponInfo;
        weapon3rdPersonModel = vp_Utility.GetTransformByNameInChildren(wpCam.gameObject.transform.root, weaponInfo.Mountname, true).gameObject;
        if (weapon3rdPersonModel == null)
        {
            Loger.Error("BaseWeapon not find the mountName:{0}", weaponInfo.Mountname);
            return;
        }

        weapon3rdPersonModelRenderer = weapon3rdPersonModel.GetComponent<Renderer>();

        if ((WeaponEnum)weaponInfo.Weapontype == WeaponEnum.Pistol)
        {
            this.shooter = new PistolShooter();
            this.shooter.Init(weaponInfo, wpCam, weapon3rdPersonModel);
        }
    }

    public void Active(bool active = true)
    {
        weapon3rdPersonModelRenderer.enabled = active;
        vp_Utility.Activate(weapon3rdPersonModel, active);
    }

    public virtual void Fire()
    {

    }
}
