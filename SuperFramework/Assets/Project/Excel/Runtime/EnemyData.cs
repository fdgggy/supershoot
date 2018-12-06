using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class EnemyData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { id = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { name = value;} }
  
  [SerializeField]
  string note;
  public string Note { get {return note; } set { note = value;} }
  
  [SerializeField]
  int enemytype;
  public int Enemytype { get {return enemytype; } set { enemytype = value;} }
  
  [SerializeField]
  string prefabname;
  public string Prefabname { get {return prefabname; } set { prefabname = value;} }
  
  [SerializeField]
  int level;
  public int Level { get {return level; } set { level = value;} }
  
  [SerializeField]
  int weaponid;
  public int Weaponid { get {return weaponid; } set { weaponid = value;} }
  
  [SerializeField]
  int aiid;
  public int AIID { get {return aiid; } set { aiid = value;} }
  
}