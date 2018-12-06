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
  float firingrate;
  public float Firingrate { get {return firingrate; } set { firingrate = value;} }
  
  [SerializeField]
  int clipcapacity;
  public int Clipcapacity { get {return clipcapacity; } set { clipcapacity = value;} }
  
}