using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class LevelData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { id = value;} }
  
  [SerializeField]
  string levelname;
  public string Levelname { get {return levelname; } set { levelname = value;} }
  
  [SerializeField]
  int mapid;
  public int Mapid { get {return mapid; } set { mapid = value;} }
  
  [SerializeField]
  int leveltype;
  public int Leveltype { get {return leveltype; } set { leveltype = value;} }
  
  [SerializeField]
  string levelai;
  public string Levelai { get {return levelai; } set { levelai = value;} }
  
}