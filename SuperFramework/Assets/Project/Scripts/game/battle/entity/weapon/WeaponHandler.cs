using System.Collections.Generic;

public class WeaponHandler
{
    private List<BaseWeapon> weapons = null;
    private BaseWeapon currentWeapon = null;
    private WeaponStatus weaponStatus = WeaponStatus.None;
    public void Init(string weaponIds)
    {
        weaponStatus = WeaponStatus.Init;

        weapons = new List<BaseWeapon>();

        Weapon weapon = ExcelDataManager.Instance.GetExcel(ExcelType.Weapon) as Weapon;
        foreach (var id in weaponIds.Split('|'))
        {
            WeaponData weaponData = weapon.QueryByID(int.Parse(id));
            if ((WeaponEnum)weaponData.Weapontype == WeaponEnum.Pistol)
            {
                BaseWeapon wp = new PistolWeapon();
                wp.Init(weaponData);

                weapons.Add(wp);
            }
        }

        InitWeapon();
    }

    public void Update()
    {

    }

    private void InitWeapon()
    {
        currentWeapon = weapons[0];
        weaponStatus = WeaponStatus.OK;
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
