
public enum WeaponEnum
{
    None = 0,
    Pistol,
    AssaultRifle,
    Shotgun,
    Grenade,
    Knife,

    Num
}

public enum WeaponStatus
{
    None = 0,
    Init,
    Reload,
    Change,
    OK,
}

//武器动作类型
public enum WeaponActionType
{
    Custom,
    Firearm,
    Melee,
    Thrown
}

//武器握把
public enum WeaponGrip 
{
    Custom,
    OneHanded,
    TwoHanded,
    TwoHandedHeavy
}