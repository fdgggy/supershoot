using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class WeaponData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { id = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { name = value;} }
  
  [SerializeField]
  int weapontype;
  public int Weapontype { get {return weapontype; } set { weapontype = value;} }
  
  [SerializeField]
  string picture;
  public string Picture { get {return picture; } set { picture = value;} }
  
  [SerializeField]
  string introduce;
  public string Introduce { get {return introduce; } set { introduce = value;} }
  
  [SerializeField]
  string prefabname;
  public string Prefabname { get {return prefabname; } set { prefabname = value;} }
  
  [SerializeField]
  int hurt;
  public int Hurt { get {return hurt; } set { hurt = value;} }
  
  [SerializeField]
  float penetrate;
  public float Penetrate { get {return penetrate; } set { penetrate = value;} }
  
  [SerializeField]
  float crit;
  public float Crit { get {return crit; } set { crit = value;} }
  
  [SerializeField]
  int clipcapacity;
  public int Clipcapacity { get {return clipcapacity; } set { clipcapacity = value;} }
  
  [SerializeField]
  float firerate;
  public float Firerate { get {return firerate; } set { firerate = value;} }
  
  [SerializeField]
  float soundfiredelay;
  public float Soundfiredelay { get {return soundfiredelay; } set { soundfiredelay = value;} }
  
  [SerializeField]
  float projectilespawndelay;
  public float Projectilespawndelay { get {return projectilespawndelay; } set { projectilespawndelay = value;} }
  
  [SerializeField]
  float shellejectdelay;
  public float Shellejectdelay { get {return shellejectdelay; } set { shellejectdelay = value;} }
  
  [SerializeField]
  float muzzleflashdelay;
  public float Muzzleflashdelay { get {return muzzleflashdelay; } set { muzzleflashdelay = value;} }
  
}