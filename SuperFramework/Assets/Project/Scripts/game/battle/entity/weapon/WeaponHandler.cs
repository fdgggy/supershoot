using System.Collections.Generic;

public class WeaponHandler
{
    private List<BaseWeapon> weapons = null;
    private BaseWeapon currentWeapon = null;
    private WeaponStatus weaponStatus = WeaponStatus.None;

    protected vp_PlayerEventHandler eventHandler = null;

    //这两个变量有动画LateUpdate调用
    public int OnValue_CurrentWeaponType
    {
        get
        {
            return (int)currentWeapon.weaponActionType;
        }
    }

    public int OnValue_CurrentWeaponGrip
    {
        get
        {
            return (int)currentWeapon.weaponGrip;
        }
    }

    public void OnDisable()
    {
        eventHandler.Unregister(this);
    }

    public void Init(string weaponIds, EntityCam wpCam)
    {
        eventHandler = (vp_PlayerEventHandler)wpCam.transform.root.GetComponent(typeof(vp_FPPlayerEventHandler));
        eventHandler.Register(this);

        weaponStatus = WeaponStatus.Init;
        weapons = new List<BaseWeapon>();
        Weapon weapon = ExcelDataManager.Instance.GetExcel(ExcelType.Weapon) as Weapon;
        foreach (var id in weaponIds.Split('|'))
        {
            WeaponData weaponData = weapon.QueryByID(int.Parse(id));
            if ((WeaponEnum)weaponData.Weapontype == WeaponEnum.Pistol)
            {
                BaseWeapon wp = new PistolWeapon();
                wp.Init(weaponData, wpCam);

                weapons.Add(wp);
            }
        }

        ChangeWeapon(0);
        weaponStatus = WeaponStatus.OK;
    }

    public void Update()
    {

    }

    public void ChangeWeapon(int index)
    {
        foreach (var weapon in weapons)
        {
            weapon.Active(false);
        }

        if (weapons[index] == null)
        {
            Loger.Error("ChangeWeapon index:{0} is not exist !", index);
            return;
        }

        currentWeapon = weapons[index];
        currentWeapon.Active(true);
    }

    public void Fire()
    {
        if (currentWeapon == null)
        {
            Loger.Error("WeaponHandler Fire, currentWeapon is null !");
            return;
        }

        if (weaponStatus != WeaponStatus.OK)
        {
            Loger.Warn("WeaponHandler Fire, weaponStatus is not OK !");
            return;
        }

        currentWeapon.Fire();
    }
}
