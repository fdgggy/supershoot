using UnityEngine;
public class BaseWeapon
{
    private WeaponData weaponInfo;
    public WeaponActionType weaponActionType { get; set; }
    public WeaponGrip weaponGrip { get; set; }

    protected BaseShooter shooter;
    protected GameObject weaponModel = null;
    protected GameObject weaponModelParent = null;
    protected Animation weaponModelAnimation = null;

    protected GameObject weapon3rdPersonModel = null;
    protected Renderer weapon3rdPersonModelRenderer = null;
    public virtual void Init(WeaponData weaponInfo, EntityCam wpCam)
    {
        this.weaponInfo = weaponInfo;
        if ((WeaponEnum)weaponInfo.Weapontype == WeaponEnum.Pistol)
        {
            this.shooter = new PistolShooter();
            this.shooter.Init(weaponInfo, wpCam);
        }

        //ResManager.Instance.LoadPrefab(weaponInfo.Prefabname, (string asstName, object original) =>
        //{
        //    weaponModelParent = new GameObject();
        //    weaponModelParent.name = weaponInfo.Prefabname + "_parent";
        //    weaponModelParent.transform.parent = wpCam.gameObject.transform;

        //    weaponModel = (GameObject)Object.Instantiate(original as GameObject);
        //    weaponModel.transform.parent = weaponModelParent.transform;
        //    weaponModel.transform.localPosition = Vector3.zero;
        //    weaponModel.transform.localScale = Vector3.one;
        //    weaponModel.transform.localEulerAngles = Vector3.zero;
        //    weaponModelAnimation = weaponModel.GetComponent<Animation>();
        //});
        weapon3rdPersonModel = vp_Utility.GetTransformByNameInChildren(wpCam.gameObject.transform.root, weaponInfo.Mountname, true).gameObject;
        if (weapon3rdPersonModel == null)
        {
            Loger.Error("BaseWeapon not find the mountName:{0}", weaponInfo.Mountname);
            return;
        }

        weapon3rdPersonModelRenderer = weapon3rdPersonModel.GetComponent<Renderer>();
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
